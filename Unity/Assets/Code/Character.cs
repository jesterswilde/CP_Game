using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character : MonoBehaviour {

    [SerializeField]
    float speed = 5;
    CharacterCam _cam;
    public CharacterCam Cam { get { return _cam; } }
    Transform _camTrans; 
    public Transform CamPoint { get { return _cam.CameraSpot; } }
    [SerializeField]
    int _count;
    float _prevTime; 

    CharacterState _state = new CharacterState();
    CharacterState _baseState = new CharacterState(); 
    History _history = new History(); 

    [SerializeField]
    bool _playerControlled = false;

    public void SetAction(IAction _action)
    {
        Debug.Log(_action.Type); 
        _history.AddToHead(_action);
    }

    public void RewindToTime()
    {
        _state.SetToBase(_baseState); 
        _history.SetCurrentTime(GameManager.GameTime);
        if(_history.PointerAction != null)
        {
            Act(GameManager.GameTime - _history.PointerAction.Time);
        }else
        {
            Act(); 
        }
    }
    public void Play()
    {
        while (true)
        {
            HistoryNode _node = _history.PlayTime(GameManager.GameTime);
            if(_node == null)
            {
                Act(GameManager.GameTime - _prevTime);
                _prevTime = GameManager.GameTime; 
                break; 
            }
            Act(_node.Action.Time - _prevTime);
            _state.UseAction(_node.Action); 
            _prevTime = _node.Action.Time; 
        }
    }
    public void Rewind()
    {
        while (true)
        {
            HistoryNode _node = _history.RewindTime(GameManager.GameTime);
            if (_node == null)
            {
                ActReverse(_prevTime - GameManager.GameTime);
                _prevTime = GameManager.GameTime;
                break;
            }
            ActReverse(_prevTime - _node.Action.Time);
            _state.ReverseAction(_node.Action);
            _prevTime =_node.Action.Time;
        }
    }
    public void Act()
    {
        Act(Time.deltaTime); 
    }
    public void Act(float _deltaTime)
    {
        transform.forward = _cam.CameraForward(_state.XRot); 
        Vector3 _move = (_state.Forward - _state.Backward) * transform.forward + (_state.Right - _state.Left) * transform.right;
        transform.position += new Vector3(_move.x, 0, _move.z).normalized * speed * _deltaTime;
    }
    public void ActReverse()
    {
        ActReverse(Time.deltaTime); 
    }
    public void ActReverse(float _deltaTime)
    {
        transform.forward = _cam.CameraForward(_state.XRot);
        Vector3 _move = (_state.Forward - _state.Backward) * transform.forward + (_state.Right - _state.Left) * transform.right;
        transform.position += new Vector3(_move.x, 0, _move.z).normalized * speed * _deltaTime * -1;
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