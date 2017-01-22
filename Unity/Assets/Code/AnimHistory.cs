using UnityEngine;
using System.Collections;

public class AnimHistory : MonoBehaviour {

    History _history = new History();

    [SerializeField]
    AnimationClip _shoot;
    Animator _anim; 

    void Start()
    {
        _anim = GetComponent<Animator>();
     }

    public void ChangeSpeed(float _speed)
    {
        if(_anim != null)
        {
            _anim.speed = _speed; 
        }
    }
}
