using UnityEngine;
using System.Collections;

public struct TargetableDist
{
    InteractableTrigger _target;
    float _dist; 
    public InteractableTrigger Target { get { return _target; } }
    public float Distance { get { return _dist; } }
    public TargetableDist(InteractableTrigger target, float dist)
    {
        _target = target;
        _dist = dist; 
    }
}

public struct AttackableDist
{
    IAttackable _target;
    float _dist;
    public IAttackable Target { get { return _target; } }
    public float Distance { get { return _dist; } }
    public AttackableDist(IAttackable target, float dist)
    {
        _target = target;
        _dist = dist; 
    }
}