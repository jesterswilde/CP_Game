﻿using UnityEngine;
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
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }

    public float Value { get { return float.NaN; } }
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
    public ActionType Type { get { return _action; } }
    public float Time { get { return _time; } }
    public float Value { get { return _value; } }
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
    Clear
}

