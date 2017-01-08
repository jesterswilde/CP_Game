using UnityEngine;
using System.Collections;
using System;

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
            if(_currentTask != null && _history.IsPointerAtHead())
            {
                SetTask(_currentTask);
            }else
            {
                if(_defaultTask != null && _history.IsPointerAtHead())
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

    protected override void UseAction(IAction _action, float _time)
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
        }
        if(_combat != null && _action != null)
        {
            SetAction(_combat.UseAction(_action), true); 
        }
    }

    protected override void ReverseAction(IAction _action, float _time)
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
            case ActionType.SetTask:
                _currentTask = null;
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
    protected virtual void UnloadAll()
    {
        _history.SpliceOffPossibleFuture();
        if (_currentAction != null)
        {
            SetAction(_currentAction.Unset());
        }
        if (_currentTask != null)
        {
            SetAction(new TaskAction(ActionType.UnsetTask, _currentTask, true));
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
        if(_task != null)
        {
            SetTask(_task);
        }
    }
    public override void SetExternalAction(IAction _action)
    {
        base.SetExternalAction(_action);
    }
    public virtual void SetTask(Task _task)
    {
        if (GameManager.IsPlaying)
        {
            UnloadAll(); 
            //SetAction(new TaskAction(ActionType.SetTask, _task)); 
            _task.SimulateTask(this, GameManager.FixedGameTime); 
        }
    }
    void Awake()
    {
        _history.AddToHead(new Action(ActionType.Null));
        RegisterWibblyWobbly();
        _combat = GetComponent<CombatState>();
        _defaultTask = _currentTask; 
        if(_combat != null)
        {
            _combat.SetCallbacks(new SetActionDelegate(SetAction));
        }
    }
}
