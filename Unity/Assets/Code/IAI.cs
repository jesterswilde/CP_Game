using UnityEngine;
using System.Collections;

public interface IAI{
    float MoveSpeed { get; }
    float RotationSpeed { get; }
    Transform transform { get; }
    void SetAction(IAction _action);
    void SetAction(IAction _action, bool _detrmineOrigin); 
    CombatState Combat { get; }
    void SetAtomicAction(ActionType _type, IAI _ai, Vector3 _target, float _time);
}
