using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WibblyWobbly : MonoBehaviour {

    protected History _history = new History();
    protected List<IAction> _actionsToBeConsumed = new List<IAction>();
    protected abstract void UseAction(IAction _action, float _time);
    protected abstract void ReverseAction(IAction _action, float _time); 
    protected abstract void Act(float _deltaTime);
    protected abstract void ActReverse(float _deltaTIme);
    protected void RegisterWibblyWobbly()
    {
        GameManager.RegisterWibblyWobbly(this); 
    }
    public virtual void SetAction(IAction _action)
    {
        if (CanAddActions())
        {
            _actionsToBeConsumed.Add(_action); 
        }
    }
    public virtual void SetAction(List<IAction> _actions)
    {
        if (CanAddActions())
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
        }
        _actionsToBeConsumed.Clear();
    }
    protected bool CanAddActions()
    {
        return (_history.HeadAction == null || GameManager.IsPlaying && _history.IsPointerAtHead());
    }
    public virtual void SetExternalAction(IAction _action)
    {
        SetAction(_action); 
    }
    protected float _prevTime;
    public virtual void Play()
    {
        while (true)
        {
            HistoryNode _node = _history.PlayTime(GameManager.GameTime);
            if (_node == null)
            {
                Act(GameManager.GameTime - _prevTime);
                _prevTime = GameManager.GameTime;
                break;
            }
            Act(_node.Action.Time - _prevTime);
            _prevTime = _node.Action.Time;
            UseAction(_node.Action, _prevTime);
        }
    }
    public virtual void Rewind()
    {
        while (true)
        {
            HistoryNode _node = _history.RewindTime(GameManager.GameTime);
            if (_node == null)
            {
                ActReverse(_prevTime - GameManager.GameTime);
                _prevTime = GameManager.GameTime;
                break;
            }
            ActReverse(_prevTime - _node.Action.Time);
            _prevTime = _node.Action.Time;
            ReverseAction(_node.Action, _prevTime);
        }
    }
}
