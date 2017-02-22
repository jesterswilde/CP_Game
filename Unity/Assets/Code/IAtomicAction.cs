using UnityEngine;
using System.Collections;

public interface IAtomicAction {

    void Act(float _deltaTime);
    void ActReverse(float _deltaTIme);
    void UseAction(Action _action, float _time);
    void ReverseAction(Action _action, float _time);
    float FinishesAt { get; }
    Action Unset();
    Action CreateAction(); 
}
