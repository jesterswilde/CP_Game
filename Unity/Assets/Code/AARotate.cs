﻿using UnityEngine;
using System.Collections;
using System;

public class AARotate : IAtomicAction
{
    float _maxRotation;
    float _remainingRotation;
    int _dir;
    float _finishesAt;
    Vector3 _target; 
    public float FinishesAt { get { return _finishesAt; } }
    IAI _enemy;
	int _index; 

    public AARotate(IAI enemy)
    {
        _enemy = enemy; 
    }

    public void Act(float _deltaTime)
    {
        float _rotAmount = _deltaTime * _enemy.RotationSpeed * _dir;
        _remainingRotation -= _rotAmount;
        _enemy.transform.Rotate(new Vector3(0, _rotAmount, 0));
    }

    public void ActReverse(float _deltaTime)
    {
        float _rotAmount = _deltaTime * _enemy.RotationSpeed * _dir * -1;
        _enemy.transform.Rotate(new Vector3(0, _rotAmount, 0));
        _remainingRotation -= _rotAmount; 
    }

    public void ReverseAction(Action _action, float _time)
    {
		_index = _action.IValue; 
        _remainingRotation = _action.Value - (_time - _action.Time) * _enemy.RotationSpeed;
        _maxRotation = _action.Value;
        _dir = (_action.Value > 0) ? 1 : -1;
        CalculateFinishTime(_action); 
    }

    public void UseAction(Action _action, float _time)
    {
		_index = _action.IValue; 
        _remainingRotation = _action.Value;
        _maxRotation = _action.Value;
        _dir = (_action.Value > 0) ? 1 : -1;
        _target = _action.SecondVec; 
        CalculateFinishTime(_action);
    }
    void CalculateFinishTime(Action _action)
    {
        _finishesAt = _action.Time + Math.Abs(_action.Value) / _enemy.RotationSpeed;
    }

    public Action Unset()
    {
		return new ValueIntAction(ActionType.AIRotateUnset, _maxRotation, _index, true); 
    }

    public static Action CreateAction(Transform _enemy, Vector3 _target, float _time)
    {
        Vector3 _currentForward = _enemy.forward;
        Vector3 _tempTarget = new Vector3(_target.x, 0, _target.z); 
        _enemy.transform.LookAt(_tempTarget);
        Vector3 _targetForward = _enemy.forward;
        _enemy.transform.forward = _currentForward;
        return new ValueAction(ActionType.AIRotate, Util.AngleBetweenVector3(_currentForward, _targetForward, _enemy.up), _time);
    }
    public Action CreateAction()
    {
        return new ValueTargetAction(ActionType.AIRotate, _remainingRotation, _target); 
    }
	public static void SimulateAction(DummyEnemy _enemy, Vector3 _target, int index)
    {
        Vector3 _currentForward = _enemy.transform.forward;
        Vector3 _tempTarget = new Vector3(_target.x, _enemy.transform.position.y, _target.z);
        Vector3 _up = _enemy.transform.up; 
        _enemy.transform.LookAt(_tempTarget);
        float _angle = Util.AngleBetweenVector3(_currentForward, _enemy.transform.forward, _up);
		_enemy.SetAction(new ValueIntAction(ActionType.AIRotate, _angle, index, _enemy.Time));
        _enemy.AddToTIme(Math.Abs(_angle) / _enemy.RotationSpeed);
		_enemy.SetAction(new ValueIntAction(ActionType.AIRotateUnset, _angle, index, _enemy.Time));
    }
    public static void SimulateAction(IAI _ai, Vector3 _target, float _startTime)
    {
        Vector3 _tempTarget = new Vector3(_target.x, _ai.transform.position.y, _target.z);
        Vector3 _up = _ai.transform.up;
        Vector3 _dir = _tempTarget - _ai.transform.position;
        float _angle = Util.AngleBetweenVector3(_ai.transform.forward, _dir, _up);
        _ai.SetAction(new ValueTargetAction(ActionType.AIRotate, _angle, _target, _startTime));
        float _endTime = Math.Abs(_angle) / _ai.RotationSpeed + _startTime; 
        _ai.SetAction(new ValueTargetAction(ActionType.AIRotateUnset, _angle, _target, _endTime));
    }
}
