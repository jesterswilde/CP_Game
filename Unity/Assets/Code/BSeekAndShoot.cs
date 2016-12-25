using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BSeekAndShoot : MonoBehaviour, IBehavior
{
    [SerializeField]
    float _firingAngle;
    [SerializeField]
    float _timeToLoseTarget;
    float _timeSinceSeenTarget = 0;
    float _timeSinceLastCheck = 0; 
    float _checkInterval = 1f; 
    ITargetable _target;
    CombatState _combat; 
    IAI _self;
    bool _isRotating;
    bool _shouldFire = false; 

    void CheckIfStillSeeing(float _deltaTime)
    {
        _timeSinceLastCheck += _deltaTime;
        if (_timeSinceLastCheck > _checkInterval)
        {
            _timeSinceLastCheck -= _checkInterval;
           if(_target.Combat.CanSee(_self.transform.position))
            {
                _timeSinceSeenTarget = 0;
            }else
            {
                _timeSinceSeenTarget += _checkInterval; 
                if(_timeSinceSeenTarget > _timeToLoseTarget)
                {
                    EndBehavior(); 
                }
            }
        }
    }
    public void EndBehavior()
    {

    }
    public void StartBehavior(IAI self, ITargetable _target)
    {

    }
    void ShouldFireCheck()
    {
        if(Math.Abs(Util.AngleBetweenVector3(transform.forward, _target.Position - transform.forward, transform.up)) < _firingAngle)
        {
            if (!_shouldFire)
            {
                _self.SetAction(new Action(ActionType.PullTrigger), true);
                _shouldFire = true; 
            }
        }else
        {
            if (_shouldFire)
            {
                _shouldFire = false;
                _self.SetAction(new Action(ActionType.ReleaseTrigger), true); 
            }
        }
    }

    public List<IAction> Act(float _deltaTime, Transform _trans)
    {
        throw new NotImplementedException();
    }

    public void ReverseAct(float _deltaTime, Transform _trans)
    {
        throw new NotImplementedException();
    }

    public void ReverseAction(IAction _action)
    {
        throw new NotImplementedException();
    }

    public IAction UseAction(IAction _action)
    {
        throw new NotImplementedException();
    }
}
