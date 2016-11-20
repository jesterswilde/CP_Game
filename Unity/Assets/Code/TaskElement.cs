using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TaskElement {

    [SerializeField]
    AIActions _action;
    public AIActions Action { get { return _action; } }
    [SerializeField]
    float _value;
    public float Value { get { return _value; } }
    [SerializeField]
    Transform _trans;
    Vector3 _target; 


    public Vector3 GetTarget()
    {
        if(_trans == null)
        {
            return _target; 
        }
        return _trans.position; 
    }
    public TaskElement(AIActions action)
    {
        _action = action; 
    }
    public TaskElement(AIActions action, float value)
    {
        _action = action;
        _value = value; 
    }
    public TaskElement(AIActions action, Vector3 target)
    {
        _target = target; 
    }
    public TaskElement(AIActions action, Transform trans)
    {
        _action = action;
        _trans = trans;
    }
}
