﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class Kickable : WibblyWobbly, ITargetable {

	[SerializeField]
	float _force = 10f; 
	[SerializeField]
	float _drag = 0.1f; 
	[SerializeField]
	LayerMask _collMask; 
	[SerializeField]
	float _minActivteDistance = 3;
	[SerializeField]
	float _collRadius = 1f; 
	Renderer _renderer; 
	Material _baseMaterial; 

	Vector3 _dir = Vector3.zero;
	float _speed = 0; 
	float _minSpeedThreshold = 0.5f; 
	bool _firstPlay = true; 
	bool _hasCast = false; 

	public override void Play (float _time)
	{
		if (_speed != 0) {
			_firstPlay = false; 
			Ray _ray = new Ray (transform.position, _dir); 
			RaycastHit _hit; 
			float _deltaTime = GameSettings.FixedTimestep;
			float _travelDist = _speed * _deltaTime; 
			if (Physics.Raycast (_ray, out _hit, _collRadius + _travelDist, _collMask)) {
				_hasCast = true; 
				Vector3 _travelVec = _dir * _speed * _deltaTime;
				float _toCollision = _hit.distance - _collRadius;
				float _collTime = (_toCollision / _travelDist) * _deltaTime + _time;
				Vector3 _newDir = Vector3.Reflect (_travelVec, _hit.normal).normalized;
				SetExternalAction(new VectorAction(ActionType.Deactivate, _dir * _speed, true));
				SetExternalAction(new DirTargetAction (ActionType.Activate, _speed * _newDir, transform.position , true)); 
			}
		}
		base.Play (_time);
	}
	int ModSpeed(float _deltaTime){
		int _intSpeed = (int)(_speed * 10000); 
	    return Mathf.FloorToInt(_intSpeed * _drag * _deltaTime); 
	}
        
	protected override void Act (float _deltaTime)
	{
		if (_speed != 0) {
            if(_speed < _minSpeedThreshold){
                SetExternalAction(new ValueAction(ActionType.ThresholdReached, _speed));
                _speed = 0;
                return; 
            }
			int _intSpeed = Mathf.FloorToInt(_speed * 10000); 
			_intSpeed -= ModSpeed (_deltaTime);	
			_speed = _intSpeed / 10000f; 
			transform.position += _speed * _dir * _deltaTime;
		}
	}
	
	protected override void ActReverse (float _deltaTime)
	{
		if(_speed != 0){
			int _intSpeed = Mathf.FloorToInt(_speed * 10000); 
			_intSpeed += ModSpeed (_deltaTime);
			_speed = _intSpeed / 10000f; 
			transform.position -= _speed * _dir * _deltaTime; 
		}
	}

	protected override void UseAction (Action _action, float _time)
	{
		Debug.Log ("Ball | " + _action.Type); 
		switch (_action.Type) {
		case(ActionType.Activate):
			_speed = _action.Vector.magnitude;
			_dir = _action.Vector.normalized;
			_hasCast = false;
			break;
		}
	}
	protected override void ReverseAction (Action _action, float _time)
	{
			switch (_action.Type) {
			case(ActionType.Deactivate):
				//Here my morals die. 
				transform.position = _action.SecondVec; 
				_speed = _action.Vector.magnitude; 
				_dir = _action.Vector.normalized;
				break; //my heart
		case(ActionType.ThresholdReached):
			_speed = _action.Value;
			break;
		}
	}

	public void Targeted (float dist)
	{
		if (dist < _minActivteDistance) {
			_renderer.material = ColorManager.InteractableTargetMaterial;
		} else {
			_renderer.material = _baseMaterial; 
		}
		_renderer.material.SetFloat ("_OutlineWidth", ColorManager.OutlineWidth); 
	}
	public void UnTargeted ()
	{
		_renderer.material = _baseMaterial; 
		_renderer.material.SetFloat ("_OutlineWidth", 0);
	}
	public List<Action> Activate (Character _character, Vector3 _newDir)
	{
		_newDir = new Vector3 (_newDir.x, 0, _newDir.z).normalized * _force; 
		SetAction(new VectorAction (ActionType.Activate, _newDir, true));  
		SetAction(new DirTargetAction(ActionType.Deactivate, _dir * _speed, transform.position, true)); 
		return null; 
	}
	public void RewindActivation (Action _action)
	{
		
	}
	public bool IsVisible { get { return false; } }
	public Vector3 Position { get { return transform.position; } }
	public bool isActivatable { get { return true; } }
	public bool isAttackable { get { return false; } }
	public GameObject Go { get { return gameObject; } }
	public float MinDistanceToActivate { get { return _minActivteDistance; } }

	void Awake(){
		_renderer = gameObject.GetComponent<Renderer> ();
		_baseMaterial = _renderer.material; 
	}
	void Start(){
		SetAction(new BasicAction(ActionType.Null));
		RegisterWibblyWobbly();
	}
}
