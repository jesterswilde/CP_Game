using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic; 

public class CombatState : MonoBehaviour {

    [SerializeField]
    float _health = 20;
    [SerializeField]
    float _currentHealth;
    [SerializeField]
    float _dodge = 5; 
    public float Dodge { get { return _dodge; } }
    [SerializeField]
    float _damageReduction;
    [SerializeField]
    float _accuracy;
    Inventory _inventory;
    Vitals _vitals = new Vitals();
    [SerializeField]
    Transform[] _vitalPoints;
    bool _isDead = false;
    Renderer _renderer;
    int _rollI; 

    bool _pullingTrigger; 
    public bool PullingTrigger { get { return _pullingTrigger; } }

    public IAction UseAction(IAction _action)
    {
        switch (_action.Type)
        {
            case ActionType.Fire:
                FireWeapon(_action);
                break;
            case ActionType.PullTrigger:
                _pullingTrigger = true;
                break; 
            case ActionType.ReleaseTrigger:
                _pullingTrigger = false;
                break;
            case ActionType.ShotBy:
                return ShotBy(_action.Weapon);
            case ActionType.TakeDamage:
                return TakeDamage(_action.Value);
            case ActionType.Die:
                Die();
                break; 
        }
        return null; 
    }
    public void ReverseAction(IAction _action)
    {
        switch (_action.Type)
        {
            case ActionType.Fire:
                ReverseFire(_action);
                break;
            case ActionType.PullTrigger:
                _pullingTrigger = false;
                break;
            case ActionType.ReleaseTrigger:
                _pullingTrigger = true;
                break;
            case ActionType.TakeDamage:
                ReverseDamage(_action.Value);
                break; 
            case ActionType.Die:
                ReverseDying();
                break;
        }
    }

    public List<IAction> ActionsToReset(List<IAction> _actions)
    {
        if (PullingTrigger)
        {
            _actions.Add(new Action(ActionType.ReleaseTrigger));
        }
        return _actions; 
    }
    public List<IAction> SetStateToKeyboard(List<IAction> _actions)
    {
        bool[] _keyboard = PlayerInput.GetAbsKeyboardState();
        if (_keyboard[4] && !_pullingTrigger)
        {
            _actions.Add(new Action(ActionType.PullTrigger));
        }
        if (!_keyboard[4] && _pullingTrigger)
        {
            _actions.Add(new Action(ActionType.ReleaseTrigger));
        }
        return _actions;
    }

    public TargetedAction PullTrigger (float _time, ITargetable _target)
    {
        if (_pullingTrigger)
        {
            if (_inventory.SelectedWeapon.PullTrigger(_time))
            {
                return new TargetedAction(ActionType.Fire, _target);
            }
        }
        return null;
    }
    public void FireWeapon(IAction _action)
    {
        ITargetable _target = _action.Target;
        if (_target != null && _target.isAttackable)
        {
            CombatState _combatent = _target.Combat;
            float _coverAmount;
            if (_combatent.CoverAmount(transform.position, out _coverAmount))
            {
                float _weaponAccuracy = _inventory.SelectedWeapon.Accuracy - _inventory.SelectedWeapon.AccuracyLoss(Vector3.Distance(transform.position, _target.Position));
                float _dc = Math.Max(5f, _accuracy + _weaponAccuracy - _coverAmount - _combatent.Dodge);
                if (SRand.Roll(ref _rollI) <= _dc)
                {
                    Debug.Log(_dc + " | Hit");
                    _target.SetAction(new WeaponAction(ActionType.ShotBy, _inventory.SelectedWeapon, true), true); 
                }
                else
                {
                    Debug.Log(_dc + " | MIssed");
                }
            }
            else
            {
                Debug.Log("object had cover");
            }
        }
    }
    public void ReverseFire(IAction _action)
    {
        SRand.ReverseRoll(ref _rollI);
        _inventory.SelectedWeapon.ReverseFireWeapon();
    }
    public bool CanSee(Vector3 gunPos)
    {
        return _vitals.CanSee(gunPos); 
    }
    public bool CoverAmount(Vector3 gunPos, out float accuracyLoss)
    {
        return _vitals.AmountOfBodyExposed(gunPos, out accuracyLoss);
    }

    Action TakeDamage(float _damage)
    {
        Debug.Log("Alas, I am shot");
        if (!_isDead)
        {
            _currentHealth -= _damage;
            if (_currentHealth < 0 && !_isDead)
            {
                return new Action(ActionType.Die, true);
            }
        }
        return null;
    }
    void ReverseDamage(float _damage)
    {
        Debug.Log("Nu-uh, youdidn't shoot me");
        _currentHealth += _damage;
    }
    public void Die()
    {
        Debug.Log("Hrk, I am dead");
        _isDead = true;
        _renderer.enabled = false;
    }
    public void ReverseDying()
    {
        Debug.Log("I am lazarus");
        _isDead = false;
        _renderer.enabled = true;
    }

    public ValueAction ShotBy(Weapon _bullet)
    {
        if (!_isDead)
        {
            Debug.Log("Got shot"); 
            return new ValueAction(ActionType.TakeDamage, _bullet.Damage - Math.Max(0, _damageReduction - _bullet.AP), true);
        }
        return null; 
    }



    void Awake()
    {
        _currentHealth = _health;
        _inventory = GetComponent<Inventory>();
        _renderer = GetComponent<Renderer>(); 
        if(_vitalPoints.Length > 0)
        {
            _vitals.SetVitals(_vitalPoints);
        }else
        {
            _vitals.SetVitals(transform); 
        }
    }

	// Use this for initialization
	void Start () {
        _rollI = SRand.GetStartingIndex(); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
