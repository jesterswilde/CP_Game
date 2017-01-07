using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;

public class Turret : Interactable, ITargetable {

    [SerializeField]
    bool _isActive = true;
    [SerializeField]
    bool _isAlerted = false;
    bool _isDead = false; 

    Renderer _renderer;
    Material _defaultMat; 
    Color _baseColor;
    CombatState _target;
    IBehavior _currentBehavior; 
    public bool IsVisible { get { return _renderer.isVisible; } }
    public Vector3 Position { get { return transform.position;  } }


    public bool isActivatable { get { return false; } }
    public bool isAttackable { get { return (_combat != null) ? true : false; } }
    public GameObject Go { get { return gameObject; } }
    public float MinDistanceToActivate { get { return float.NaN; } }

    public void Targeted()
    {
        _renderer.material.color = Color.red; 
    }

    public void UnTargeted()
    {
        _renderer.material.color = _baseColor; 
    }
    public override void ExternalTrigger(Task _task, TriggerType _trigger, Character _character)
    {
        if(_task != null)
        {
            SetTask(_task);
        }else
        {
            switch (_trigger)
            {
                case TriggerType.Activate:
                    if (!_isActive)
                    {
                        SetExternalAction(new Action(ActionType.Activate, true));
                    }
                    break;
                case TriggerType.Deactivate:
                    if (_isActive)
                    {
                        SetExternalAction(new Action(ActionType.Deactivate, true)); 
                    }
                    break;
                case TriggerType.Alert:
                    SawPlayer(_character.Combat);
                    break;
                case TriggerType.UnAlert:
                    if(_isActive && _isAlerted)
                    {
                        SetExternalAction(new Action(ActionType.UnAlert, true)); 
                    }
                    break; 
            }
        }
    }
    
    void SawPlayer(CombatState _target)
    {
        if (!_isAlerted && _isActive)
        {
            UnloadAll();
            SetAction(new Action(ActionType.Alert, true)); 
            BSeekAndShoot _behavior = new BSeekAndShoot();
            SetAction(new BehaviorAction(ActionType.BSeekAndShoot, _behavior, true));
            _behavior.StartBehavior(this, _target);
        }
    }
    protected override void UnloadAll()
    {
         _history.SpliceOffPossibleFuture(); 
        if(_currentAction != null)
        {
            SetAction(_currentAction.Unset()); 
        }
        if(_currentBehavior != null)
        {
            SetAction(_currentBehavior.EndBehavior()); 
        }
        if (_currentTask != null)
        {
            SetAction(new TaskAction(ActionType.UnsetTask, _currentTask, true));
        }
    }
    void Alert(IAction _action)
    {
        _isAlerted = true; 
    }
    void UnAlert(IAction _action)
    {
        _isAlerted = false; 
        if(_currentBehavior != null)
        {
            SetAction(_currentBehavior.EndBehavior()); 
        }
    }
    void Activate(IAction _action)
    {
        _renderer.material = _defaultMat;
        _baseColor = _renderer.material.color; 
        _isActive = true;
    }
    void Deactivate(IAction _action)
    {
        _renderer.material = GameSettings.InactiveMaterial;
        _baseColor = _renderer.material.color; 
        _isActive = false;
    }



    protected override void UseAction(IAction _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.BSeekAndShoot:
                _currentBehavior = _action.Behavior; 
                break;
            case ActionType.BSeekAndShootUnset:
                _currentBehavior = null;
                break;
            case ActionType.SawPlayer:
                SawPlayer(_action.Combat);
                break; 
            case ActionType.Alert:
                Alert(_action); 
                break;
            case ActionType.UnAlert:
                UnAlert(_action); 
                break;
            case ActionType.Die:
                UnloadAll(); 
                _isDead = true;
                break;
            case ActionType.Activate:
                Activate(_action);
                break;
            case ActionType.Deactivate:
                Deactivate(_action);
                break; 
        }
        if(_currentBehavior != null)
        {
            SetAction(_currentBehavior.UseAction(_action)); 
        }
        base.UseAction(_action, _time); 
    }

    protected override void ReverseAction(IAction _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.BSeekAndShoot:
                _currentBehavior = null;
                break;
            case ActionType.BSeekAndShootUnset:
                _currentBehavior = _action.Behavior;
                break;
            case ActionType.Alert:
                _isAlerted = false; 
                break;
            case ActionType.UnAlert:
                _isAlerted = true; 
                break;
            case ActionType.Die:
                _isDead = false;
                break;
            case ActionType.Deactivate:
                Activate(_action);
                break;
            case ActionType.Activate:
                Deactivate(_action);
                break; 
        }
        if(_currentBehavior != null)
        {
            _currentBehavior.ReverseAction(_action); 
        }
        base.ReverseAction(_action, _time); 
    }

    protected override void StartPlay(float _deltaTime)
    {
        if(_currentBehavior != null)
        {
            _currentBehavior.StartPlay(_deltaTime); 
        }
        base.StartPlay(_deltaTime); 
    }

    protected override void Act(float _deltaTime)
    {
        if (_isActive && !_isDead)
        {
            if (_currentBehavior != null)
            {
                _currentBehavior.Act(_deltaTime);
                if (_currentAction != null)
                {
                    _currentAction.Act(_deltaTime);
                }
            }
            else
            {
                base.Act(_deltaTime);
            }
        }
    }

    protected override void ActReverse(float _deltaTIme)
    {
        if (!_isDead && _isActive)
        {
            if(_currentBehavior != null)
            {
                _currentBehavior.ReverseAct(_deltaTIme); 
            }
            base.ActReverse(_deltaTIme); 
        }
    }


    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _baseColor = _renderer.material.color;
        _defaultMat = _renderer.material; 
        _defaultTask = _currentTask;
        _combat = GetComponent<CombatState>();
        if (_combat != null)
        {
            _combat.SetCallbacks(new SetActionDelegate(SetAction));
        }
    }
    void Start()
    {
        GameManager.RegisterTargetable(this);
        GameManager.RegisterWibblyWobbly(this);
        SetAction(new Action(ActionType.Null));
        if (_isActive)
        {
            Activate(new Action(ActionType.Null));
        }else
        {
            Deactivate(new Action(ActionType.Null)); 
        }
    }

    public void RewindActivation(IAction _action)
    {
        throw new NotImplementedException();
    }

    public IAction Activate(Character _character)
    {
        throw new NotImplementedException();
    }
}
