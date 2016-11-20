using UnityEngine;
using System.Collections;
using System;

public class AAMoveForward : IAtomicAction
{
    Enemy _enemy;
    float _distance;
    float _maxDistance;
    float _finishesAt;
    public float FinishesAt { get { return _finishesAt; } }
    public AAMoveForward(Enemy enemy)
    {
        _enemy = enemy; 
    }
    public void Act(float _deltaTime)
    {
        _enemy.transform.position += _enemy.transform.forward * _enemy.MoveSpeed * _deltaTime;
        _distance -= _enemy.MoveSpeed * _deltaTime; 
    }

    public void ActReverse(float _deltaTime)
    {
        _distance += _enemy.MoveSpeed * _deltaTime;
        _enemy.transform.position -= _enemy.transform.forward * _enemy.MoveSpeed * _deltaTime; 
    }

    void CalculateFinishTime(IAction _action)
    {
        _finishesAt = _action.Time + _action.Value / _enemy.MoveSpeed; 
    }

    public void ReverseAction(IAction _action, float _time)
    {
        _distance = _action.Value - (_action.Time - _time) * _enemy.MoveSpeed;
        _maxDistance = _action.Value;
        CalculateFinishTime(_action); 
    }

    public void UseAction(IAction _action, float _time)
    {
        _distance = _action.Value;
        _maxDistance = _action.Value;
        CalculateFinishTime(_action);
    }

    public static IAction CreateAction(Transform _enemy, Vector3 _target, float _time)
    {
        return new ValueAction(ActionType.AIMoveForward, Vector3.Distance(_enemy.transform.position, _target), _time); 
    }
    public static void SimulateAction(DummyEnemy _enemy, Vector3 _target)
    {
        float _distance = Vector3.Distance(_enemy.transform.position, _target);
        _enemy.transform.position += _enemy.transform.forward * _distance; 
        _enemy.SetAction(new ValueAction(ActionType.AIMoveForward, _distance, _enemy.Time));
        _enemy.AddToTIme(_distance / _enemy.MoveSpeed);
    }
}
