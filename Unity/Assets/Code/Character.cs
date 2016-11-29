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
    float _maxTargetDistance = 0.1f; 
    InteractableTrigger _target;

    CharacterState _state = new CharacterState();

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
        switch (_action.Type)
        {
            case ActionType.Activate:
                _action.Target.Activate();
                return; 
        }
        _state.UseAction(_action);
    }
    protected override void ReverseAction(IAction _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.Activate:
                _action.Target.RewindActivation();
                return;
        }
        _state.ReverseAction(_action);
    }
    public override void SetAction(IAction _action)
    {
        if (GameManager.CanAcceptPlayerInput)
        {
            base.SetAction(_action);
        }
    }
    public TargetedAction CreateTargetAction()
    {
        if(_target != null)
        {
            return new TargetedAction(ActionType.Activate, _target); 
        }
        return null; 
    }
    public void Activate()
    {
        if(_target != null && (transform.position - _target.transform.position).magnitude < _target.MinDistanceToActivate)
        {
            Ray _ray = new Ray(transform.position, _target.transform.position - transform.position);
            RaycastHit _hit; 
            if(Physics.Raycast(_ray, out _hit, GameManager.CollMask))
            {
                if(System.Object.ReferenceEquals(_hit.collider.gameObject, _target.gameObject))
                {
                    _target.Activate(); 
                }
            }
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
        SetAction(_state.ActionsToReset()); 
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
    public bool IsPastMostRecentAction()
    {
        return _history.IsPointerAtHead(); 
    }
    public void DeleteFuture()
    {
        _history.ClearFromPointer();
        ClearState();
    }
    public void ClearState()
    {
        SetAction(_state.ActionsToReset());
    }
    public void FaceCamrea()
    {
        if (WillRotate())
        {
            SetAction(new ValueAction(ActionType.Rotation, RotationDifference()));
        }
    }
    public void SetStateToKeyboard()
    {
        SetAction(_state.SetStateToKeyboard());
    }

    // Use this for initialization
    void Start () {
        SetAction(new Action(ActionType.Null)); 
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
        InteractableTrigger _trigger = GameManager.ClosestToViewPort(_maxTargetDistance);
        if(_trigger == null && _target != null)
        {
            _target.UnTarget();
            _target = _trigger; 
        }
        if(!System.Object.ReferenceEquals(_trigger, _target) && _trigger != null)
        {
            if(_target != null)
            {
                _target.UnTarget();
            }
            _target = _trigger;
            _target.Target(); 
        }
    }

   

}