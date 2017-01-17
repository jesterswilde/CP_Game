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
    [SerializeField]
    string _muzzleFlash;
    public string MuzzleFlash { get { return _muzzleFlash; } } 
    Stack<float> _firedAt = new Stack<float>(); 

    public void ReverseFireWeapon()
    {
        if(_firedAt.Count > 0)
        {
            _firedAt.Pop(); 
        }
    }
    public bool PullTrigger(float _currentTime)
    {
        if(_firedAt.Count == 0)
        {
            _firedAt.Push(_currentTime); 
            return true; 
        }
        if (_firedAt.Peek() + _maximmumFireRate < _currentTime)
        {
            _firedAt.Push(_currentTime);
            return true;
        }
        return false; 
    }
    public float AccuracyLoss(float distance)
    {
        return distance * _distanceFalloff; 
    }
}
