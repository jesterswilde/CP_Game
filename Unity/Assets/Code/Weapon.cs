using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
    [SerializeField]
    float _maximmumFireRate;
    [SerializeField]
    float _damage;
    public float Damage { get { return _damage;  } }
    [SerializeField]
    float _ap;
    public float AP { get { return _ap; } }
    [SerializeField]
    float _accuracy;

    float _timeLastFired; 

    public void FiredWeaponAt(float _time)
    {
        _timeLastFired = _time;
    }
    public bool FireWeapon(float _currentTime)
    {
        if(_timeLastFired + _maximmumFireRate < _currentTime)
        {
            return true; 
        }
        return false; 
    }
}
