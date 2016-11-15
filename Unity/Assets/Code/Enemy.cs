using UnityEngine;
using System.Collections;
using System;

public class Enemy : WibblyWobbly {
    [SerializeField]
    float _rotationSpeed;

    AIActions _currentAction;

    protected override void UseAction(IAction _action)
    {
        switch (_action.Type)
        {
            case ActionType.AIMove:
                _currentAction = AIActions.Move;
                break;
            case ActionType.AIRotate:
                _currentAction = AIActions.Rotate;
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
}

public enum AIActions
{
    Move,
    Rotate,
    Wait
}
