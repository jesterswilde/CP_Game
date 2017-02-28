using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 

public class Interactable : WibblyWobbly, IAI {

    protected IAtomicAction _currentAction;
    [SerializeField]
    protected float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField]
    protected float _rotationSpeed;
    public float RotationSpeed { get { return _rotationSpeed; } }
    [SerializeField]
    protected Task _currentTask;
    protected Task _defaultTask;
    protected HistoryNode _interruptedBranch;
    protected Task _interruptedTask;
    protected Stack<InterruptedBranch> _branches = new Stack<InterruptedBranch>(); 

    public override void Play(float _time)
    {
        StartPlay(_time - _prevTime); 
        base.Play(_time);
    }
    protected virtual void StartPlay(float _deltaTime)
    {
    }
    protected override void Act(float _deltaTime)
    {
        if(_currentAction != null)
        {
            _currentAction.Act(_deltaTime);
        }else
        {
            if (_currentTask != null && _history.IsPointerAtHead())
            {
                PlayTask(_currentTask);
            } else
            {
                if (_defaultTask != null && _history.IsPointerAtHead())
                {
                    SetTask(_defaultTask);
                }
            }
        }
    }

    protected override void ActReverse(float _deltaTIme)
    {
        if(_currentAction != null)
        {
            _currentAction.ActReverse(_deltaTIme); 
        }
    }

    protected override void UseAction(Action _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.AIMoveForward:
                _currentAction = new AAMoveForward(this);
                _currentAction.UseAction(_action, _time);
                break;
            case ActionType.AIRotate:
                _currentAction = new AARotate(this);
                _currentAction.UseAction(_action, _time);
                break;
            case ActionType.AIMoveTo:
                _currentAction = new AAMoveTo(this);
                _currentAction.UseAction(_action, _time);
                break;
            case ActionType.AIWait:
                _currentAction = new AAWait();
                _currentAction.UseAction(_action, _time);
                break;
            case ActionType.AIRotateDir:
                _currentAction = new AARotateDir(this);
                _currentAction.UseAction(_action, _time);
                break; 
            case ActionType.SetTask:
                _currentTask = _action.Task;
                break;
            case ActionType.UnsetTask:
                _currentTask = null;
                break;
            case ActionType.ReloadInterruptedTask:
                if(_currentTask != null)
                {
                    SetExternalAction(new TaskAction(ActionType.UnsetTask, _currentTask));
                }
                _currentTask = _interruptedTask;
                _interruptedTask = null;
                break;
            case ActionType.ResumeBranch:
                Task.ResumeTask(_interruptedBranch, this, GameManager.FixedGameTime);
                _interruptedBranch = null;
                break; 
            case ActionType.SpliceFuture:
                _history.SpliceOffPossibleFuture();
                break; 
            case ActionType.AIMoveForwardUnset:
            case ActionType.AIMoveToUnset:
            case ActionType.AIRotateUnset:
            case ActionType.AIWaitUnset:
            case ActionType.AIRotateDirUnset:
                _currentAction = null;
                break;
            case ActionType.Interrupt:
                Interrupt(); 
                SetAction(new BasicAction(ActionType.Null, float.MaxValue, true));
                break;
            case ActionType.Resume:
                Resume(); 
                break;
        }
        if(_combat != null && _action != null)
        {
            SetAction(_combat.UseAction(_action), true); 
        }
    }

    protected override void ReverseAction(Action _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.AIMoveForwardUnset:
                _currentAction = new AAMoveForward(this);
                _currentAction.ReverseAction(_action, _time); 
                break;
            case ActionType.AIRotateUnset:
                _currentAction = new AARotate(this);
                _currentAction.ReverseAction(_action, _time);
                break; 
            case ActionType.AIMoveToUnset:
                _currentAction = new AAMoveTo(this);
                _currentAction.ReverseAction(_action, _time); 
                break;
            case ActionType.AIWaitUnset:
                _currentAction = new AAWait();
                _currentAction.ReverseAction(_action, _time); 
                break;
            case ActionType.AIRotateDirUnset:
                _currentAction = new AARotateDir(this);
                _currentAction.ReverseAction(_action, _time);
                break; 
            case ActionType.UnsetTask:
                _currentTask = _action.Task;
                break;
            case ActionType.UnsetTaskInterruption:
                _interruptedTask = _action.Task;
                break; 
            case ActionType.UnsetInterruption:
                _interruptedBranch = _action.PossibleFuture;
                break; 
            case ActionType.SetTask:
                _currentTask = null;
                break;
            case ActionType.ReloadInterruptedTask:
                _interruptedTask = _action.Task;
                break;
            case ActionType.ResumeBranch:
                _interruptedBranch = _action.PossibleFuture;
                break;
            case ActionType.Interrupt:
                _branches.Pop();
                break;
            case ActionType.Resume:
                _branches.Push(_action.Branch);
                break; 
            case ActionType.AIMoveForward:
            case ActionType.AIMoveTo:
            case ActionType.AIRotate: 
            case ActionType.AIWait:
            case ActionType.AIRotateDir:
                _currentAction = null;
                break; 
            case ActionType.SpliceFuture:
                _history.ReloadPossibleFuture(_action.PossibleFuture);
                break;
        }
        if (_combat != null)
        {
            _combat.ReverseAction(_action);
        }
    }
    protected virtual void Interrupt()
    {
        HistoryNode _branch = null;
        if(_currentAction != null)
        {
            _branch = _history.CreateBranch(_currentAction.CreateAction()); 
        }
        _branches.Push(new InterruptedBranch(_branch, _currentTask));
        _history.SpliceOffPossibleFuture();
        if (_currentAction != null)
        {
            SetExternalAction(_currentAction.Unset());
            _currentAction = null;
        }
        if (_currentTask != null)
        {
            SetExternalAction(new TaskAction(ActionType.UnsetTask, _currentTask, true));
            _currentAction = null; 
        }
    }
    protected virtual void Resume()
    {
        UnloadAll(); 
        InterruptedBranch _branch = _branches.Pop();
        _currentTask = _branch.Task;
        Task.ResumeTask(_branch.Branch, this, GameManager.FixedGameTime);
    }
    protected virtual void UnloadAll()
    {
        _history.SpliceOffPossibleFuture();
        if (_currentAction != null)
        {
            SetExternalAction(_currentAction.Unset());
            _currentAction = null; 
        }
        if (_currentTask != null)
        {
            SetExternalAction(new TaskAction(ActionType.UnsetTask, _currentTask, true));
            _currentTask = null; 
        }
    }
    public void SetAtomicAction(ActionType _type, IAI _ai, Vector3 _target, float _time)
    {
        _history.SpliceOffPossibleFuture();
        if (_currentAction != null)
        {
            SetAction(_currentAction.Unset());
        }
        switch (_type)
        {
            case ActionType.AIRotate:
                AARotate.SimulateAction(_ai, _target, _time);
                break; 
        } 
    }
    public virtual void ExternalTrigger(Task _task, TriggerType _trigger, Character _character  )
    {
        if (GameManager.IsPlaying)
        {
            if(_task != null)
            {
                SetTask(_task);
            }else
            {
                switch (_trigger)
                {
                    case TriggerType.Interrupt:
                        SetExternalAction(new BasicAction(ActionType.Interrupt, true));
                        break;
                    case TriggerType.Resume:
                        SetExternalAction(new BranchAction(ActionType.Resume, _branches.Peek() , true));
                        break;
                }
            }
        }
    }
    public override void SetExternalAction(Action _action)
    {
        base.SetExternalAction(_action);
    }
    public virtual void SetTask(Task _task)
    {
        if (GameManager.IsPlaying)
        {
            UnloadAll(); 
            _task.SimulateTask(this, GameManager.FixedGameTime); 
        }
    }
    public virtual void PlayTask(Task _task)
    {
        if (GameManager.IsPlaying)
        {
            _task.SimulateTask(this, GameManager.FixedGameTime);
        }
    }
    void Awake()
    {
        _history.AddToHead(new BasicAction(ActionType.Null));
        RegisterWibblyWobbly();
        _combat = GetComponent<CombatState>();
        if (_currentTask != null && _currentTask.Default)
        {
            _defaultTask = _currentTask; 
        }
        if(_combat != null)
        {
            _combat.SetCallbacks(new SetActionDelegate(SetAction));
        }
    }
}

public struct InterruptedBranch
{
    HistoryNode _branch;
    public HistoryNode Branch { get { return _branch; } }
    Task _task; 
    public Task Task { get { return _task; } }
    public InterruptedBranch(HistoryNode branch, Task task)
    {
        _task = task;
        _branch = branch; 
    }
}
