using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnim : MonoBehaviour {

    Animator _anim;
    CharacterState _state;
    int _currentAnimHash;
    float _currentAnimTime;
    bool _isReplaying = false; 

    public List<Action> PlayAnim(float deltaTime)
    {
        if (_isReplaying)
        {
            _anim.StopPlayback();
            _isReplaying = false; 
        }
        List<Action> _actions = new List<Action>();
        int _nextHash = _anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
        if(_currentAnimHash != _nextHash)
        {
            _actions.Add(new AnimAction(ActionType.AnimEnded, _currentAnimHash, _currentAnimTime, true));
            _currentAnimHash = _nextHash; 
        }
        _currentAnimTime = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime; 
        return _actions; 
    }
    public void RewindAnim(float deltaTime)
    {
        if (!_isReplaying)
        {
            _anim.StartPlayback();
        }
        float _normalizedDelta = deltaTime / _anim.GetCurrentAnimatorClipInfo(0)[0].clip.length * _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float _normal = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime - _normalizedDelta;
    }
    void Awake()
    {
        _anim = GetComponent<Animator>(); 
        if(_anim == null)
        {
            _anim = GetComponentInChildren<Animator>(); 
        }
    }
}
