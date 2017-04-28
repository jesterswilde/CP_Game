using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 
using System.Linq;

public class Item : MonoBehaviour, ITargetable {
    [SerializeField]
    string _itemName = "McGuffin";
    public string ItemName { get { return _itemName;  } }
    [SerializeField]
    float _amount = 1;
    public float Amount { get { return _amount;  } }
    [SerializeField]
    bool _isPhysical = false;
    public bool IsPhysical { get { return _isPhysical; } }
    [SerializeField]
    bool _isGlobal = false; 
    public bool IsGlobal { get { return _isGlobal; } }
    [SerializeField]
    float _minDistanceToActivate = 2f;

    bool _isHeld = false;
    Renderer _renderer;
    Collider _collider;
    CombatState _combat;
	Material _baseMaterial;
	List<IRequire> _activationRequirements = new List<IRequire> (); 


    public static bool HasItem(string _name)
    {
        return HasItem(_name, 1); 
    }
    public static bool HasItem(string _name, float _amountNeeded)
    {
        float test = -1;
        _globalItems.TryGetValue(_name, out test); 
        float _amountHas;
        return _globalItems.TryGetValue(_name, out _amountHas) && (_amountHas >= _amountNeeded); 
    }
    public static void AcquireItem(string _name, float _amount)
    {
        if (_globalItems.ContainsKey(_name))
        {
            _globalItems[_name] += _amount;
        }else
        {
            _globalItems[_name] = _amount; 
        }
    }
    public static bool RemoveItem(string _name, float _amount)
    {
        if (!_globalItems.ContainsKey(_name))
        {
            return false; 
        }
        Debug.Log(_globalItems[_name] + " | " + _amount);
        _globalItems[_name] = Math.Max(0, _globalItems[_name] - _amount); 
        return true; 
    }

    public bool IsVisible
    { get { return _renderer.isVisible; }}

    public Vector3 Position { get { return transform.position; } }

    public bool isActivatable { get { return true; } }

    public bool isAttackable { get { return _combat != null; } }

    public CombatState Combat { get { return _combat; } }

    public GameObject Go { get { return gameObject;  } }
    public float MinDistanceToActivate { get { return _minDistanceToActivate; } }
    static Dictionary<string, float> _globalItems = new Dictionary<string, float>(); 
    public void PickedUp()
    {
        Debug.Log("picking up " + _itemName);
        if(!_isHeld)
        {
            _isHeld = true;
            GameManager.UnRegisterTargetable(this); 
            if (_isPhysical)
            {
                _collider.enabled = false; 
                _renderer.enabled = false; 
            }
        }
    }
    public void PutDown()
    {
        Debug.Log("Putting down " + _itemName); 
        if (_isHeld)
        {
            _isHeld = false;
            GameManager.RegisterTargetable(this);
            if (_isPhysical)
            {
                _collider.enabled = true;
                _renderer.enabled = true;
				_renderer.material = _baseMaterial;  
            }
        }
    }
    public void PutDown(Vector3 _pos, Quaternion _rot)
    {
        PutDown();
        transform.position = _pos;
        transform.rotation = _rot; 
    }
    public void AddTo(float _amountToAdd)
    {
        _amount += _amountToAdd; 
    }

	public void Targeted(float _dist)
    {
		if (!_isHeld) {
			if (_dist < _minDistanceToActivate) {
				_renderer.material = ColorManager.ItemMaterial; 
			} else {
				_renderer.material = _baseMaterial; 
			}
			_renderer.material.SetFloat ("_OutlineWidth", ColorManager.OutlineWidth); 
		} else {
			UnTargeted (); 
		}
    }

    public void UnTargeted()
    {
		_renderer.material = _baseMaterial; 
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }
	bool MeetsRequirements(Character _character){
		return _activationRequirements.All ((_req) => _req.AllowActivation (_character)); 
	}
	public List<Action> Activate(Character _character, Vector3 _dir)
    {
		if(MeetsRequirements(_character) && !_isHeld)
        {
			List<Action> _actions = new List<Action>();
			foreach (IRequire _req in _activationRequirements) {
				_req.ActivationConsequences ().AddTo (_actions); 
			}
            _actions.Add(new ItemAction(ActionType.PickUp, new InvenItem(this), true));
           return _actions;
        }
        return null; 
    }

    public void RewindActivation(Action _action)
    {
        if (_isHeld)
        {
            PutDown(); 
        }
    }

    public void SetAction(Action _action)
    {
        throw new NotImplementedException();
    }

    public void SetAction(Action _action, bool _evaluatSource)
    {
        throw new NotImplementedException();
    }
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>(); 
		_baseMaterial = _renderer.material;
		_activationRequirements = GetComponents<Component>().Where((Component _comp)=> _comp is IRequire).Select((_comp)=> _comp as IRequire).ToList(); 

    }
    void Start()
    {
        GameManager.RegisterTargetable(this);
    }
}

