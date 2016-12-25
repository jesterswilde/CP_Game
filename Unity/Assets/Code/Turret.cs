using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;

public class Turret : Interactable, ITargetable {

    [SerializeField]
    bool _isActive = true;
    bool _isAlerted = false; 

    Renderer _renderer;
    Color _baseColor;
    Character _target; 
    public bool IsVisible { get { return _renderer.isVisible; } }
    public Vector3 Position { get { return transform.position;  } }


    public bool isActivatable { get { return false; } }
    public bool isAttackable { get { return (_combat != null) ? true : false; } }
    public CombatState Combat { get { return _combat; } }
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
    public void Die()
    {
    }
    public void ReverseDying()
    {
    }

    protected override void UseAction(IAction _action, float _time)
    {
        Debug.Log(_action.Type); 
        SetAction(_combat.UseAction(_action), true); 
        base.UseAction(_action, _time); 
    }

    protected override void ReverseAction(IAction _action, float _time)
    {
        _combat.ReverseAction(_action); 
        base.ReverseAction(_action, _time); 
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _combat = GetComponent<CombatState>(); 
        _baseColor =  _renderer.material.color;
    }
    void Start()
    {
        GameManager.RegisterTargetable(this); 
        GameManager.RegisterWibblyWobbly(this);
        SetAction(new Action(ActionType.Null)); 
    }

    public void Activate()
    {
        throw new Exception("cannot be activated");
    }

    public void RewindActivation()
    {
        throw new Exception("cannot be activated"); 
    }


    protected override void Act(float _deltaTime)
    {
        base.Act(_deltaTime); 
    }

    protected override void ActReverse(float _deltaTIme)
    {
        base.Act(_deltaTIme); 
    }
}
