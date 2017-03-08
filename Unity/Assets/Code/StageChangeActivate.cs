﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChangeActivate : MonoBehaviour, ITargetable {

    [SerializeField]
    int _stage; 
    [SerializeField]
    float _minDistance = 5f; 
    Renderer _renderer;
	Material _baseMaterial;
    RequiredItems _requiredItems; 

    public GameObject Go { get { return gameObject; } }
    public bool IsVisible { get { return _renderer.isVisible; } }
    public CombatState Combat { get { return null; } }
    public bool isActivatable { get { return true; } }
    public bool isAttackable { get { return false; } }

    public float MinDistanceToActivate { get { return _minDistance; } }

    public Vector3 Position { get { return transform.position; } }

    bool _canActivate = true; 
    public bool CanActivate { get { return _canActivate; } set { _canActivate = value; } }
	public List<Action> Activate(Character _character, Vector3 _dir)
    {
        if (_canActivate)
        {
            List<Action> _actions = new List<Action> { new BasicAction(ActionType.Null) }; //adding null to not rewind. 
            if(_requiredItems == null || _requiredItems.HasRequiredItems(_character))
            {
                StageManager.ChangeStage(_stage); 
                if(_requiredItems != null && _requiredItems.UseItems() != null)
                {
                    _actions.AddRange(_requiredItems.UseItems());
                    return _actions;
                }
            }
            return _actions;
        }
        return null; 
    }

    public void RewindActivation(Action _action)
    {
        
    }

    public void SetAction(Action _action)
    {
        throw new NotImplementedException();
    }

    public void SetAction(Action _action, bool _evaluatSource)
    {
        throw new NotImplementedException();
    }

	public void Targeted(float _dist)
    {
		if (_dist < _minDistance) {
			_renderer.material = ColorManager.StageTargetMaterial; 
		} else {
			_renderer.material = _baseMaterial; 
		}
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }

    public void UnTargeted()
    {
        _renderer.material = _baseMaterial; 
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>(); 
        _baseMaterial = _renderer.material;
        _requiredItems = GetComponent<RequiredItems>();
        GameManager.RegisterTargetable(this); 
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
