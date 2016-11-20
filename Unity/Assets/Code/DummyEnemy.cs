﻿using UnityEngine;
using System.Collections;

public class DummyEnemy : MonoBehaviour {

    Enemy _enemy;
    float _time;  
    public float Time { get { return _time; } }
    public float MoveSpeed { get { return _enemy.MoveSpeed; } }
    public float RotationSpeed { get { return _enemy.RotationSpeed; } }
    public void Startup(Enemy enemy, float time)
    {
        _enemy = enemy;
        _time = time;
        transform.position = _enemy.transform.position;
        transform.rotation = _enemy.transform.rotation;
    }
    public void AddToTIme(float _addedTime)
    {
        _time += _addedTime; 
    }
    public void SetAction(IAction _action)
    {
        _enemy.SetAction(_action); 
    }
}
