using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WibblyWobbly : MonoBehaviour {

    protected History _history = new History();
    protected List<Action> _actionsToBeConsumed = new List<Action>();
    protected CombatState _combat; 
    protected abstract void UseAction(Action _action, float _time);
    protected abstract void ReverseAction(Action _action, float _time); 
    protected abstract void Act(float _deltaTime);
    protected abstract void ActReverse(float _deltaTIme);
    public CombatState Combat { get { return _combat; } }
    protected void RegisterWibblyWobbly()
    {
        GameManager.RegisterWibblyWobbly(this); 
    }
    public virtual void SetAction(Action _action, bool _determineExternal)
    {
        if(_action == null)
        {
            return; 
        }
        if (_action.IsExternal)
        {
            SetExternalAction(_action);
        }else
        {
            SetAction(_action); 
        }
    }
    public virtual void SetAction(Action _action)
    {
        if (CanAddActions() && _action != null)
        {
            _actionsToBeConsumed.Add(_action); 
        }
    }
    public virtual void SetAction(List<Action> _actions)
    {
        if (CanAddActions() && _actions.Count > 0)
        {
            _actionsToBeConsumed.AddRange(_actions); 
        }
    }
    public virtual void SetAction(List<Action> _actions, bool _determinExternal)
    {
        if (CanAddActions() && _actions.Count > 0)
        {
            for(int i = 0; i < _actions.Count; i++)
            {
                if (_actions[i].IsExternal)
                {
                    SetExternalAction(_actions[i]);
                }else
                {
                    SetAction(_actions[i]); 
                }
            }
        }
    }
	protected virtual void InsertPreviousAction(Action _action){
		_history.InsertBeforePointer (_action); 
	}
    public virtual void ApplyActions()
    {
        if (CanAddActions())
        {
            for (int i = 0; i < _actionsToBeConsumed.Count; i++)
            {
                _history.AddToHead(_actionsToBeConsumed[i]);
            }
            _actionsToBeConsumed.Clear();
        }
    }
    protected bool CanAddActions()
    {
        return (_history.HeadAction == null || GameManager.IsPlaying && _history.IsPointerAtHead());
    }
    public virtual void SetExternalAction(Action _action)
    {
        if (GameManager.IsPlaying && _action != null)
        {
            _history.InsertAfterPointer(_action); 
        }
    }
    public virtual void SetExternalAction(Action _action, bool _movePointer)
    {
        if (GameManager.IsPlaying && _action != null)
        {
            _history.InsertAfterPointer(_action, _movePointer);
        }
    }
    public virtual void SetExternalAction(List<Action> _actions)
    {
        if (GameManager.IsPlaying && _actions != null)
        {
            for(int i = 0; i < _actions.Count; i++)
            {
                _history.InsertAfterPointer(_actions[i]); 
            }
        }
    }
    public virtual void SetExternalAction(List<Action> _actions, bool _movePointer)
    {
        if (GameManager.IsPlaying && _actions != null)
        {
            for (int i = 0; i < _actions.Count; i++)
            {
                _history.InsertAfterPointer(_actions[i], _movePointer);
            }
        }
    }
    protected float _prevTime;
    public virtual void Play(float _time)
    {
        while (true)
        {
            HistoryNode _node = _history.PlayTime(_time);
            if (_node == null)
            {
                Act(_time - _prevTime);
                if (_history.IsPointerAtHead() || _history.Pointer.Next.Action.Time > _time)
                {
                    _prevTime = _time;
                    break;
                }else
                {
                    _prevTime = _history.Pointer.Next.Action.Time; 
                }
            }else
            {
                Act(_node.Action.Time - _prevTime);
                _prevTime = _node.Action.Time;
                UseAction(_node.Action, _prevTime);
            }
        }
    }
    public virtual void Rewind(float _time)
    {
        while (true)
        {
            HistoryNode _node = _history.RewindTime(_time);
            if (_node == null)
            {
                ActReverse(_prevTime - _time);
                _prevTime = _time;
                break;
            }
            ActReverse(_prevTime - _node.Action.Time);
            _prevTime = _node.Action.Time;
            ReverseAction(_node.Action, _prevTime);
        }
    }
}
