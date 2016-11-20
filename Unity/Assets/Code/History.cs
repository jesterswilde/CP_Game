﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class History {

    HistoryNode _head = null;
    HistoryNode _tail = null;
    HistoryNode _pointer = null;
    public HistoryNode Pointer { get { return _pointer; } }
    int _count = 0; 
    public int Count { get { return _count;  } }

    public IAction TailAction  { get{ return (_tail != null) ? _tail.Action : null; } }
    public IAction HeadAction { get { return (_head != null) ? _head.Action : null; } }
    public IAction PointerAction { get { return (_pointer != null) ? _pointer.Action : null; } }

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
    public void AddToHead(IAction _action)
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
            return _node;
        }
        return null; 
    }

}
