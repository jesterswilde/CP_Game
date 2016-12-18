using UnityEngine;
using System.Collections;

public interface IAttackable {
    void Targeted();
    void UnTargeted();
    float Dodge { get; }
    void ShotBy(Weapon _bullet); 
    bool IsVisible { get; }
    Vector3 Position { get; } 
}
