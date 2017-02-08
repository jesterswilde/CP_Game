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
    List<Action> Activate(Character _character);
    void RewindActivation(Action _action);
    GameObject Go { get; }
    float MinDistanceToActivate { get; }
    void SetAction(Action _action);
    void SetAction(Action _action, bool _evaluatSource); 
}
