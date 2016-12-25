using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBehavior
{
    IAction UseAction(IAction _action);
    void ReverseAction(IAction _action);
    List<IAction> Act(float _deltaTime, Transform _trans);
    void ReverseAct(float _deltaTime, Transform _trans);
    void StartBehavior(IAI self, ITargetable _target);
    void EndBehavior(); 
}
