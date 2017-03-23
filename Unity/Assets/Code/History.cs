using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class History {

    HistoryNode _head = null;
    HistoryNode _tail = null;
    HistoryNode _pointer = null;
    public HistoryNode Pointer { get { return _pointer; } }
    public HistoryNode Tail { get { return _tail; } }
    int _count = 0; 
    public int Count { get { return _count;  } }

    public Action TailAction  { get{ return (_tail != null) ? _tail.Action : null; } }
    public Action HeadAction { get { return (_head != null) ? _head.Action : null; } }
    public Action PointerAction { get { return (_pointer != null) ? _pointer.Action : null; } }

    HistoryNode ChopTail()
    {
        if(_tail != null)
        {
            _count--; 
            HistoryNode _chopped = _tail; 
            if(_tail.Next == null)
            {
                _head = null;
                _pointer = null; 
            }
            _tail.ClearPrevious(); 
            _tail = _tail.Next;
            return _chopped; 
        }
        return null; 
    }
    public void AddToHead(Action _action)
    {
        _count++;
        HistoryNode _newHead = new HistoryNode(_action);
        if(_tail == null)
        {
            _head = _newHead;
            _tail = _newHead;
            _pointer = _newHead; 
        }
        else
        {
            _head.SetNext(_newHead);
            _newHead.SetPrevious(_head); 
            _head = _newHead;
        }
    }
    public void InsertAfterPointer(Action _action)
    {
        _count++;
        if(_head == null)
        {
            AddToHead(_action);
            return;
        }
        HistoryNode _node = new HistoryNode(_action);
        HistoryNode _next = _pointer.Next;
        _pointer.SetNext(_node);
        _node.SetNext(_next);
        _node.SetPrevious(_pointer);
        if(_next != null)
        {
            _next.SetPrevious(_node);
        }else
        {
            _head = _node; 
        }
    }
    public void InsertAfterPointer(Action _action, bool _moveForward)
    {
        InsertAfterPointer(_action);
        if (_moveForward)
        {
            _pointer = _pointer.Next; 
        }
    }
	public void InsertBeforePointer(Action _action){
		if (_head != null) {
			HistoryNode _node = new HistoryNode (_action); 
			HistoryNode _prev = _pointer.Previous; 
			if (_prev != null) {
				_prev.SetNext (_node); 
			}
			_node.SetPrevious (_prev); 
			_node.SetNext (_pointer); 
			_pointer.SetPrevious (_node); 
		} else {
			AddToHead (_action); 
		}
	}
    public HistoryNode PopNode(HistoryNode _node)
    {
        if(_node == null)
        {
            return null; 
        }
        _count--;
        if(_node.Next == null && _node.Previous == null)
        {
            _head = null;
            _tail = null;
            return _node; 
        }
        if(System.Object.ReferenceEquals(_node, _head))
        {
            _head = _head.Previous;
            _head.SetNext(null);
            return _node;  
        }
        if(System.Object.ReferenceEquals(_node, _tail))
        {
            _tail = _tail.Next;
            _tail.SetPrevious(null);
            return _node;  
        }
        if(System.Object.ReferenceEquals(_node, _pointer))
        {
            _pointer = _pointer.Previous; 
        }
        _node.Previous.SetNext(_node.Next);
        _node.Next.SetPrevious(_node.Previous); 
        return _node; 
    }

    public void ClearFromPointer()
    {
        if(_head == null || _pointer == null)
        {
            return;
        }
        _head = _pointer;
        _head.ClearNext(); 
    }

    public void ClearToPointer(float _time)
    {
        
    }

    public void SetCurrentTime(float _time)
    {
        if(_tail == null)
        {
            _pointer = null; 
            return; 
        }
        _pointer = _tail;
        PlayTime(_time); 
    }
    public bool IsPointerAtHead()
    {
        return System.Object.ReferenceEquals(_pointer, _head); 
    }
    public bool IsPointerAtTail()
    {
        return System.Object.ReferenceEquals(_pointer, _tail);
    }
    public void SpliceOffPossibleFuture()
    {
        HistoryNode _next = _pointer.Next; 
        ClearFromPointer(); 
        AddToHead(new FutureActions(ActionType.SpliceFuture, _next, GameManager.FixedGameTime));
        _pointer = _head;
    }
    public HistoryNode CreateBranch(Action _action)
    {
        HistoryNode _node = new HistoryNode(_action);
        _node.SetNext(_pointer.Next);
        return _node; 
    }
    public void ReloadPossibleFuture(HistoryNode _future)
    {
        _pointer.SetNext(_future);
        HistoryNode _node = _pointer;
        while (true)
        {
            if(_node.Next == null)
            {
                _head = _node;
                break; 
            }
            _node = _node.Next; 
        }
    }
    public HistoryNode PlayTime(float _time)
    {
        if(_pointer == null || _pointer.Next == null)
        {
            return null;
        }
        if (_pointer.Next.Action.Time <= _time)
        {
            _pointer = _pointer.Next;
            return _pointer; 
        }
        return null; 
    }

    public HistoryNode RewindTime(float _time)
    {
        if (_pointer == null || _pointer.Previous == null)
        {
            return null;
        }
        if(_pointer.Action.Time > _time)
        {
            HistoryNode _node = _pointer;
            _pointer = _pointer.Previous;
            if (_node.Action.IsExternal)
            {
                return PopNode(_node);
            } 
            return _node;
        }
        return null; 
    }

}
