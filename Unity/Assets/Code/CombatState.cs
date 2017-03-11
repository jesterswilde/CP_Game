using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic;

public delegate void SetActionDelegate(Action _action, bool _determinOrigin);
public class CombatState : MonoBehaviour, ITargetable {

    [SerializeField]
    float _maximumHealth = 20;
    public float MaximumHealth { get { return _maximumHealth; } }
    [SerializeField]
    float _currentHealth;
    public float CurrentHealth { get { return _currentHealth; } }
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
	Material _baseMaterial; 
	int _rollI;
    [SerializeField]
    Transform _gunTrans;
    public Transform GunTrans { get { return _gunTrans != null ? _gunTrans : transform; } set { _gunTrans = value; } }
    bool _pullingTrigger; 
    public bool PullingTrigger { get { return _pullingTrigger; } }
    SetActionDelegate _actionDelegate;
    [SerializeField]
    string _muzzleFlash;
	bool _isPlayer;



	public void Targeted(float _dist)
	{
		_renderer.material = ColorManager.EnemyTargetMaterial; 
	}

	public void UnTargeted()
	{
		_renderer.material = _baseMaterial; 
	}

	public List<Action> Activate (Character _character, Vector3 _dir)
	{
		return null; 
	}


	public void RewindActivation (Action _action)
	{
	}


	public void SetAction (Action _action)
	{
		throw new NotImplementedException ();
	}


	public bool IsVisible {
		get {
			throw new NotImplementedException ();
		}
	}


	public Vector3 Position { get { return transform.position; } }


	public bool isActivatable { get { return false; } }


	public bool isAttackable { get { return true; } }


	public CombatState Combat { get { return this; } }


	public GameObject Go { get { return gameObject; } }


	public float MinDistanceToActivate { get { return Mathf.Infinity; } }

 
    

    public Action UseAction(Action _action)
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
    public void ReverseAction(Action _action)
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
    public void SetAction(Action _action, bool _determinOrigin)
    {
        _actionDelegate(_action, _determinOrigin);
    }
    public void SetCallbacks(SetActionDelegate _delegate)
    {
        _actionDelegate = _delegate;
    }

    public List<Action> ActionsToReset(List<Action> _actions)
    {
        if (PullingTrigger)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseTrigger));
        }
        return _actions; 
    }
    public List<Action> SetStateToKeyboard(List<Action> _actions)
    {
        bool[] _keyboard = PlayerInput.GetAbsKeyboardState();
        if (_keyboard[4] && !_pullingTrigger)
        {
            _actions.Add(new BasicAction(ActionType.PullTrigger));
        }
        if (!_keyboard[4] && _pullingTrigger)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseTrigger));
        }
        return _actions;
    }

    public CombatAction PullTrigger (float _time, CombatState _target)
    {
        if (_pullingTrigger)
        {
            if (_inventory.SelectedWeapon.PullTrigger(_time))
            {
                return new CombatAction(ActionType.Fire, _target);
            }
        }
        return null;
    }
    public void FireWeapon(Action _action)
    {
        new Effect(_muzzleFlash, GunTrans.position, GunTrans.rotation);
        CombatState _target = _action.Combat;
        if (_target != null)
        {
            float _coverAmount;
            if (_target.CoverAmount(GunTrans.position, out _coverAmount))
            {
                float _weaponAccuracy = _inventory.SelectedWeapon.Accuracy - _inventory.SelectedWeapon.AccuracyLoss(Vector3.Distance(transform.position, _target.transform.position));
                float _dc = Math.Max(5f, _accuracy + _weaponAccuracy - _coverAmount - _target.Dodge);
                if (SRand.Roll(ref _rollI) <= _dc)
                {
                    _target.SetAction(new WeaponAction(ActionType.ShotBy, _inventory.SelectedWeapon, true), true); 
                }
                else
                {
                }
            }
            else
            {
            }
        }
    }
    public void ReverseFire(Action _action)
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

    BasicAction TakeDamage(float _damage)
    {
        if (!_isDead)
        {
            _currentHealth -= _damage;
            if (_currentHealth < 0 && !_isDead)
            {
                return new BasicAction(ActionType.Die, true);
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
            return new ValueAction(ActionType.TakeDamage, _bullet.Damage - Math.Max(0, _damageReduction - _bullet.AP), true);
        }
        return null; 
    }



    void Awake()
    {
		_isPlayer = (GetComponent<Character> () != null) ? true : false; 
        _currentHealth = _maximumHealth;
        _inventory = GetComponent<Inventory>();
        _renderer = GetComponent<Renderer>(); 
		_baseMaterial = _renderer.material; 
        if(_vitalPoints.Length > 0)
        {
            _vitals.SetVitals(_vitalPoints);
        }else
        {
            _vitals.SetVitals(transform); 
        }
    }
		
	void Start () {
        _rollI = SRand.GetStartingIndex(); 
		GameManager.RegisterTargetable (this); 
	}

	void Update () {
	
	}
}
