using UnityEngine;
using System.Collections;
using System;

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
    float _minDistanceToActivate = 2f;

    bool _isHeld = false;
    Renderer _renderer;
    Collider _collider;
    CombatState _combat;
    Color _baseColor; 

    public bool IsVisible
    { get { return _renderer.isVisible; }}

    public Vector3 Position { get { return transform.position; } }

    public bool isActivatable { get { return true; } }

    public bool isAttackable { get { return _combat != null; } }

    public CombatState Combat { get { return _combat; } }

    public GameObject Go { get { return gameObject;  } }
    public float MinDistanceToActivate { get { return _minDistanceToActivate; } }

    public void PickedUp()
    {
        Debug.Log("picking up " + _itemName);  ; 
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
                _renderer.material.color = _baseColor;  
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

    public void Targeted()
    {
        _renderer.material.color = Color.yellow; 
    }

    public void UnTargeted()
    {
        _renderer.material.color = _baseColor; 
    }

    public IAction Activate(Character _character)
    {
        if (!_isHeld)
        {
            Debug.Log("activating");
           return new ItemAction(ActionType.PickUp, new InvenItem(this), true);
        }
        return null; 
    }

    public void RewindActivation(IAction _action)
    {
        if (_isHeld)
        {
            PutDown(); 
        }
    }

    public void SetAction(IAction _action)
    {
        throw new NotImplementedException();
    }

    public void SetAction(IAction _action, bool _evaluatSource)
    {
        throw new NotImplementedException();
    }
    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>(); 
        _baseColor = _renderer.material.color;
    }
    void Start()
    {
        GameManager.RegisterTargetable(this); 
    }
}

