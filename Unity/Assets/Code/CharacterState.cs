using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterState
{

    int _forward = 0;
    int _backward = 0;
    int _left = 0;
    int _right = 0;
    float _rotX = 0;
    int _canMoveForward = 1;
    int _canMoveBackward = 1;
    int _canMoveLeft = 1;
    int _canMoveRight = 1; 
    Vector3 _prevPos = Vector3.zero;
    Quaternion _prevRot = new Quaternion();

    public int Forward { get { return _forward; } set { _forward = value; } }
    public int Backward { get { return _backward; } set { _backward = value; } }
    public int Left { get { return _left; } set { _left = value; } }
    public int Right { get { return _right; } set { _left = value; } }
    public int CanForward { get { return _canMoveForward; } }
    public int CanBackward { get { return _canMoveBackward; } }
    public int CanLeft { get { return _canMoveLeft; } }
    public int CanRight { get { return _canMoveRight; } }
    public float XRot { get { return _rotX; } }
    public Vector3 PrevPos { get { return _prevPos; } set { _prevPos = value; } }
    public Quaternion PrevRot { get { return _prevRot; } set { _prevRot = value; } }

    public List<IAction> ActionsToReset()
    {
        List<IAction> _actions = new List<IAction>(); 
        if(Forward == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseForward));
        }
        if(Backward == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseBack));
        }
        if(Left == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseLeft));
        }
        if(Right == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseRight));
        }
        return _actions; 
    }
    public List<IAction> SetStateToKeyboard()
    {
        bool[] _keyboard = PlayerInput.GetAbsKeyboardState(); 
        List<IAction> _actions = new List<IAction>(); 
        if(_keyboard[0] && _forward == 0)
        {
            _actions.Add(new Action(ActionType.PressForward));
        }
        if(!_keyboard[0] && _forward == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseForward));
        }
        if (_keyboard[1] && _right == 0)
        {
            _actions.Add(new Action(ActionType.PressRight));
        }
        if (!_keyboard[1] && _right == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseRight));
        }
        if (_keyboard[2] && _backward == 0)
        {
            _actions.Add(new Action(ActionType.PressBack));
        }
        if (!_keyboard[2] && _backward == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseBack));
        }
        if (_keyboard[3] && _left == 0)
        {
            _actions.Add(new Action(ActionType.PressLeft)); 
        }
        if (!_keyboard[3] && _left == 1)
        {
            _actions.Add(new Action(ActionType.ReleaseLeft));
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
    public void UseAction(IAction _action)
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
            case ActionType.ReleaseRight:
                _right = 0;
                break;
            case ActionType.Rotation:
                _rotX += _action.Value;
                break; 
            case ActionType.Clear:
                ClearState();
                break;
            default:
                break;
        }
    }

    public void ReverseAction(IAction _action)
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
            default:
                break;
        }
    }
}
