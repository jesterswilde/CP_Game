﻿using UnityEngine;
using System.Collections;

public interface ITargetable {
    void Targeted();
    void UnTargeted();
    bool IsVisible { get; }
    Vector3 Position { get; } 
    bool isActivatable { get; }
    bool isAttackable { get; }
    CombatState Combat { get; }
    IAction Activate(Character _character);
    void RewindActivation(IAction _action);
    GameObject Go { get; }
    float MinDistanceToActivate { get; }
    void SetAction(IAction _action);
    void SetAction(IAction _action, bool _evaluatSource); 
}
