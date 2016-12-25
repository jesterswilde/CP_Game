using UnityEngine;
using System.Collections;

public struct TargetableDist
{
    TargetableTrigger _target;
    float _dist; 
    public TargetableTrigger Target { get { return _target; } }
    public float Distance { get { return _dist; } }
    public TargetableDist(TargetableTrigger target, float dist)
    {
        _target = target;
        _dist = dist; 
    }
}

public struct AttackableDist
{
    ITargetable _target;
    float _dist;
    public ITargetable Target { get { return _target; } }
    public float Distance { get { return _dist; } }
    public AttackableDist(ITargetable target, float dist)
    {
        _target = target;
        _dist = dist; 
    }
}