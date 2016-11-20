using UnityEngine;
using System.Collections;
using System;

public class AAWait : IAtomicAction
{
    float _remainingWait;
    float _finishesAt;
    public float FinishesAt { get { return _finishesAt; } }
    public void Act(float _deltaTime)
    {
        _remainingWait -= _deltaTime; 
    }

    public void ActReverse(float _deltaTIme)
    {
        _remainingWait += _deltaTIme; 
    }
    void CalculateFinishTime(IAction _action)
    {
        _finishesAt = _action.Time + _action.Value; 
    }
    public void ReverseAction(IAction _action, float _time)
    {
        _remainingWait = _action.Time + _action.Value - _time;
        CalculateFinishTime(_action);  
    }

    public void UseAction(IAction _action, float _time)
    {
        _remainingWait = _action.Time + _action.Value - _time;
        CalculateFinishTime(_action);  
    }

    public static IAction CreateAction(Enemy _enemy, float _value, float _time)
    {
        return new ValueAction(ActionType.AIWait, _value, _time); 
    }
    public static void SimulateAction(DummyEnemy _enemy, float _waitAmount)
    {
        _enemy.SetAction(new ValueAction(ActionType.AIWait, _waitAmount, _enemy.Time));
        _enemy.AddToTIme(_waitAmount); 
    }
}
