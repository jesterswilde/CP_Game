using UnityEngine;
using System.Collections;

public struct Action{
    ActionType _action;
    float _time;
    Vector3 _pos; 

    public Action(ActionType _actionType, Character _character)
    {
        _action = _actionType;
        _pos = _character.transform.position; 
        _time = GameManager.GameTime; 
    }
    public ActionType Type { get { return _action; } }
    public Vector3 Position { get { return _pos; } }
    public float Time { get { return _time; } }
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
    Clear
}

