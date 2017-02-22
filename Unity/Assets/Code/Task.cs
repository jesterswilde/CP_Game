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
    List<TaskElement> _taskElements = new List<  TaskElement>();

	public void SimulateTask(IAI _enemy, float _time)
    {
        GameObject _go = new GameObject(); 
        DummyEnemy _dummy = _go.AddComponent<DummyEnemy>();
        _dummy.Startup(_enemy, _time); 
        for(int i = 0; i < _taskElements.Count; i++)
        {
            TaskElement _taskElement = _taskElements[i]; 
            switch (_taskElement.Action)
            {
                case AIActions.MoveFoward:
                    AAMoveForward.SimulateAction(_dummy, _taskElement.GetTarget());
                    break;
                case AIActions.LookAt:
                    AARotate.SimulateAction(_dummy, _taskElement.GetTarget());
                    break;
                case AIActions.Wait:
                    AAWait.SimulateAction(_dummy, _taskElement.Value);
                    break;
                case AIActions.MoveTo:
                    AAMoveTo.SimulateAction(_dummy, _taskElement.GetTarget());
                    break; 
            }
        }
        //_enemy.SetAction(new TaskAction(ActionType.UnsetTask, this, _dummy.Time)); 
        GameObject.Destroy(_go); 
    }
    public static void ResumeTask(HistoryNode _node, IAI _enemy, float _time)
    {
        GameObject _go = new GameObject();
        DummyEnemy _dummy = _go.AddComponent<DummyEnemy>();
        _dummy.Startup(_enemy, _time);
        while(_node != null)
        {
            switch (_node.Action.Type)
            {
                case ActionType.AIMoveForward:
                    AAMoveForward.SimulateAction(_dummy, _node.Action.OriginalVec);
                    break;
                case ActionType.AIRotate:
                    AARotate.SimulateAction(_dummy, _node.Action.OriginalVec);
                    break;
                case ActionType.AIWait:
                    AAWait.SimulateAction(_dummy, _node.Action.Value);
                    break;
                case ActionType.AIMoveTo:
                    AAMoveTo.SimulateAction(_dummy, _node.Action.OriginalVec);
                    break;
            }
            _node = _node.Next; 
        }
    }
}
