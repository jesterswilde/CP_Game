using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim : MonoBehaviour, ITime {

    Animator _anim;
    CharacterState _state;
    int _currentAnimHash;
    float _currentAnimTime;
	bool _isPlaying = false; 
	Vector3 _lastPos; 

	public List<Action> PlayVisuals (float _time)
	{
		if (!_isPlaying) {
			_anim.SetBool ("IsPlaying", true); 
			_isPlaying = true;
		}
		_anim.SetFloat("Speed", GameManager.GameSpeed);
		List<Action> _actions = new List<Action> ();
		if (GameManager.GameSpeed > 0) {
			//CheckSpeed (); 
			int _nextHash = _anim.GetCurrentAnimatorStateInfo (0).shortNameHash;
			if (_currentAnimHash != _nextHash) {
				_actions.Add (new AnimAction (ActionType.AnimEnded, _currentAnimHash, _currentAnimTime, true));
				_currentAnimHash = _nextHash; 
			}
			_currentAnimTime = _anim.GetCurrentAnimatorStateInfo (0).normalizedTime; 
		}
		return _actions; 
	}

	public void RewindVisuals (float _time)
	{
		if (_isPlaying) {
			Debug.Log ("rewindig now."); 
			_anim.SetBool ("IsPlaying", false); 
			_isPlaying = false; 
		}
		if (GameManager.GameTime == 0) {
			_anim.Play ("Idle");
			_anim.SetFloat("Speed", 0); 
		}else{
			_anim.SetFloat("Speed", GameManager.GameSpeed);
		}
	}

 

    public List<Action> Act(float deltaTime)
    {	
		return null; 
    }
    public void RewindAct(float deltaTime)
    {
    }

	public List<Action> UseAction(Action _action, float _time){
		switch (_action.Type) {
		case ActionType.AnimRunning:
			_anim.SetBool ("IsRunning", true); 
			break;
		case ActionType.AnimStopRunning:
			_anim.SetBool ("IsRunning", false); 
			break; 
		}
		return null; 
	}
	
	public void ReverseAction (Action _action, float _time)
	{
		switch (_action.Type) {
		case ActionType.AnimEnded:
			if (_action.IValue == 0) {
				_anim.Play ("Idle"); 
			}
			_anim.CrossFade (_action.IValue, 0.25f, 0, _action.Value); 
			break; 
		}
	}
    void Awake()
    {
		_lastPos = transform.position; 
        _anim = GetComponent<Animator>(); 
        if(_anim == null)
        {
            _anim = GetComponentInChildren<Animator>(); 
        }
		GetComponent<WibblyWobbly> ().RegisterITime (this); 
    }
	void Start(){
		_anim.Play ("Idle"); 
		_currentAnimHash = _anim.GetCurrentAnimatorStateInfo (0).shortNameHash; 
	}
}
