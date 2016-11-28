using UnityEngine;
using System.Collections;

public interface IAction {
    float Time { get; }
    ActionType Type { get; }
    float Value { get; }
    Vector3 Vector { get; }
    HistoryNode PossibleFuture { get; }
    bool IsExternal { get; }
}
