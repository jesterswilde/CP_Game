using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ITargetable {
    void Targeted();
    void UnTargeted();
    bool IsVisible { get; }
    Vector3 Position { get; } 
    bool isActivatable { get; }
    bool isAttackable { get; }
    CombatState Combat { get; }
    List<IAction> Activate(Character _character);
    void RewindActivation(IAction _action);
    GameObject Go { get; }
    float MinDistanceToActivate { get; }
    void SetAction(IAction _action);
    void SetAction(IAction _action, bool _evaluatSource); 
}
