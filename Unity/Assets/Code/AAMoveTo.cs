﻿using UnityEngine;
using System.Collections;

public class AAMoveTo : IAtomicAction {

    IAI _enemy;
    Vector3 _maxVector;
    Vector3 _dir; 
    Vector3 _remaining;
    Vector3 _target; 
    float _finishesAt;
    public float FinishesAt { get { return _finishesAt; } }
    public AAMoveTo(IAI enemy)
    {
        _enemy = enemy;
    }
    public void Act(float _deltaTime)
    {
        _enemy.transform.position += _dir * _enemy.MoveSpeed * _deltaTime;
        _remaining -= _enemy.MoveSpeed * _deltaTime * _dir;
    }

    public void ActReverse(float _deltaTime)
    {
        _remaining += _enemy.MoveSpeed * _deltaTime * _dir;
        _enemy.transform.position -= _dir * _enemy.MoveSpeed * _deltaTime;
    }

    void CalculateFinishTime(Action _action)
    {
        _finishesAt = _action.Time + _action.Vector.magnitude / _enemy.MoveSpeed;
    }

    public void ReverseAction(Action _action, float _time)
    {
        _dir = _action.Vector.normalized; 
        _maxVector = _action.Vector;
        _remaining = _action.Vector - (_action.Time - _time) * _enemy.MoveSpeed * _dir;
        CalculateFinishTime(_action);
    }

    public void UseAction(Action _action, float _time)
    {
        _remaining = _action.Vector;
        _maxVector = _action.Vector;
        _dir = _action.Vector.normalized;
        _target = _action.OriginalVec;  
        CalculateFinishTime(_action);
    }
    public Action Unset()
    {
        return new VectorAction(ActionType.AIMoveToUnset, _maxVector); 
    }
    public static Action CreateAction(Transform _enemy, Vector3 _target, float _time)
    {
        return new ValueAction(ActionType.AIMoveTo, Vector3.Distance(_enemy.transform.position, _target), _time);
    }
    public Action CreateAction()
    {
        return new DirTargetAction(ActionType.AIMoveTo, _remaining, _target);
    }
    public static void SimulateAction(DummyEnemy _enemy, Vector3 _target)
    {
        Vector3 _toVector  = _target - _enemy.transform.position;
        _enemy.transform.position = _target;
        _enemy.SetAction(new DirTargetAction(ActionType.AIMoveTo, _toVector, _target, _enemy.Time));
        _enemy.AddToTIme(_toVector.magnitude / _enemy.MoveSpeed);
        _enemy.SetAction(new DirTargetAction(ActionType.AIMoveToUnset, _toVector, _target, _enemy.Time));
    }
}
