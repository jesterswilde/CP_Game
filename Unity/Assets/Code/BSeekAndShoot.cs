using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BSeekAndShoot : IBehavior
{
    float _firingAngle = 90;
    float _timeToLoseTarget = 2f;
    float _timeSinceSeenTarget = 0;
    float _timeSinceLastCheck = 0; 
    float _checkInterval = 0.5f;
    float _distanceThrehold = 0.1f;
    Vector3 _lastKnownPostion; 
    CombatState _target;
    CombatState _combat; 
    IAI _self;
    float _rotation = 0;
    bool _shouldFire = false;


    public void StartBehavior(IAI self, CombatState target)
    {
        _target = target;
        _self = self;
        _combat = _self.Combat;
        _lastKnownPostion = _target.transform.position;
        _self.SetAtomicAction(ActionType.AIRotate, _self, _target.transform.position, GameManager.FixedGameTime); 
    }
    void ReturnControl()
    {
        _self.SetAction(new BasicAction(ActionType.UnAlert, true)); 
    }
    public Action EndBehavior()
    {
        return new BehaviorAction(ActionType.BSeekAndShootUnset, this, true); 
    }
    void SawTarget(Action _action)
    {
        if (!float.IsNaN(_action.Value))
        {
            _timeSinceSeenTarget = 0;
        }
    }
    void ReverseSawTarget(Action _action)
    {
        if (float.IsNaN(_action.Value))
        {
            _lastKnownPostion = _action.Vector;
        }else
        {
            _timeSinceSeenTarget += _action.Value; 
        }
    }
    void DidntSeeTarget()
    {
        _timeSinceSeenTarget += _checkInterval;
        if (_timeSinceSeenTarget > _timeToLoseTarget)
        {
            Debug.Log("leaving seek");
            ReturnControl();
        }
    }
    void ReverseDidntSeeTarget()
    {
        _timeSinceSeenTarget -= _checkInterval; 
    }
    void CheckIfStillSeeing(float _deltaTime)
    {
        _timeSinceLastCheck += _deltaTime;
        if (_timeSinceLastCheck > _checkInterval)
        {
            _timeSinceLastCheck -= _checkInterval;
            if (_target.CanSee(_self.transform.position))
            {
                _self.SetAction(new ValueAction(ActionType.SawTarget, _timeSinceSeenTarget, true));
                _self.SetAction(new VectorAction(ActionType.SawTarget, _lastKnownPostion, true));
                if(Vector3.Distance(_lastKnownPostion, _target.transform.position) > _distanceThrehold)
                {
                    _self.SetAtomicAction(ActionType.AIRotate, _self, _target.transform.position, GameManager.FixedGameTime); 
                }
                _lastKnownPostion = _target.transform.position; 
            }
            else
            {
                _timeSinceSeenTarget += _checkInterval;
                if (_timeSinceSeenTarget > _timeToLoseTarget)
                {
                    _self.SetAction(new BasicAction(ActionType.DidntSeeTarget, true)); 
                }
            }
        }
        if(_timeSinceLastCheck < 0)
        {
            _timeSinceLastCheck += _checkInterval;
        }
    }
    void AttemptToFire()
    {
        
        if (Math.Abs(_rotation) < _firingAngle)
        {
            if (!_shouldFire)
            {
                _self.SetAction(new BasicAction(ActionType.PullTrigger, true), true);
                _shouldFire = true; 
            }
        }else
        {
            if (_shouldFire)
            {
                Debug.Log("ending fire"); 
                _shouldFire = false;
                _self.SetAction(new BasicAction(ActionType.ReleaseTrigger, true), true); 
            }
        }
    }
    public List<Action> StartPlay(float _deltaTime)
    {
        CheckIfStillSeeing(_deltaTime);
        return null;
    }
    public List<Action> Act(float _deltaTime)
    {
        if(_combat != null)
        {
            AttemptToFire(); 
            _self.SetAction(_combat.PullTrigger(GameManager.FixedGameTime, _target)); 
        }
        return null;
    }

    public void ReverseAct(float _deltaTime)
    {
        CheckIfStillSeeing(_deltaTime);
    }

    public Action UseAction(Action _action)
    {
        switch (_action.Type)
        {
            case ActionType.SawTarget:
                SawTarget(_action);
                break;
            case ActionType.DidntSeeTarget:
                DidntSeeTarget();
                break;
        }
        return null; 
    }
    public void ReverseAction(Action _action)
    {
        switch (_action.Type)
        {
            case ActionType.SawTarget:
                ReverseSawTarget(_action);
                break;
            case ActionType.DidntSeeTarget:
                ReverseDidntSeeTarget();
                break;
        }
    }
}
