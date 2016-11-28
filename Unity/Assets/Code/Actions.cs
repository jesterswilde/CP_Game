using UnityEngine;
using System.Collections;
using System;

public class Action : IAction{
    ActionType _action;
    float _time;
    bool _isExternal; 

    public Action(ActionType _actionType)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
    }
    public Action(ActionType _actionType, bool isExternal)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
        _isExternal = isExternal; 
    }
    public Action(ActionType _actionType, float time, bool isExternal)
    {
        _action = _actionType;
        _time = time;
        _isExternal = isExternal;
    }
    public Action(ActionType _actionType, float time)
    {
        _action = _actionType;
        _time = time;
    }
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return float.NaN; } }
    public Vector3 Vector { get { return Vector3.zero; } }
    public bool IsExternal { get { return _isExternal; } }
    public HistoryNode PossibleFuture { get { return null; } }
}

public class ValueAction : IAction
{
    ActionType _action;
    float _time;
    float _value;
    bool _isExternal; 

    public ValueAction(ActionType _actionType, float value)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
        _value = value; 
    }
    public ValueAction(ActionType _actionType, float value, float time)
    {
        _action = _actionType;
        _time = time;
        _value = value;
    }
    public ValueAction(ActionType _actionType, float value, bool isExternal)
    {
        _action = _actionType;
        _value = value;
        _isExternal = isExternal; 
    }
    public ValueAction(ActionType _actionType, float value, float time, bool isExternal)
    {
        _action = _actionType;
        _value = value;
        _time = time;
        _isExternal = isExternal; 
    }
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return _value; } }
    public Vector3 Vector { get { return Vector3.zero; } }
    public bool IsExternal { get { return _isExternal; } }
    public HistoryNode PossibleFuture { get { return null; } }
}
public class VectorAction : IAction
{
    ActionType _action;
    float _time;
    Vector3 _vector;
    bool _isExternal; 

    public VectorAction(ActionType _actionType, Vector3 vector)
    {
        _action = _actionType;
        _time = GameManager.FixedGameTime;
        _vector = vector;
    }
    public VectorAction(ActionType _actionType, Vector3 vector, float time)
    {
        _action = _actionType;
        _time = time;
        _vector = vector;
    }
    public VectorAction(ActionType _actionType, Vector3 vector, bool isExternal)
    {
        _action = _actionType;
        _vector = vector;
        _isExternal = isExternal; 
    }
    public VectorAction(ActionType _actionType, Vector3 vector, float time, bool isExternal)
    {
        _action = _actionType;
        _vector = vector;
        _isExternal = isExternal;
        time = _time; 
    }
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return float.NaN; } }
    public Vector3 Vector { get { return _vector; } }
    public bool IsExternal { get { return _isExternal; } }
    public HistoryNode PossibleFuture { get { return null; } }
}
public class FutureActions : IAction
{
    bool _isExternal = true;
    HistoryNode _future;
    float _time;
    ActionType _type; 

    public FutureActions(ActionType type, HistoryNode future)
    {
        _type = type;
        _future = future; 
    }
    public FutureActions(ActionType type, HistoryNode future, float time)
    {
        _type = type; 
        _future = future;
        _time = time; 
    }
    public bool IsExternal {get{ return _isExternal; }}
    public float Time { get { return _time; } }
    public ActionType Type{get{return _type;}}
    public HistoryNode PossibleFuture { get { return _future; } }
    public float Value {get{return float.NaN;}}
    public Vector3 Vector { get { return Vector3.zero; } }
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
    AIWaitUnset,
    AIMoveForward,
    AIMoveForwardUnset,
    AIRotate,
    AIRotateUnset,
    AIMoveTo, 
    AIMoveToUnset,
    SpliceFuture,
    ClearFuture, 
    Null, 
    AIAlert
}

