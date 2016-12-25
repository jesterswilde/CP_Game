using UnityEngine;
using System.Collections;
using System;

public class AARotateDir : IAtomicAction
{
    int _dir;
    float _finishesAt;
    public float FinishesAt { get { return _finishesAt; } }
    IAI _enemy;

    public AARotateDir(IAI enemy)
    {
        _enemy = enemy;
    }

    public void Act(float _deltaTime)
    {
        Vector3 _forward = _enemy.transform.forward;
        float _rotAmount = _deltaTime * _enemy.RotationSpeed * _dir;
        _enemy.transform.Rotate(new Vector3(0, _rotAmount, 0));
    }

    public void ActReverse(float _deltaTime)
    {
        float _rotAmount = _deltaTime * _enemy.RotationSpeed * _dir * -1;
        _enemy.transform.Rotate(new Vector3(0, _rotAmount, 0));
    }

    public void ReverseAction(IAction _action, float _time)
    {
        _dir = (_action.Value > 0) ? 1 : -1;
    }

    public void UseAction(IAction _action, float _time)
    {
        _dir = (_action.Value > 0) ? 1 : -1;
    }
    public IAction Unset()
    {
        return new ValueAction(ActionType.AIRotateDirUnset, _dir * 1f);
    }

    public static IAction CreateAction(Transform _enemy, Vector3 _target, float _time)
    {
        Vector3 _currentForward = _enemy.forward;
        Vector3 _tempTarget = new Vector3(_target.x, 0, _target.z);
        _enemy.transform.LookAt(_tempTarget);
        Vector3 _targetForward = _enemy.forward;
        _enemy.transform.forward = _currentForward;
        return new ValueAction(ActionType.AIRotateDir, Util.AngleBetweenVector3(_currentForward, _targetForward, _enemy.up), _time);
    }
    public static void SimulateAction(DummyEnemy _enemy, Vector3 _target)
    {
        throw new Exception("Cannot be simulated"); 
    }
}
