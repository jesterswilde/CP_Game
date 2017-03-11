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
	protected int _currentTaskIndex; 
    protected Task _interruptedTask;
	protected int _interruptedTaskIndex;
	protected Stack<InterruptedTask> _interuptedTasks = new Stack<InterruptedTask> (); 

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
				_currentAction = new AAMoveForward (this);
				_currentAction.UseAction (_action, _time);
				_currentTaskIndex = _action.IValue;
                break;
            case ActionType.AIRotate:
                _currentAction = new AARotate(this);
                _currentAction.UseAction(_action, _time);
				_currentTaskIndex = _action.IValue;
                break;
			case ActionType.AIMoveTo:
				_currentAction = new AAMoveTo (this);
				_currentAction.UseAction (_action, _time);
				_currentTaskIndex = _action.IValue;
				break;
			case ActionType.AIWait:
				_currentAction = new AAWait ();
				_currentAction.UseAction (_action, _time);
				_currentTaskIndex = _action.IValue;
				break;
            case ActionType.AIRotateDir:
                _currentAction = new AARotateDir(this);
                _currentAction.UseAction(_action, _time);
				_currentTaskIndex = _action.IValue;
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

    protected override void ReverseAction(Action _action, float _time)
    {
        switch (_action.Type)
        {
			case ActionType.AIMoveForwardUnset:
				_currentAction = new AAMoveForward (this);
				_currentAction.ReverseAction (_action, _time); 
				_currentTaskIndex = _action.IValue;
				break;
            case ActionType.AIRotateUnset:
                _currentAction = new AARotate(this);
                _currentAction.ReverseAction(_action, _time);
				_currentTaskIndex = _action.IValue; 
				break; 
            case ActionType.AIMoveToUnset:
                _currentAction = new AAMoveTo(this);
                _currentAction.ReverseAction(_action, _time); 
				_currentTaskIndex = _action.IValue;
				break;
            case ActionType.AIWaitUnset:
                _currentAction = new AAWait();
                _currentAction.ReverseAction(_action, _time); 
				_currentTaskIndex = _action.IValue; 
				break;
            case ActionType.AIRotateDirUnset:
                _currentAction = new AARotateDir(this);
                _currentAction.ReverseAction(_action, _time);
				_currentTaskIndex = _action.IValue;
				break; 
            case ActionType.UnsetTask:
                _currentTask = _action.Task;
                break;
			case ActionType.UnsetInterruption:
				PopInterruption ();
				break;
			case ActionType.SetInterruption:
				PushInterruption ();
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
    protected virtual void Interrupt()
    { 
		PushInterruption ();
		UnloadAll (); 
		SetExternalAction(new BasicAction(ActionType.UnsetInterruption, true));
		SetAction (new BasicAction (ActionType.Null, Mathf.Infinity, true));
    }
	protected virtual void PushInterruption(){
		_interuptedTasks.Push (new InterruptedTask (_currentTask, _currentTaskIndex)); 
		_interruptedTask = _currentTask; 
		_interruptedTaskIndex = _currentTaskIndex;
	}
    protected virtual void Resume()
    {
		PopInterruption (); 
		SetExternalAction (new BasicAction (ActionType.SetInterruption, true));
    }
	protected virtual void PopInterruption(){
		InterruptedTask _task = _interuptedTasks.Pop ();
		if (_task.Task != null) {
			SetTask (_task.Task, _task.Index); 
			_currentTask = _task.Task;
			_currentTaskIndex = _task.Index; 
		}
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
						Interrupt ();
                        break;
					case TriggerType.Resume:
						Resume ();
                        break;
                }
            }
        }
    }
    public override void SetExternalAction(Action _action)
    {
        base.SetExternalAction(_action);
    }
	public virtual void SetTask(Task _task, int index)
    {
        if (GameManager.IsPlaying)
        {
            UnloadAll(); 
            _task.SimulateTask(this, GameManager.FixedGameTime, index); 
        }
    }
	public virtual void SetTask(Task _task){
		SetTask (_task, 0); 
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
	void Start(){
		_history.AddToHead(new BasicAction(ActionType.Null));
		RegisterWibblyWobbly();
	}
}

public class InterruptedTask
{
	Task _task; 
	int _index; 
	public Task Task{ get { return _task; } }
	public int Index { get { return _index; } }

	public InterruptedTask(Task task, int index){
		_task = task; 
		_index = index; 
	}
}
