using UnityEngine;
using System.Collections;

public interface IAI{
    float MoveSpeed { get; }
    float RotationSpeed { get; }
    Transform transform { get; }
    void SetAction(Action _action);
    void SetAction(Action _action, bool _detrmineOrigin); 
    CombatState Combat { get; }
    void SetAtomicAction(ActionType _type, IAI _ai, Vector3 _target, float _time);
}
