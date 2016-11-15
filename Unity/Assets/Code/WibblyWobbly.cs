using UnityEngine;
using System.Collections;

public abstract class WibblyWobbly : MonoBehaviour {

    protected History _history = new History();
    protected abstract void UseAction(IAction _action);
    protected abstract void ReverseAction(IAction _action); 
    protected abstract void Act(float _deltaTime);
    protected abstract void ActReverse(float _deltaTIme);
    public virtual void SetAction(IAction _action)
    {
        _history.AddToHead(_action); 
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
            UseAction(_node.Action);
            _prevTime = _node.Action.Time;
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
            ReverseAction(_node.Action);
            _prevTime = _node.Action.Time;
        }
    }
}
