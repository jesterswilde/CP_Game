using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBehavior
{
    Action UseAction(Action _action);
    void ReverseAction(Action _action);
    List<Action> Act(float _deltaTime);
    void ReverseAct(float _deltaTime);
    void StartBehavior(IAI self, CombatState _target);
    Action EndBehavior();
    List<Action> StartPlay(float _deltaTime); 
}
