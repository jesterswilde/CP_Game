using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;

public class Turret : Interactable {

    [SerializeField]
    bool _isActive = true;
    [SerializeField]
    bool _isAlerted = false;
    bool _isDead = false; 

    Renderer _renderer;
    Material _defaultMat; 
	Material _baseMaterial;
    CombatState _target;
    IBehavior _currentBehavior; 
    public bool IsVisible { get { return _renderer.isVisible; } }
    public Vector3 Position { get { return transform.position;  } }

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
                        SetExternalAction(new BasicAction(ActionType.Activate, true));
                    }
                    break;
                case TriggerType.Deactivate:
                    if (_isActive)
                    {
                        SetExternalAction(new BasicAction(ActionType.Deactivate, true)); 
                    }
                    break;
                case TriggerType.Alert:
                    SawPlayer(_character.Combat);
                    break;
                case TriggerType.UnAlert:
                    if(_isActive && _isAlerted)
                    {
                        SetExternalAction(new BasicAction(ActionType.UnAlert, true)); 
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
            SetAction(new BasicAction(ActionType.Alert, true)); 
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
    void Alert(Action _action)
    {
        _isAlerted = true; 
    }
    void UnAlert(Action _action)
    {
        _isAlerted = false; 
        if(_currentBehavior != null)
        {
            SetAction(_currentBehavior.EndBehavior()); 
        }
    }
	void Activate(Action _action)
    {
        _renderer.material = _defaultMat;
		_baseMaterial = _renderer.material; 
        _isActive = true;
    }
    void Deactivate(Action _action)
    {
        _renderer.material = GameSettings.InactiveMaterial;
		_baseMaterial = _renderer.material; 
        _isActive = false;
    }



    protected override void UseAction(Action _action, float _time)
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

    protected override void ReverseAction(Action _action, float _time)
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
		_baseMaterial = _renderer.material;
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
		GameManager.RegisterWibblyWobbly(this);
        SetAction(new BasicAction(ActionType.Null));
        if (_isActive)
        {
            Activate(new BasicAction(ActionType.Null));
        }else
        {
            Deactivate(new BasicAction(ActionType.Null)); 
        }
    }

    public void RewindActivation(Action _action)
    {
        throw new NotImplementedException();
    }

	public List<Action> Activate(Character _character, Vector3 _dir)
    {
        throw new NotImplementedException();
    }
}
