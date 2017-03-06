using UnityEngine;
using System.Collections;
using System;

public class AAWait : IAtomicAction
{
    float _remainingWait;
    float _finishesAt;
    float _maxWait; 
    public float FinishesAt { get { return _finishesAt; } }
	int _index; 

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
		_index = _action.IValue; 
        _maxWait = _action.Value; 
        _remainingWait = _action.Time + _action.Value - _time;
        CalculateFinishTime(_action);  
    }

    public void UseAction(Action _action, float _time)
    {
		_index = _action.IValue; 
        _maxWait = _action.Value;  
        _remainingWait = _action.Time + _action.Value - _time;
        CalculateFinishTime(_action);
    }

    public Action Unset()
    {
		return new ValueIntAction(ActionType.AIWaitUnset, _maxWait, _index);
    }

    public static Action CreateAction(Enemy _enemy, float _value, float _time)
    {
        return new ValueAction(ActionType.AIWait, _value, _time); 
    }
    public Action CreateAction()
    {
        return new ValueAction(ActionType.AIWait, _remainingWait);
    }
	public static void SimulateAction(DummyEnemy _enemy, float _waitAmount, int index)
    {
        _enemy.SetAction(new ValueIntAction(ActionType.AIWait, _waitAmount, index, _enemy.Time));
        _enemy.AddToTIme(_waitAmount);
		_enemy.SetAction(new ValueIntAction(ActionType.AIWaitUnset, _waitAmount, index, _enemy.Time));
    }
}
