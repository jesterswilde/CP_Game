using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(CombatState))]
public class Character : WibblyWobbly {
    [SerializeField]
    float _health = 20;
    [SerializeField]
    float _currentHealth; 
    [SerializeField]
    float _accuracy = 80; 
    [SerializeField]
    float _damageReduction = 0;
    [SerializeField]
    float _dodge = 5; 
    [SerializeField]
    float speed = 5;
    bool _isDead = false; 
    CharacterCam _cam;
    public CharacterCam Cam { get { return _cam; } }
    Transform _camTrans; 
    public Transform CamPoint { get { return _cam.CameraSpot; } }
    [SerializeField]
    float _maxTargetDistance = 0.1f;
    [SerializeField]
    Transform[] _vitalsTrans;
    Vitals _vitals = new Vitals(); 
    ITargetable _currentTarget;
    int _rollI; 

    CharacterState _state = new CharacterState();
    Renderer _renderer; 

    [SerializeField]
    bool _playerControlled = false;
    Inventory _inventory;

    #region Time
    public override void Play(float _time)
    {
        SetAction(_combat.PullTrigger(_time, _currentTarget), true);
        base.Play(_time);
    }
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
        SetAction(_combat.UseAction(_action), true); 
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
        _combat.ReverseAction(_action);
        _state.ReverseAction(_action);
    }
    public override void SetAction(IAction _action)
    {
        if (GameManager.CanAcceptPlayerInput)
        {
            base.SetAction(_action);
        }
    }
    #endregion

    public void Die()
    {

    }
    public void ReverseDying()
    {

    }

    public void Activate()
    {
        if(_currentTarget != null && _currentTarget.isActivatable &&
            (transform.position - _currentTarget.Position).magnitude < _currentTarget.MinDistanceToActivate)
        {
            Ray _ray = new Ray(transform.position, _currentTarget.Position - transform.position);
            RaycastHit _hit; 
            if(Physics.Raycast(_ray, out _hit, GameManager.CollMask))
            {
                if(System.Object.ReferenceEquals(_hit.collider.gameObject, _currentTarget.Go))
                {
                    _currentTarget.Activate(); 
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
    public IAction CreateTargetAction()
    {
        return new TargetedAction(ActionType.Activate, _currentTarget); 
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
        SetAction(_combat.ActionsToReset(_state.ActionsToReset()));
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
        SetAction(_combat.SetStateToKeyboard(_state.SetStateToKeyboard()));
    }

        // Use this for initialization
    void Start () {
        SetAction(new Action(ActionType.Null));
        _rollI =  SRand.GetStartingIndex(); 
	}
    void Awake()
    {
        GameManager.RegisterCharacter(this);
        RegisterWibblyWobbly(); 
        _cam = gameObject.GetComponent<CharacterCam>();
        _camTrans = _cam.CameraSpot;
        _inventory = gameObject.GetComponent<Inventory>();
        _currentHealth = _health;
        _renderer = GetComponent<Renderer>();
        _combat = GetComponent<CombatState>(); 
        if(_vitalsTrans.Length > 0)
        {
            _vitals.SetVitals(_vitalsTrans);
        }else
        {
            _vitals.SetVitals(transform); 
        }
    }
	
	// Update is called once per frame
	void Update () {
        ITargetable _target = GameManager.GetTargeted(GameSettings.MaxTargetDistance); 
        if(_target == null && _currentTarget != null)
        {
            _currentTarget.UnTargeted();
            _currentTarget = _target; 
        }
        if(!System.Object.ReferenceEquals(_target, _currentTarget) && _target != null)
        {
            if(_currentTarget != null)
            {
                _currentTarget.UnTargeted();
            }
            _currentTarget = _target;
            _currentTarget.Targeted(); 
        }
    }

   

}