using UnityEngine;
using System.Collections;

public abstract class WibblyWobbly : MonoBehaviour {

    protected History _history = new History();
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
        if(_history.HeadAction == null)
        {
            _history.AddToHead(_action);
            return; 
        }
        if (GameManager.IsPlaying && _action.Time >= _history.HeadAction.Time)
        {
            _history.AddToHead(_action); 
        }
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
