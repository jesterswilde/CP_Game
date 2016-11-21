using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Character : WibblyWobbly {

    public void PrintHistory()
    {
        HistoryNode _node;
        _node = _history.Tail; 
        while (true)
        {
            Debug.Log(_node.Action.Type + " | " + _node.Action.Time); 
            if(_node.Next != null)
            {
                _node = _node.Next;
            }else
            {
                break; 
            }
        }
    }

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
    protected override void UseAction(IAction _action, float _time)
    {
        _state.UseAction(_action);
    }
    protected override void ReverseAction(IAction _action, float _time)
    {
        _state.ReverseAction(_action);
    }
    public override void SetAction(IAction _action)
    {
        if (GameManager.CanAcceptPlayerInput)
        {
            base.SetAction(_action);
        }
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
    public void SwitchFromCharacter()
    {
        _history.AddToHead(new Action(ActionType.Clear, _history.HeadAction.Time)); 
    }
    public float GetHeadTimestamp()
    {
        if(_history.HeadAction == null)
        {
            return 0; 
        }
        return _history.HeadAction.Time; 
    }
    public void DeleteFuture()
    {
        _history.ClearFromPointer();
        List<IAction> _actions = _state.ActionsToReset(); 
        for(int i = 0; i < _actions.Count; i++)
        {
            _history.AddToHead(_actions[i]); 
        }
    }

    // Use this for initialization
    void Start () {
        SetAction(new Action(ActionType.Clear)); 
	}
    void Awake()
    {
        GameManager.RegisterCharacter(this);
        RegisterWibblyWobbly(); 
        _cam = gameObject.GetComponent<CharacterCam>();
        _camTrans = _cam.CameraSpot; 
    }
	
	// Update is called once per frame
	void Update () {
        _count = _history.Count; 
    }

   

}