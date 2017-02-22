using UnityEngine;
using System.Collections;
using System;

public class AAWait : IAtomicAction
{
    float _remainingWait;
    float _finishesAt;
    float _maxWait; 
    public float FinishesAt { get { return _finishesAt; } }
    public void Act(float _deltaTime)
    {
        _remainingWait -= _deltaTime; 
    }

    public void ActReverse(float _deltaTIme)
    {
        _remainingWait += _deltaTIme; 
    }
    void CalculateFinishTime(Action _action)
    {
        _finishesAt = _action.Time + _action.Value; 
    }
    public void ReverseAction(Action _action, float _time)
    {
        _maxWait = _action.Value; 
        _remainingWait = _action.Time + _action.Value - _time;
        CalculateFinishTime(_action);  
    }

    public void UseAction(Action _action, float _time)
    {
        _maxWait = _action.Value;  
        _remainingWait = _action.Time + _action.Value - _time;
        CalculateFinishTime(_action);
    }

    public Action Unset()
    {
        return new ValueAction(ActionType.AIWaitUnset, _maxWait);
    }

    public static Action CreateAction(Enemy _enemy, float _value, float _time)
    {
        return new ValueAction(ActionType.AIWait, _value, _time); 
    }
    public Action CreateAction()
    {
        return new ValueAction(ActionType.AIWait, _remainingWait);
    }
    public static void SimulateAction(DummyEnemy _enemy, float _waitAmount)
    {
        _enemy.SetAction(new ValueAction(ActionType.AIWait, _waitAmount, _enemy.Time));
        _enemy.AddToTIme(_waitAmount);
        _enemy.SetAction(new ValueAction(ActionType.AIWaitUnset, _waitAmount, _enemy.Time));
    }
}
