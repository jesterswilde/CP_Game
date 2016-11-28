using UnityEngine;
using System.Collections;

public interface IAI{
    float MoveSpeed { get; }
    float RotationSpeed { get; }
    Transform transform { get; }
    void SetAction(IAction _action); 
}
