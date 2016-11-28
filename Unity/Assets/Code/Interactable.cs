using UnityEngine;
using System.Collections;
using System;

public class Interactable : WibblyWobbly, IAI {

    public void PrintHistory()
    {
        string _log = "";
        HistoryNode _node = _history.Tail;
        while (true)
        {
            if (_node == null) {
                break;
            }
            _log += _node.Action.Type + ", " + _node.Action.Time +" | ";
            _node = _node.Next; 
        }
        Debug.Log(_log); 
    }
    IAtomicAction _currentAction;
    [SerializeField]
    float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField]
    float _rotationSpeed;
    public float RotationSpeed { get { return _rotationSpeed; } }

    protected override void Act(float _deltaTime)
    {
        if(_currentAction != null)
        {
            _currentAction.Act(_deltaTime); 
        }
    }

    protected override void ActReverse(float _deltaTIme)
    {
        if(_currentAction != null)
        {
            _currentAction.ActReverse(_deltaTIme); 
        }
    }


    protected override void ReverseAction(IAction _action, float _time)
    {
        Debug.Log("Reversing " + _action.Type); 
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
            case ActionType.AIMoveForward:
            case ActionType.AIMoveTo:
            case ActionType.AIRotate:
            case ActionType.AIWait:
                _currentAction = null;
                break; 
            case ActionType.SpliceFuture:
                _history.ReloadPossibleFuture(_action.PossibleFuture);
                if(_history.Pointer.Next != null)
                {
                    ReverseAction(_history.Pointer.Next.Action, GameManager.FixedGameTime); 
                }
                break;
        }
    }

    protected override void UseAction(IAction _action, float _time)
    {
        Debug.Log("Using " + _action.Type); 
        switch(_action.Type)
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
            case ActionType.AIMoveForwardUnset:
            case ActionType.AIMoveToUnset:
            case ActionType.AIRotateUnset:
            case ActionType.AIWaitUnset:
                _currentAction = null;
                break; 
        }
    }
    void ProcessTask()
    {
        _history.AddToHead(new Action(ActionType.ClearFuture, true));
    }
    public override void SetExternalAction(IAction _action)
    {
        _history.SpliceOffPossibleFuture(); 
        base.SetExternalAction(_action);
    }
    public void SetTask(Task _task)
    {
        if (GameManager.IsPlaying)
        {
            _history.SpliceOffPossibleFuture();
            _task.SimulateTask(this, GameManager.FixedGameTime); 
        }
    }
    void Awake()
    {
        _history.AddToHead(new Action(ActionType.Null));
        RegisterWibblyWobbly(); 
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintHistory();
        }
    }
}
