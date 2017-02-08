using UnityEngine;
using System.Collections;

public class HistoryNode {
    Action _action;
    HistoryNode _next;
    HistoryNode _prev; 
    public HistoryNode Next { get { return _next; } }
    public HistoryNode Previous { get { return _prev; } }
    public Action Action { get { return _action; } }


	public HistoryNode(Action action)
    {
        _action = action;
        _next = null;
        _prev = null; 
    }
    public HistoryNode(Action action, HistoryNode _nextNode, HistoryNode _prevNode)
    {
        _action = action;
        _next = _nextNode;
        _prev = _prevNode;
    }
    public void SetNext(HistoryNode _nextNode)
    {
        _next = _nextNode; 
    }
    public void SetPrevious(HistoryNode _prevNode)
    {
        _prev = _prevNode;
    }
    public void ClearNext()
    {
        _next = null;
    }
    public void ClearPrevious()
    {
        _prev = null; 
    }

}
