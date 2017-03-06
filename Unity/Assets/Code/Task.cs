using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task : MonoBehaviour {
    [SerializeField]
    bool _loop;
    public bool Loop { get { return _loop; } }
    [SerializeField]
    bool _default;
    public bool Default { get { return _default; } }
    [SerializeField]
    List<TaskElement> _taskElements = new List<TaskElement>();

	public void SimulateTask(IAI _enemy, float _time){
		SimulateTask (_enemy, _time, 0); 
	}

	public void SimulateTask(IAI _enemy, float _time, int index)
    {
        GameObject _go = new GameObject(); 
        DummyEnemy _dummy = _go.AddComponent<DummyEnemy>();
        _dummy.Startup(_enemy, _time); 
        for(int i = index; i < _taskElements.Count; i++)
        {
            TaskElement _taskElement = _taskElements[i]; 
            switch (_taskElement.Action)
            {
                case AIActions.MoveFoward:
                    AAMoveForward.SimulateAction(_dummy, _taskElement.GetTarget(), i);
                    break;
                case AIActions.LookAt:
                    AARotate.SimulateAction(_dummy, _taskElement.GetTarget(), i);
                    break;
                case AIActions.Wait:
                    AAWait.SimulateAction(_dummy, _taskElement.Value, i);
                    break;
                case AIActions.MoveTo:
                    AAMoveTo.SimulateAction(_dummy, _taskElement.GetTarget(), i);
                    break; 
            }
        }
        //_enemy.SetAction(new TaskAction(ActionType.UnsetTask, this, _dummy.Time)); 
        GameObject.Destroy(_go); 
    }
}
