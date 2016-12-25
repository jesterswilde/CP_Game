using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

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
    public float Accuracy { get { return _accuracy; } }
    [SerializeField]
    float _distanceFalloff = 5; 
    List<float> _firedAt = new List<float>(); 

    public void ReverseFireWeapon()
    {
        _firedAt.RemoveAt(_firedAt.Count - 1); 
    }
    public bool PullTrigger(float _currentTime)
    {
        if(_firedAt.Count == 0)
        {
            _firedAt.Add(_currentTime); 
            return true; 
        }
        if (_firedAt.Last() + _maximmumFireRate < _currentTime)
        {
            _firedAt.Add(_currentTime);
            return true;
        }
        return false; 
    }
    public float AccuracyLoss(float distance)
    {
        return distance * _distanceFalloff; 
    }
}
