using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(CombatState))]
public class Character : WibblyWobbly {
    [SerializeField]
    float speed = 5;
    [SerializeField]
    bool _canRewind = true;
    public bool CanRewind { get { return _canRewind; } }
    [SerializeField]
    bool _canDeleteFuture = true;
    public bool CanDeleteFuture { get { return _canDeleteFuture; } }
    bool _isDead = false;
    CharacterCam _cam;
    public CharacterCam Cam { get { return _cam; } }
    Transform _camTrans;
    public Transform CamPoint { get { return _cam.CameraSpot; } }
    [SerializeField]
    float _maxTargetDistance = 0.1f;
    ITargetable _currentTarget;
    int _rollI;
    bool _isExtracted;
    float _lastLockTime = 0; 

    CharacterState _state = new CharacterState();
    Renderer _renderer;
    Transform _artGO;
    Vector3 _artForward;
    float _lerpIncrement = 0.05f;
    float _lerpAmount = 1f;
    float _lerpThreshold = 10;

    [SerializeField]
    bool _playerControlled = false;
    Inventory _inventory;
    public Inventory Inventory { get { return _inventory; } }

    #region Time
    bool CanAct()
    {
        return !_isDead && !_isExtracted;
    }
    public override void Play(float _time)
    {
        if (_currentTarget != null)
        {
            SetAction(_combat.PullTrigger(_time, _currentTarget.Combat), true);
        }
       if(_lastLockTime + 1 < GameManager.FixedGameTime 
            && (!_history.IsPointerAtHead() || _playerControlled))
        {
            LockFacing();
            _lastLockTime = GameManager.FixedGameTime; 
        }
        base.Play(_time);
        SetExternalAction(_state.AnimState());
    }

    internal string toString()
    {
        throw new NotImplementedException();
    }

    protected override void Act(float _deltaTime)
    {
        if (CanAct())
        {
            transform.forward = _cam.CameraForward(_state.XRot);
            Vector3 _move = ((_state.Forward * _state.CanForward) - (_state.Backward * _state.CanBackward)) * transform.forward +
                ((_state.Right * _state.CanRight) - (_state.Left * _state.CanLeft)) * transform.right;
            Vector3 _projectedMove = Vector3.ProjectOnPlane(_move, _state.GroundNormal);
            if (_state.IsGrounded == 0)
            {
                _projectedMove = new Vector3(_projectedMove.x, -10, _projectedMove.z);
            }
            transform.position += _projectedMove.normalized * speed * _deltaTime;
            foreach (ITime _timeUser in _timeUsers) {
                _timeUser.Act(_deltaTime);
            }
        }
    }

    protected override void ActReverse(float _deltaTime)
    {
        if (CanAct())
        {
            transform.forward = _cam.CameraForward(_state.XRot);
            Vector3 _move = ((_state.Forward * _state.CanForward) - (_state.Backward * _state.CanBackward)) * transform.forward +
                ((_state.Right * _state.CanRight) - (_state.Left * _state.CanLeft)) * transform.right;
            Vector3 _projectedMove = Vector3.ProjectOnPlane(_move, _state.GroundNormal);
            if (_state.IsGrounded == 0)
            {
                _projectedMove = new Vector3(_projectedMove.x, -10, _projectedMove.z);
            }
            transform.position += _projectedMove.normalized * speed * _deltaTime * -1;
            foreach (ITime _timeUser in _timeUsers) {
                _timeUser.RewindAct(_deltaTime);
            }
        }
    }
    public override void PlayVisuals(float _time)
    {
        foreach (ITime _timeUser in _timeUsers) {
            SetExternalAction(_timeUser.PlayVisuals(_time));
        }
        SmoothArtLerp(Time.deltaTime * GameManager.GameSpeed);
    }
    public override void RewindVisuals(float _time)
    {
        foreach (ITime _timeUser in _timeUsers) {
            _timeUser.RewindVisuals(_time);
        }
        SmoothArtLerp(Time.deltaTime * GameManager.GameSpeed);
    }
    protected override void UseAction(Action _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.StandingOnSurface:
                SetExternalAction(new VectorAction(ActionType.LeavingSurface, _state.GroundNormal, true));
                break;
            case ActionType.Activate:
                Activate(_action.Target, _action.Vector);
                return;
            case ActionType.PickUp:
                _inventory.PickUpItem(_action.Item);
                return;
            case ActionType.PutDown:
                _inventory.RemoveItem(_action.Item);
                return;
            case ActionType.Extract:
                _isExtracted = true;
                return;
        }
        foreach (ITime _timeUser in _timeUsers) {
            SetAction(_timeUser.UseAction(_action, _time), true);
        }
        SetAction(_combat.UseAction(_action), true);
        _state.UseAction(_action);
    }
    protected override void ReverseAction(Action _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.Activate:
                if (_action.Target != null)
                {
                    _action.Target.RewindActivation(_action);
                }
                return;
            case ActionType.PickUp:
                _inventory.RemoveItem(_action.Item);
                return;
            case ActionType.PutDown:
                _inventory.PickUpItem(_action.Item);
                return;
            case ActionType.Extract:
                _isExtracted = false;
                return;
            case ActionType.LockTrans:
                transform.position = _action.Vector;
                transform.rotation = Quaternion.Euler(_action.SecondVec);
                _lastLockTime = _action.Time; 
                break;
        }

        foreach (ITime _timeUser in _timeUsers) {
            _timeUser.ReverseAction(_action, _time);
        }
        _combat.ReverseAction(_action);
        _state.ReverseAction(_action);
    }

    public override void SetAction(Action _action)
    {
        if (GameManager.CanAcceptPlayerInput && CanAct())
        {
            base.SetAction(_action);
        }
    }
    #endregion

    public string PrintInventory()
    {
        return gameObject.name + " Escaped with: \n" + _inventory.PrintInventory();
    }
    public void Die()
    {
        ClearState();
    }
    public void ReverseDying()
    {

    }
    public void Activate(ITargetable _target, Vector3 _dir)
    {
        float _dist = 0;
        if (_target != null)
        {
            _dist = (transform.position - _target.Position).magnitude;
        }
        if (_target != null && _target.isActivatable && _dist < _target.MinDistanceToActivate)
        {
            Ray _ray = new Ray(transform.position, _target.Position - transform.position);
            RaycastHit _hit;
            if (!Physics.Raycast(_ray, out _hit, _dist, GameManager.CollMask))
            {
                SetExternalAction(_target.Activate(this, _dir));
            }
        }
    }
    public void Target() {
        ITargetable _target = GameManager.GetTargeted(GameSettings.MaxTargetDistance);
        if (_target == null && _currentTarget != null)
        {
            _currentTarget.UnTargeted();
            _currentTarget = _target;
            return;
        }
        if (_target != null)
        {
            float _dist = Vector3.Distance(_target.Position, transform.position);
            if (_currentTarget != null)
            {
                _currentTarget.UnTargeted();
            }
            _currentTarget = _target;
            _currentTarget.Targeted(_dist);
        }
    }
    public bool WillRotate()
    {
        if (_playerControlled && _state.IsMoving())
        {
            return _state.XRot != _cam.XRot;
        }
        return false;
    }
    public void FaceCamera()
    {
        if (WillRotate())
        {
            SetAction(new ValueAction(ActionType.Rotation, RotationDifference()));
            //LockFacing(); 
        }
    }
    public float RotationDifference()
    {
        return _cam.XRot - _state.XRot;
    }
    public Action CreateTargetAction()
    {
        return new TargetedAction(ActionType.Activate, _currentTarget, _currentTarget.Position - transform.position);
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
        _history.AddToHead(new BasicAction(ActionType.Clear, _history.HeadAction.Time));
    }
    public float GetHeadTimestamp()
    {
        if (_history.HeadAction == null)
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
    public void SetStateToKeyboard()
    {
        SetAction(_combat.SetStateToKeyboard(_state.SetStateToKeyboard(this, transform.position, transform.rotation.eulerAngles)));
    }
    #region ArtGO

    void SmoothArtLerp(float _delta)
    {
        Vector3 _facing = GetFacing();
        if (_facing != _artForward && _state.IsMoving())
        {
            float _diff = Util.AngleBetweenVector3(_facing, _artForward, transform.up);
            float _theta = Mathf.Deg2Rad * (_delta / _lerpIncrement * _lerpAmount * _diff);
            if (Mathf.Abs(_diff) < _lerpThreshold * GameManager.GameSpeed)
            {
                _artForward = _facing;
            } else
            {
                float x = _artForward.x * Mathf.Cos(_theta) - _artForward.z * Mathf.Sin(_theta);
                float z = _artForward.z * Mathf.Cos(_theta) + _artForward.x * Mathf.Sin(_theta);
                _artForward.x = x;
                _artForward.z = z;
            }
            _artGO.forward = _artForward;
        }
    }
    Vector3 GetFacing()
    {
        Vector3 _forward = (_state.Forward - _state.Backward) * _cam.transform.forward;
        Vector3 _right = (_state.Right - _state.Left) * _cam.transform.right;
        return (_forward + _right).normalized;
    }
    void LockFacing()
    {
        SetExternalAction(new DirTargetAction(ActionType.LockTrans, transform.position, transform.rotation.eulerAngles, true));
    }

    #endregion

    // Use this for initialization
    void Start () {
		GameManager.RegisterCharacter(this);
		RegisterWibblyWobbly(); 
        SetAction(new BasicAction(ActionType.Null));
        _rollI =  SRand.GetStartingIndex(); 
		SetAction (new AnimAction (ActionType.AnimEnded, 0, 0)); 
	}
    void Awake()
    {
        _cam = gameObject.GetComponent<CharacterCam>();
        _camTrans = _cam.CameraSpot;
        _inventory = gameObject.GetComponent<Inventory>();
        _renderer = GetComponent<Renderer>();
        _combat = GetComponent<CombatState>();
        _artGO = GetComponentInChildren<Animator>().transform;
        _artForward = transform.forward; 
        if(_combat == null)
        {
            _combat = gameObject.AddComponent<CombatState>(); 
        }
        if (_combat != null)
        {
            _combat.SetCallbacks(new SetActionDelegate(SetAction));
        }
    }

   

}