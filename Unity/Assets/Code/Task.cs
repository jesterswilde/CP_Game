﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task : MonoBehaviour {
    [SerializeField]
    List<TaskElement> _actions = new List<TaskElement>();

	public void SetRoutine(Enemy _enemy, float _time)
    {
        GameObject _go = new GameObject(); 
        DummyEnemy _dummy = _go.AddComponent<DummyEnemy>();
        _dummy.Startup(_enemy, _time); 
        for(int i = 0; i < _actions.Count; i++)
        {
            TaskElement _action = _actions[i]; 
            switch (_action.Action)
            {
                case AIActions.MoveFoward:
                    AAMoveForward.SimulateAction(_dummy, _action.GetTarget());
                    break;
                case AIActions.LookAt:
                    AARotate.SimulateAction(_dummy, _action.GetTarget());
                    break;
                case AIActions.Wait:
                    AAWait.SimulateAction(_dummy, _action.Value);
                    break; 
            }
        }
        GameObject.Destroy(_go); 
    }
}