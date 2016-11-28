using UnityEngine;
using System.Collections;
using System;

public class AARotate : IAtomicAction
{
    float _maxRotation;
    float _remainingRotation;
    int _dir;
    float _finishesAt;
    public float FinishesAt { get { return _finishesAt; } }
    IAI _enemy;

    public AARotate(IAI enemy)
    {
        _enemy = enemy; 
    }

    public void Act(float _deltaTime)
    {
        Vector3 _forward = _enemy.transform.forward;
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

    public void ReverseAction(IAction _action, float _time)
    {
        _remainingRotation = _action.Value - (_time - _action.Time) * _enemy.RotationSpeed;
        _maxRotation = _action.Value;
        _dir = (_action.Value > 0) ? 1 : -1;
        CalculateFinishTime(_action); 
    }

    public void UseAction(IAction _action, float _time)
    {
        _remainingRotation = _action.Value;
        _maxRotation = _action.Value;
        _dir = (_action.Value > 0) ? 1 : -1;
        CalculateFinishTime(_action);
    }
    void CalculateFinishTime(IAction _action)
    {
        _finishesAt = _action.Time + Math.Abs(_action.Value) / _enemy.RotationSpeed;
    }

    public IAction Unset()
    {
        return new ValueAction(ActionType.AIRotateUnset, _maxRotation); 
    }

    public static IAction CreateAction(Transform _enemy, Vector3 _target, float _time)
    {
        Vector3 _currentForward = _enemy.forward;
        Vector3 _tempTarget = new Vector3(_target.x, 0, _target.z); 
        _enemy.transform.LookAt(_tempTarget);
        Vector3 _targetForward = _enemy.forward;
        _enemy.transform.forward = _currentForward;
        return new ValueAction(ActionType.AIRotate, Util.AngleBetweenVector3(_currentForward, _targetForward, _enemy.up), _time);
    }
    public static void SimulateAction(DummyEnemy _enemy, Vector3 _target)
    {
        Vector3 _currentForward = _enemy.transform.forward;
        Vector3 _tempTarget = new Vector3(_target.x, _enemy.transform.position.y, _target.z);
        Vector3 _up = _enemy.transform.up; 
        _enemy.transform.LookAt(_tempTarget);
        float _angle = Util.AngleBetweenVector3(_currentForward, _enemy.transform.forward, _up);
        _enemy.SetAction(new ValueAction(ActionType.AIRotate, _angle, _enemy.Time));
        _enemy.AddToTIme(Math.Abs(_angle) / _enemy.RotationSpeed);
        _enemy.SetAction(new ValueAction(ActionType.AIRotateUnset, _angle, _enemy.Time));
    }
}
