using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;
using System.Linq; 

public class Enemy : WibblyWobbly, IAI {
    [SerializeField]
    float _moveSpeed;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField]
    float _rotationSpeed;
    public float RotationSpeed { get { return _rotationSpeed; } }
    float _lastChecked;

    [SerializeField]
    Task _task;
    AAMoveForward _aaMoveForward;
    AARotate _aaRotate;
    AAWait _aaWait;
    AAMoveTo _aaMoveTo; 
    IAtomicAction _currentAA;
    ActionType[] _validControllers = new ActionType[] { ActionType.AIMoveForward, ActionType.AIMoveTo, ActionType.Rotation, ActionType.AIWait }; 

    public override void Play(float _time)
    {
        if (_history.IsPointerAtHead() && _currentAA == null)
        {
             SetAction(new ValueAction(ActionType.AIWait, _time - _currentAA.FinishesAt, _currentAA.FinishesAt));
            _task.SimulateTask(this, _currentAA.FinishesAt); 
        }
        base.Play(_time); 
    }
    IAtomicAction GetAAFromAction(Action _action)
    {
        switch (_action.Type)
        {
            case ActionType.AIMoveForward:
                return _aaMoveForward;
            case ActionType.AIRotate:
                return _aaRotate;
            case ActionType.AIWait:
                return _aaWait;
            case ActionType.AIMoveTo:
                return _aaMoveTo;
            case ActionType.AIMoveForwardUnset:
            case ActionType.AIRotateUnset:
            case ActionType.AIMoveToUnset:
            case ActionType.AIWaitUnset:
                _currentAA = null;
                break; 
            case ActionType.Alert:
                Debug.Log(gameObject.name + " saw da cybapunk at approximately " + GameManager.GameTime);
                break; 
        }
        return null;
    }
    IAtomicAction RewindAAFromAction(Action _action)
    {
        switch (_action.Type)
        {
            case ActionType.AIMoveForwardUnset:
                return _aaMoveForward;
            case ActionType.AIRotateUnset:
                return _aaRotate;
            case ActionType.AIWaitUnset:
                return _aaWait;
            case ActionType.AIMoveToUnset:
                return _aaMoveTo;
            case ActionType.AIMoveForward:
            case ActionType.AIRotate:
            case ActionType.AIMoveTo:
            case ActionType.AIWait:
                _currentAA = null;
                break; 
        }
        return null;
    }
    protected override void UseAction(Action _action, float _time)
    {
        IAtomicAction _aa = GetAAFromAction(_action);
        _currentAA = _aa;
        if(_currentAA != null)
        {
            _currentAA.UseAction(_action, _time); 
        }
    }

    protected override void ReverseAction(Action _action, float _time)
    {
        IAtomicAction _aa = RewindAAFromAction(_history.PointerAction);
        _currentAA = _aa; 
        if(_currentAA != null)
        {
            _currentAA.ReverseAction(_history.PointerAction, _time); 
        }
    }

    protected override void Act(float _deltaTime)
    {
        if(_currentAA != null)
        {
            _currentAA.Act(_deltaTime); 
        }
    }

    protected override void ActReverse(float _deltaTIme)
    {
        if(_currentAA != null)
        {
            _currentAA.ActReverse(_deltaTIme); 
        }
    }
    public override void SetAction(Action _action)
    {
        if(GetAAFromAction(_action) != null && _currentAA != null)
        {
            base.SetAction(_currentAA.Unset()); 
        } 
        base.SetAction(_action);
    }
    public override void SetExternalAction(Action _action)
    {
        if (_validControllers.Contains(_action.Type)){
            base.SetExternalAction(_action);
        }else
        {
            if(_action.Type == ActionType.Alert)
            {
                Debug.Log("Cybapunk spotted at approximately " + _action.Time); 
            }
        }
    }
    void Awake()
    {
        _aaMoveForward = new AAMoveForward(this);
        _aaRotate = new AARotate(this);
        _aaWait = new AAWait();
        _aaMoveTo = new AAMoveTo(this); 
        _combat = GetComponent<CombatState>();
    }

    void Start()
    {
		UseAction(new ValueAction(ActionType.AIWait, 0.5f), 0); 
		RegisterWibblyWobbly();
    }

    public void SetAtomicAction(ActionType _type, IAI _ai, Vector3 _target, float _time)
    {
        throw new NotImplementedException();
    }
}

public enum AIActions
{
    LookAt,
    MoveFoward,
    Wait,
    MoveTo,
    Alert
}
