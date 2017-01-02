using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WibblyWobbly : MonoBehaviour {

    protected History _history = new History();
    protected List<IAction> _actionsToBeConsumed = new List<IAction>();
    protected CombatState _combat; 
    protected abstract void UseAction(IAction _action, float _time);
    protected abstract void ReverseAction(IAction _action, float _time); 
    protected abstract void Act(float _deltaTime);
    protected abstract void ActReverse(float _deltaTIme);
    public CombatState Combat { get { return _combat; } }
    protected void RegisterWibblyWobbly()
    {
        GameManager.RegisterWibblyWobbly(this); 
    }
    public virtual void SetAction(IAction _action, bool _determineExternal)
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
    public virtual void SetAction(IAction _action)
    {
        if (CanAddActions() && _action != null)
        {
            _actionsToBeConsumed.Add(_action); 
        }
    }
    public virtual void SetAction(List<IAction> _actions)
    {
        if (CanAddActions() && _actions.Count > 0)
        {
            _actionsToBeConsumed.AddRange(_actions); 
        }
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
    public virtual void SetExternalAction(IAction _action)
    {
        if (GameManager.IsPlaying)
        {
            _history.InsertAfterPointer(_action); 
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
