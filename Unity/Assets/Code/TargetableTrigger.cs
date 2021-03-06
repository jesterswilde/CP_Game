﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetableTrigger : MonoBehaviour, ITargetable
{

    public float MinDistanceToActivate { get { return _minDistanceToActivate;  } }
    [SerializeField]
    List<AlertList> _alertList = new List<AlertList>(); 
    [SerializeField]
    int _index = 0; 
    [SerializeField]
    float _minDistanceToActivate = 3f;
    [SerializeField]
    bool _loop = false; 

	Material _baseMaterial;
    Renderer _renderer;
    RequiredItems _requiredItems;
    public bool IsVisible { get { return _renderer.isVisible; } }

    public Vector3 Position { get { return transform.position; } }

    public bool isActivatable { get { return true;} }
    public bool isAttackable { get { return false;} }
    public CombatState Combat { get { return null; } }
    public GameObject Go { get { return gameObject; } }

	public List<Action> Activate(Character _character, Vector3 _dir)
    {
        if(_alertList.Count == 0)
        {
            return null; 
        }
        if(_requiredItems == null || _requiredItems.HasRequiredItems(_character))
        {
            if(_index < _alertList.Count)
            {
                AlertList _alerts = _alertList[_index]; 
                foreach(AlertActions _alert in _alerts.alerts)
                {
                    _alert.Interactable.ExternalTrigger(_alert.EnterTask, _alert.Enter, _character);
                }
                Debug.Log("Triggered: " + _index); 
                _index++;
            }else
            {
                if (_loop)
                {
                    _index = 0;
					Activate(_character, Vector3.zero);
                }else
                {
                    _index++; 
                }
            }
            if(_requiredItems != null)
            {
                return _requiredItems.UseItems(); 
            }
        }
        return null;
    }
    public void RewindActivation(Action _action)
    {
        _index--;
        if (_loop)
        {
            if(_index < 0)
            {
                _index = _alertList.Count -1; 
            }
        }
    }
	public void Targeted(float _dist)
    {
		if (_dist < _minDistanceToActivate) {
			_renderer.material = ColorManager.InteractableTargetMaterial; 
		} else {
			_renderer.material = _baseMaterial; 
		}
		_renderer.material.SetFloat ("_OutlineWidth", ColorManager.OutlineWidth); 
    }
    public void UnTargeted()
    {
		_renderer.material = _baseMaterial; 
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }

    void Awake()
    {
        if(_alertList.Count == 0)
        {
            throw new System.Exception(gameObject.name + " has no interactable object"); 
        }
        _requiredItems = GetComponent<RequiredItems>(); 
        _renderer = GetComponent<Renderer>();
		_baseMaterial = _renderer.material; 
    }
    void Start()
    {
        GameManager.RegisterTargetable(this); 
    }

    public void SetAction(Action _action)
    {
        throw new NotImplementedException();
    }

    public void SetAction(Action _action, bool _evaluatSource)
    {
        throw new NotImplementedException();
    }
}
