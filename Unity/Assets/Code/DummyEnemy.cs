using UnityEngine;
using System.Collections;

public class DummyEnemy : MonoBehaviour {

    IAI _enemy;
    float _time;  
    public float Time { get { return _time; } }
    public float MoveSpeed { get { return _enemy.MoveSpeed; } }
    public float RotationSpeed { get { return _enemy.RotationSpeed; } }
    public void Startup(IAI enemy, float time)
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
    public void SetAction(Action _action)
    {
        _enemy.SetAction(_action); 
    }
}
