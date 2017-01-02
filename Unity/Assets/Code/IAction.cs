using UnityEngine;
using System.Collections;

public interface IAction {
    float Time { get; }
    ActionType Type { get; }
    float Value { get; }
    Vector3 Vector { get; }
    HistoryNode PossibleFuture { get; }
    bool IsExternal { get; }
    ITargetable Target { get; }
    Task Task { get; }
    Weapon Weapon { get; }
    CombatState Combat { get; }
    IBehavior Behavior { get; }
}
