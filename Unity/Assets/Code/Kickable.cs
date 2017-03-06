using System.Collections;
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
	float _collRadius = 1; 
	Renderer _renderer; 
	Material _baseMaterial; 

	Vector3 _dir = Vector3.zero;
	float _speed = 0; 
	float _minSpeedThreshold = 0.1f; 


	public override void Play (float _time)
	{
		//Debug.Log ("Play"); 
		if (_speed != 0) {
			Ray _ray = new Ray (transform.position, _dir); 
			RaycastHit _hit; 
			float _deltaTime = _time - _prevTime; 
			float _travelDist = _speed * _deltaTime; 
			if (Physics.Raycast (_ray, out _hit, _collRadius + _travelDist, _collMask)) {
				Vector3 _travelVec = _dir * _speed * _deltaTime;
				float _toCollision = _hit.distance - _collRadius;
				float _collTime = (_toCollision / _travelDist) * _deltaTime + _time;
				Vector3 _newDir = Vector3.Reflect (_travelVec, _hit.normal).normalized; 
				float _newSpeed = _speed - ModSpeed (_collTime - _time); 
				SetExternalAction (new VectorAction (ActionType.Activate, _newSpeed * _newDir, _collTime, true)); 
			}
		}
		base.Play (_time);
	}
	float ModSpeed(float _deltaTime){
		if (_speed < _minSpeedThreshold) {
			SetExternalAction (new ValueAction (ActionType.ThresholdReached, _speed)); 
			return _speed;  
		} else {
			return _speed * _drag * _deltaTime; 
		}
	}

	protected override void Act (float _deltaTime)
	{
		transform.position += _speed * _dir * _deltaTime;
		_speed -= ModSpeed (_deltaTime); 
	}
	
	protected override void ActReverse (float _deltaTIme)
	{
		transform.position -= _speed * _dir * _deltaTIme; 
		_speed += ModSpeed (_deltaTIme); 
	}

	protected override void UseAction (Action _action, float _time)
	{
		//Debug.Log (_action.Type); 
		switch (_action.Type) {
		case(ActionType.Activate):
			SetExternalAction(new VectorAction(ActionType.Deactivate, _dir * _speed, _time, true); 
			_speed = _action.Vector.magnitude;
			_dir = _action.Vector.normalized;
			Debug.Log (_speed + " | " + _dir); 
			break;
		case(ActionType.ThresholdReached):
			_speed = 0; 
			break;
		}
	}
	protected override void ReverseAction (Action _action, float _time)
	{
		switch (_action.Type) {
		case(ActionType.Deactivate):
			_speed = _action.Vector.magnitude; 
			_dir = _action.Vector.normalized; 
			break; 
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
	public List<Action> Activate (Character _character)
	{
		Debug.Log ("kicking"); 
		Vector3 _kick = transform.position - _character.transform.position; 
		_kick = new Vector3 (_dir.x, 0, _dir.y).normalized * _force; 
		SetAction(new VectorAction (ActionType.Activate, _kick, true));  
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
		_history.AddToHead(new BasicAction(ActionType.Null));
		//RegisterWibblyWobbly();
	}
}
