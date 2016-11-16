using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;

public class Enemy : WibblyWobbly {
    [SerializeField]
    float _moveSpeed; 
    [SerializeField]
    float _rotationSpeed;
    [SerializeField]
    List<AITask> _tasks;
    int _taskIndex = 0;

    AIActions _currentAction;
    Vector3 _target;
    float _rotAmount; 

    IAction ActionFromTask(AITask _task)
    {
        return ActionFromTask(_task, GameManager.GameTime); 
    }
    IAction ActionFromTask(AITask _task, float _time)
    {
        switch (_task.Action)
        {
            case AIActions.Move:
                return new VectorAction(ActionType.AIMove, _task.Target.position, _time);
            case AIActions.Wait:
                return new ValueAction(ActionType.AIWait, _task.Value, _time);
            case AIActions.Rotate:
                return new ValueAction(ActionType.AIRotate, _task.Value, _time);
            default:
                return new Action(ActionType.Clear);
        }
    }
    void ExecuteNextTask(float _time)
    {
        if(_tasks.Count > 0)
        {
            SetAction(ActionFromTask(_tasks[_taskIndex], _time));
            _taskIndex = (_taskIndex + 1) % _tasks.Count; 
        }
    }
    protected override void UseAction(IAction _action)
    {
        switch (_action.Type)
        {
            case ActionType.AIMove:
                _currentAction = AIActions.Move;
                _target = _action.Vector; 
                break;
            case ActionType.AIRotate:
                _currentAction = AIActions.Rotate;
                _rotAmount = _action.Value; 
                break;
            case ActionType.AIWait:
                _currentAction = AIActions.Wait;
                break;
            default:
                break; 
        }
    }

    protected override void ReverseAction(IAction _action)
    {
       
    }

    protected override void Act(float _deltaTime)
    {
        throw new NotImplementedException();
    }

    protected override void ActReverse(float _deltaTIme)
    {
        throw new NotImplementedException();
    }

    void CheckWaiting()
    {
        float _timeFinished = _history.PointerAction.Time + _history.PointerAction.Value; 
        if(_timeFinished < GameManager.GameTime)
        {
            ExecuteNextTask(_timeFinished);
        }
    }
    void CheckRotation()
    {
        
    }
    void CheckMovement()
    {
        float _distSqr = _moveSpeed * _moveSpeed * Time.deltaTime - (transform.position - _target).sqrMagnitude;
        if(_distSqr > 0)
        {
            float _remainder = _moveSpeed * Time.deltaTime - (transform.position - _target).magnitude;
            float _newStateDuration = (_remainder / Time.deltaTime) * Time.deltaTime;
            ExecuteNextTask(GameManager.GameTime - _newStateDuration)
        }
    }
}

public enum AIActions
{
    Move,
    Rotate,
    Wait
}
