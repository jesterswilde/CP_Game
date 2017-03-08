using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterState
{

    int _forward = 0;
    int _backward = 0;
    int _left = 0;
    int _right = 0;
    bool _pullingTrigger = false; 
    float _rotX = 0;
    int _canMoveForward = 1;
    int _canMoveBackward = 1;
    int _canMoveLeft = 1;
    int _canMoveRight = 1;
    int _isGrounded = 0;
    Vector3 _groundNormal = Vector3.zero; 
    Vector3 _prevPos = Vector3.zero;
    Quaternion _prevRot = new Quaternion();

    public int Forward { get { return _forward; } set { _forward = value; } }
    public int Backward { get { return _backward; } set { _backward = value; } }
    public int Left { get { return _left; } set { _left = value; } }
    public int Right { get { return _right; } set { _left = value; } }
    public Vector3 GroundNormal { get { return _groundNormal; } }
    public bool PullingTrigger { get { return _pullingTrigger; } }
    public int CanForward { get { return _canMoveForward; } }
    public int CanBackward { get { return _canMoveBackward; } }
    public int CanLeft { get { return _canMoveLeft; } }
    public int CanRight { get { return _canMoveRight; } }
    public int IsGrounded { get { return _isGrounded; } }
    public float XRot { get { return _rotX; } }
    public Vector3 PrevPos { get { return _prevPos; } set { _prevPos = value; } }
    public Quaternion PrevRot { get { return _prevRot; } set { _prevRot = value; } }

    public List<Action> ActionsToReset()
    {
        List<Action> _actions = new List<Action>(); 
        if(Forward == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseForward));
        }
        if(Backward == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseBack));
        }
        if(Left == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseLeft));
        }
        if(Right == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseRight));
        }
        return _actions; 
    }
	public List<Action> SetStateToKeyboard(Character _character)
    {
        bool[] _keyboard = PlayerInput.GetAbsKeyboardState(); 
        List<Action> _actions = new List<Action>(); 
        if(_keyboard[0] && _forward == 0)
        {
            _actions.Add(new BasicAction(ActionType.PressForward));
        }
        if(!_keyboard[0] && _forward == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseForward));
        }
        if (_keyboard[1] && _right == 0)
        {
            _actions.Add(new BasicAction(ActionType.PressRight));
        }
        if (!_keyboard[1] && _right == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseRight));
        }
        if (_keyboard[2] && _backward == 0)
        {
            _actions.Add(new BasicAction(ActionType.PressBack));
        }
        if (!_keyboard[2] && _backward == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseBack));
        }
        if (_keyboard[3] && _left == 0)
        {
            _actions.Add(new BasicAction(ActionType.PressLeft)); 
        }
        if (!_keyboard[3] && _left == 1)
        {
            _actions.Add(new BasicAction(ActionType.ReleaseLeft));
        }
        return _actions; 
    }
    public bool IsMoving()
    {
        if (_forward != 0 || _backward != 0 || _left != 0 || _right != 0)
        {
            return true;
        }
        return false; 
    }
    public void ClearState()
    {
        _forward = 0;
        _backward = 0;
        _right = 0;
        _left = 0;
    }
    public void SetToBase(CharacterState _base)
    {
        _forward = _base.Forward;
        _backward = _base.Backward;
        _right = _base.Right;
        _left = _base.Right; 
    }
    public void UseAction(Action _action)
    {
        switch (_action.Type)
        {
            case ActionType.ForwardBlocked:
                _canMoveForward = 0;
                break;
            case ActionType.ForwardUnblocked:
                _canMoveForward = 1;
                break;
            case ActionType.RightBlocked:
                _canMoveRight = 0;
                break;
            case ActionType.RightUnblocked:
                _canMoveRight = 1;
                break;
            case ActionType.BackwardBlocked:
                _canMoveBackward = 0;
                break;
            case ActionType.BackwardUnblocked:
                _canMoveBackward = 1;
                break;
            case ActionType.LeftBlocked:
                _canMoveLeft = 0;
                break;
            case ActionType.LeftUnblocked:
                _canMoveLeft = 1;
                break; 
            case ActionType.PressForward:
                _forward = 1;
                break;
            case ActionType.ReleaseForward:
                _forward = 0;
                break;
            case ActionType.PressLeft:
                _left = 1;
                break;
            case ActionType.ReleaseLeft:
                _left = 0;
                break;
            case ActionType.PressBack:
                _backward = 1;
                break;
            case ActionType.ReleaseBack:
                _backward = 0;
                break;
            case ActionType.PressRight:
                _right = 1;
                break;
            case ActionType.StandingOnSurface:
                _groundNormal = _action.Vector;
                break; 
            case ActionType.IsGrounded:
                _isGrounded = 1;
                break;
            case ActionType.IsFalling:
                _isGrounded = 0;
                break; 
            case ActionType.ReleaseRight:
                _right = 0;
                break;
            case ActionType.Rotation:
                _rotX += _action.Value;
                break;
            case ActionType.Clear:
                ClearState();
                break;
            case ActionType.PullTrigger:
                _pullingTrigger = true;
                break;
            case ActionType.ReleaseTrigger:
                _pullingTrigger = false;
                break;
            default:
                break;
        }
    }

    public void ReverseAction(Action _action)
    {
        switch (_action.Type)
        {
            case ActionType.ForwardBlocked:
                _canMoveForward = 1;
                break;
            case ActionType.ForwardUnblocked:
                _canMoveForward = 0;
                break;
            case ActionType.RightBlocked:
                _canMoveRight = 1;
                break;
            case ActionType.RightUnblocked:
                _canMoveRight = 0;
                break;
            case ActionType.BackwardBlocked:
                _canMoveBackward = 1;
                break;
            case ActionType.BackwardUnblocked:
                _canMoveBackward = 0;
                break;
            case ActionType.LeftBlocked:
                _canMoveLeft = 1;
                break;
            case ActionType.LeftUnblocked:
                _canMoveLeft = 0;
                break;
            case ActionType.IsGrounded:
                _isGrounded = 0;
                break;
            case ActionType.IsFalling:
                _isGrounded = 1;
                break;
            case ActionType.LeavingSurface:
                _groundNormal = _action.Vector;
                break; 
            case ActionType.PressForward:
                _forward = 0;
                break;
            case ActionType.ReleaseForward:
                _forward = 1;
                break;
            case ActionType.PressLeft:
                _left = 0;
                break;
            case ActionType.ReleaseLeft:
                _left = 1;
                break;
            case ActionType.PressBack:
                _backward = 0;
                break;
            case ActionType.ReleaseBack:
                _backward = 1;
                break;
            case ActionType.PressRight:
                _right = 0;
                break;
            case ActionType.ReleaseRight:
                _right = 1;
                break;
            case ActionType.Rotation:
                _rotX -= _action.Value;
                break;
            case ActionType.PullTrigger:
                _pullingTrigger = false;
                break;
            case ActionType.ReleaseTrigger:
                _pullingTrigger = true;
                break;
            default:
                break;
        }
    }
}
