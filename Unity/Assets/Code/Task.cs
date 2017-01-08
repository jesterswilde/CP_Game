using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task : MonoBehaviour {
    [SerializeField]
    bool _loop;
    public bool Loop { get { return _loop; } }
    [SerializeField]
    List<TaskElement> _actions = new List<  TaskElement>();

	public void SimulateTask(IAI _enemy, float _time)
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
                case AIActions.MoveTo:
                    AAMoveTo.SimulateAction(_dummy, _action.GetTarget());
                    break; 
            }
        }
        //_enemy.SetAction(new TaskAction(ActionType.UnsetTask, this, _dummy.Time)); 
        GameObject.Destroy(_go); 
    }
}
