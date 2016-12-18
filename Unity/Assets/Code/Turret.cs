using UnityEngine;
using System.Collections;
using System;

public class Turret : MonoBehaviour, IAttackable {

    [SerializeField]
    float _health;
    [SerializeField]
    float _accuracy;
    [SerializeField]
    float _damageReduction;
    [SerializeField]
    float _dodge;
    [SerializeField]
    float _damage;
    [SerializeField]
    Transform[] _vitalPoints; 
    Vitals _vitals = new Vitals(); 

    Renderer _renderer;
    Color _baseColor;
    Character _target; 
    public bool IsVisible { get { return _renderer.isVisible; } }
    public Vector3 Position { get { return transform.position;  } }


    public float Dodge { get { return _dodge; } }

    public void Targeted()
    {
        _renderer.material.color = Color.red; 
    }

    public void UnTargeted()
    {
        _renderer.material.color = _baseColor; 
    }
    public float AmountOfBodyExposed(Vector3 _gunPoint)
    {
        return _vitals.AmountOfBodyExposed(_gunPoint); 
    }
    public void ShotBy(Weapon _bullet)
    {
        _health -= _bullet.Damage - Math.Max(0, _damageReduction - _bullet.AP); 
        if(_health <= 0)
        {
            Death(); 
        }
    }
    public void UnShitBy(Weapon _bullet)
    {
        _health += _bullet.Damage - Math.Max(0, _damageReduction - _bullet.AP); 
    }
    public void Death()
    {
        _renderer.enabled = false; 
    }
    public void Undeath()
    {
        _renderer.enabled = true; 
    }
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _baseColor =  _renderer.material.color;
        if(_vitalPoints.Length > 0)
        {
            _vitals.SetVitals(_vitalPoints);
        }else
        {
            _vitals.SetVitals(transform); 
        }
    }


}
