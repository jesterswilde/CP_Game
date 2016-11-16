using UnityEngine;
using System.Collections;
using System;

public class Action : IAction{
    ActionType _action;
    float _time;

    public Action(ActionType _actionType)
    {
        _action = _actionType;
        _time = GameManager.GameTime;
    }
    public Action(ActionType _actionType, float time)
    {
        _action = _actionType;
        _time = time;
    }
    public Action (ActionType _actionType, bool _isFixed)
    {
        _action = _actionType;
        if (_isFixed)
        {
            _time = GameManager.GetGameTimeFixed();
        }else
        {
            _time = GameManager.GameTime; 
        }
    }
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return float.NaN; } }
    public Vector3 Vector { get { return Vector3.zero; } }
}

public class ValueAction : IAction
{
    ActionType _action;
    float _time;
    float _value; 

    public ValueAction(ActionType _actionType, float value)
    {
        _action = _actionType;
        _time = GameManager.GameTime;
        _value = value; 
    }
    public ValueAction(ActionType _actionType, float value, float time)
    {
        _action = _actionType;
        _time = time;
        _value = value;
    }
    public ValueAction(ActionType _actionType, float value, bool _isFixed)
    {
        _action = _actionType;
        if (_isFixed)
        {
            _time = GameManager.GetGameTimeFixed();
        }else
        {
            _time = GameManager.GameTime;

        }
    }
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return _value; } }
    public Vector3 Vector { get { return Vector3.zero; } }
}
public class VectorAction : IAction
{
    ActionType _action;
    float _time;
    Vector3 _vector;

    public VectorAction(ActionType _actionType, Vector3 vector)
    {
        _action = _actionType;
        _time = GameManager.GameTime;
        _vector = vector;
    }
    public VectorAction(ActionType _actionType, Vector3 vector, float time)
    {
        _action = _actionType;
        _time = time;
        _vector = vector;
    }
    public VectorAction(ActionType _actionType, Vector3 vector, bool _isFixed)
    {
        _action = _actionType;
        _vector = vector; 
        if (_isFixed)
        {
            _time = GameManager.GetGameTimeFixed();
        }
        else
        {
            _time = GameManager.GameTime;

        }
    }
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return float.NaN; } }
    public Vector3 Vector { get { return _vector; } }
}

public enum ActionType
{
    PressForward,
    ReleaseForward,
    PressRight,
    ReleaseRight,
    PressLeft,
    ReleaseLeft,
    PressBack,
    ReleaseBack,
    Rotation,
    ForwardBlocked,
    ForwardUnblocked,
    BackwardBlocked,
    BackwardUnblocked,
    LeftBlocked,
    LeftUnblocked,
    RightBlocked,
    RightUnblocked,
    Clear,
    AIWait,
    AIMove,
    AIRotate
}

