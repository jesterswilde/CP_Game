using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character : WibblyWobbly {

    [SerializeField]
    float speed = 5;
    CharacterCam _cam;
    public CharacterCam Cam { get { return _cam; } }
    Transform _camTrans; 
    public Transform CamPoint { get { return _cam.CameraSpot; } }
    [SerializeField]
    int _count;

    CharacterState _state = new CharacterState();
    CharacterState _baseState = new CharacterState(); 

    [SerializeField]
    bool _playerControlled = false;

    protected override void Act(float _deltaTime)
    {
        transform.forward = _cam.CameraForward(_state.XRot); 
        Vector3 _move = ((_state.Forward * _state.CanForward) - (_state.Backward * _state.CanBackward)) * transform.forward +
            ((_state.Right * _state.CanRight) - (_state.Left * _state.CanLeft)) * transform.right;
        transform.position += new Vector3(_move.x, 0, _move.z).normalized * speed * _deltaTime;
    }

    protected override void ActReverse(float _deltaTime)
    {
        transform.forward = _cam.CameraForward(_state.XRot);
        Vector3 _move = ((_state.Forward * _state.CanForward) - (_state.Backward * _state.CanBackward)) * transform.forward + 
            ((_state.Right * _state.CanRight) - (_state.Left * _state.CanLeft)) * transform.right;
        transform.position += new Vector3(_move.x, 0, _move.z).normalized * speed * _deltaTime * -1;
    }
    protected override void UseAction(IAction _action)
    {
        _state.UseAction(_action);
    }
    protected override void ReverseAction(IAction _action)
    {
        _state.ReverseAction(_action);
    }
    public bool WillRotate()
    {
        if(_playerControlled && _state.IsMoving())
        {
            return _state.XRot != _cam.XRot; 
        }
        return false; 
    }
    public float RotationDifference()
    {
        return _cam.XRot - _state.XRot;
    }

    public void SetAsActivePlayer()
    {
        _playerControlled = true;
        GetComponent<MeshRenderer>().material.color = Color.red; 
    }
    public void SetAsInactivePlayer()
    {
        _playerControlled = false;
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
    

    // Use this for initialization
    void Start () {
        SetAction(new Action(ActionType.Clear)); 
	}
    void Awake()
    {
        GameManager.RegisterCharacter(this);
        _cam = gameObject.GetComponent<CharacterCam>();
        _camTrans = _cam.CameraSpot; 
    }
	
	// Update is called once per frame
	void Update () {
        _count = _history.Count; 
    }

   

}