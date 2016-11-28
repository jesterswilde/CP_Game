using UnityEngine;
using System.Collections;

public interface IAtomicAction {

    void Act(float _deltaTime);
    void ActReverse(float _deltaTIme);
    void UseAction(IAction _action, float _time);
    void ReverseAction(IAction _action, float _time);
    float FinishesAt { get; }
    IAction Unset(); 
}
