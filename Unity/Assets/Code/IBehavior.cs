using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBehavior
{
    IAction UseAction(IAction _action);
    void ReverseAction(IAction _action);
    List<IAction> Act(float _deltaTime);
    void ReverseAct(float _deltaTime);
    void StartBehavior(IAI self, CombatState _target);
    IAction EndBehavior();
    List<IAction> StartPlay(float _deltaTime); 
}
