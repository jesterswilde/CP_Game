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

    CharacterState _state = new CharacterState();
    Renderer _renderer; 

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
        if(_currentTarget != null)
        {
            SetAction(_combat.PullTrigger(_time, _currentTarget.Combat), true);
        }
        base.Play(_time);
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
        }
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
        SetAction(_combat.UseAction(_action), true); 
        _state.UseAction(_action);
        switch (_action.Type)
        {
            case ActionType.TakeDamage:
                if (_playerControlled)
                {
//                    TimeCounter.TookDamage(this);
                }
                break;
        }
    }
    protected override void ReverseAction(Action _action, float _time)
    {
        switch (_action.Type)
        {
            case ActionType.Activate:
                if(_action.Target != null)
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
				transform.rotation = Quaternion.Euler (_action.OriginalVec); 
				break;
        }
        _combat.ReverseAction(_action);
        _state.ReverseAction(_action);
        switch (_action.Type)
        {
            case ActionType.TakeDamage:
                if (_playerControlled)
                {
//                    TimeCounter.TookDamage(this);
                }
                break;
        }
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
        if(_target != null)
        {
            _dist = (transform.position - _target.Position).magnitude; 
        }
        if(_target != null && _target.isActivatable && _dist <_target.MinDistanceToActivate)
        {
            Ray _ray = new Ray(transform.position, _target.Position - transform.position);
            RaycastHit _hit; 
            if(!Physics.Raycast(_ray, out _hit, _dist, GameManager.CollMask))
            {
				SetExternalAction(_target.Activate(this, _dir)); 
            }
        }
    }
	public void Target(){
		ITargetable _target = GameManager.GetTargeted(GameSettings.MaxTargetDistance); 
		if(_target == null && _currentTarget != null)
		{
			_currentTarget.UnTargeted();
			_currentTarget = _target;
			return;
		}
		if(_target != null)
		{
			float _dist = Vector3.Distance (_target.Position, transform.position); 
			if(_currentTarget != null)
			{
				_currentTarget.UnTargeted();
			}
			_currentTarget = _target;
			_currentTarget.Targeted(_dist); 
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
        SetAction(_combat.SetStateToKeyboard(_state.SetStateToKeyboard(this)));
    }

        // Use this for initialization
    void Start () {
		GameManager.RegisterCharacter(this);
		RegisterWibblyWobbly(); 
        SetAction(new BasicAction(ActionType.Null));
        _rollI =  SRand.GetStartingIndex(); 
	}
    void Awake()
    {
        _cam = gameObject.GetComponent<CharacterCam>();
        _camTrans = _cam.CameraSpot;
        _inventory = gameObject.GetComponent<Inventory>();
        _renderer = GetComponent<Renderer>();
        _combat = GetComponent<CombatState>();
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