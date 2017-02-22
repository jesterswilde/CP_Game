using UnityEngine;
using System.Collections;
using System;

public class AAMoveForward : IAtomicAction
{
    IAI _enemy;
    float _distance;
    float _maxDistance;
    float _finishesAt;
    public float FinishesAt { get { return _finishesAt; } }
    public AAMoveForward(IAI enemy)
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

    void CalculateFinishTime(Action _action)
    {
        _finishesAt = _action.Time + _action.Value / _enemy.MoveSpeed; 
    }

    public void ReverseAction(Action _action, float _time)
    {
        _distance = _action.Value - (_action.Time - _time) * _enemy.MoveSpeed;
        _maxDistance = _action.Value;
        CalculateFinishTime(_action); 
    }

    public void UseAction(Action _action, float _time)
    {
        _distance = _action.Value;
        _maxDistance = _action.Value;
        CalculateFinishTime(_action);
    }
    public Action Unset()
    {
        return new ValueAction(ActionType.AIMoveForwardUnset, _maxDistance); 
    }

    public static Action CreateAction(Transform _enemy, Vector3 _target, float _time)
    {
        return new ValueAction(ActionType.AIMoveForward, Vector3.Distance(_enemy.transform.position, _target), _time); 
    }
    public Action CreateAction()
    {
        return new ValueAction(ActionType.AIMoveForward, _distance); 
    }
    public static void SimulateAction(DummyEnemy _enemy, Vector3 _target)
    {
        float _distance = Vector3.Distance(_enemy.transform.position, _target);
        _enemy.transform.position += _enemy.transform.forward * _distance; 
        _enemy.SetAction(new ValueAction(ActionType.AIMoveForward, _distance, _enemy.Time));
        _enemy.AddToTIme(_distance / _enemy.MoveSpeed);
        _enemy.SetAction(new ValueAction(ActionType.AIMoveForwardUnset, _distance, _enemy.Time));
    }
}
