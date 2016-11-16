using UnityEngine;
using System.Collections;

[System.Serializable]
public class AITask  {

    [SerializeField]
    AIActions _action;
    [SerializeField]
    float _value;
    [SerializeField]
    Transform _target; 

    public AIActions Action { get { return _action; } }
    public float Value { get { return _value; } }
    public Transform Target { get { return _target; } }
}
