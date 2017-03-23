using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : WibblyWobbly {

	[SerializeField]
	Object _bullet; 
	[SerializeField]
	float _fireRate = 0.5f;
	float _timeSinceLastShot = 0; 
	[SerializeField]
	float _reloadTime = 3f;
	float _timeSinceReloadStart = 0; 
	[SerializeField]
	float _clipSize = 10f; 
	float _bulletsFired = 0; 
	[SerializeField] 
	float _checkFrequency = 3f; 
	float _timeSinceLastCheck = 0;
	bool _canFire = true; 

	CombatState _target; 

	void FindTarget(){
		List<Character> _characters = GameManager.Characters;
		float _dist = Mathf.Infinity; 
		Character _newTarget = null; 
		foreach (Character _char in _characters) {
			float _charDist = Vector3.Distance (transform.position, _char.transform.position); 
			if (_charDist < _dist) {
				_dist = _charDist; 
				_newTarget = _char; 
			}
		}
		SetExternalAction (new CombatAction (ActionType.Untarget, _target)); 
		_target = _newTarget.Combat; 
	}
	void CreateBullet(){
		Instantiate (_bullet, transform.position, transform.rotation); 
	}

	#region implemented abstract members of WibblyWobbly
	protected override void UseAction (Action _action, float _time)
	{
		
	}
	protected override void ReverseAction (Action _action, float _time)
	{
		
	}
	protected override void Act (float _deltaTime)
	{
		Debug.Log ("acting"); 
		_timeSinceLastCheck += _deltaTime; 
		if (_timeSinceLastCheck > _checkFrequency) {
			_timeSinceLastCheck -= _checkFrequency; 
			FindTarget ();
		}
		if (_canFire) {
			_timeSinceLastShot += _deltaTime; 
			if (_timeSinceLastShot > _fireRate) {
				CreateBullet (); 
				_timeSinceLastShot -= _fireRate; 
				_bulletsFired++; 
				if (_bulletsFired >= _clipSize) {
					_canFire = false; 
				}
			}
		} else {
			_timeSinceReloadStart += _deltaTime;
			if (_timeSinceReloadStart > _reloadTime) {
				_canFire = true; 
				_bulletsFired = 0; 
			}
		}
	}
	protected override void ActReverse (float _deltaTIme)
	{
		_timeSinceLastCheck -= _deltaTIme; 
		if (_timeSinceLastCheck < 0) {
			_timeSinceLastCheck += _checkFrequency; 
		}
		if (_canFire) {
			_timeSinceLastShot -= _deltaTIme; 
			if (_timeSinceLastShot < 0) {
				_bulletsFired--; 
				if (_bulletsFired < 0) {
					_canFire = false; 
					_timeSinceReloadStart += _reloadTime; 
					_bulletsFired = _clipSize; 
				}
			}
		} else {
			_timeSinceReloadStart -= _deltaTIme; 
			if (_timeSinceReloadStart < 0) {
				_canFire = true; 
			}
		}
	}
	#endregion

	// Use this for initialization
	void Start () {
		RegisterWibblyWobbly (); 	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
