using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;
using System.Linq; 

public class Enemy : WibblyWobbly {
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
    AAMoveTo __aaMoveTo; 
    IAtomicAction _currentAA;
    ActionType[] _validControllers = new ActionType[] { ActionType.AIMoveForward, ActionType.AIMoveTo, ActionType.Rotation, ActionType.AIWait }; 

    public override void Play()
    {
        if (_history.IsPointerAtHead())
        {
            if(GameManager.GameTime >= _currentAA.FinishesAt)
            {
                SetAction(new ValueAction(ActionType.AIWait, GameManager.GameTime - _currentAA.FinishesAt, _currentAA.FinishesAt));
                _task.SetRoutine(this, _currentAA.FinishesAt); 
            }
        }
        base.Play(); 
    }

    void SetCurrentAA(IAction _action)
    {
        switch (_action.Type)
        {
            case ActionType.AIMoveForward:
                _currentAA = _aaMoveForward;
                break;
            case ActionType.AIRotate:
                _currentAA = _aaRotate;
                break;
            case ActionType.AIWait:
                _currentAA = _aaWait;
                break;
            case ActionType.AIMoveTo:
                _currentAA = __aaMoveTo;
                break;
            case ActionType.AIAlert:
                Debug.Log(gameObject.name + " saw da cybapunk at approximately " + GameManager.GameTime);
                break; 
        }
    }
    protected override void UseAction(IAction _action, float _time)
    {
        SetCurrentAA(_action);
        _currentAA.UseAction(_action, _time); 
    }

    protected override void ReverseAction(IAction _action, float _time)
    {
        SetCurrentAA(_history.PointerAction);
        _currentAA.ReverseAction(_history.PointerAction, _time); 
    }

    protected override void Act(float _deltaTime)
    {
        _currentAA.Act(_deltaTime); 
    }

    protected override void ActReverse(float _deltaTIme)
    {
        _currentAA.ActReverse(_deltaTIme); 
    }
    public override void SetExternalAction(IAction _action)
    {
        if (_validControllers.Contains(_action.Type)){
            base.SetExternalAction(_action);
        }else
        {
            if(_action.Type == ActionType.AIAlert)
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
        __aaMoveTo = new AAMoveTo(this); 
        UseAction(new ValueAction(ActionType.AIWait, 0.5f), 0); 
        RegisterWibblyWobbly(); 
    }
    void Start()
    {
    }
}

public enum AIActions
{
    LookAt,
    MoveFoward,
    Wait,
    MoveTo
}
