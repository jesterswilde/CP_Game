using UnityEngine;
using System.Collections;

public class CharacterState
{

    int _forward = 0;
    int _backward = 0;
    int _left = 0;
    int _right = 0;
    float _rotX = 0; 
    Vector3 _prevPos = Vector3.zero;
    Quaternion _prevRot = new Quaternion();

    public int Forward { get { return _forward; } set { _forward = value; } }
    public int Backward { get { return _backward; } set { _backward = value; } }
    public int Left { get { return _left; } set { _left = value; } }
    public int Right { get { return _right; } set { _left = value; } }
    public float XRot { get { return _rotX; } }
    public Vector3 PrevPos { get { return _prevPos; } set { _prevPos = value; } }
    public Quaternion PrevRot { get { return _prevRot; } set { _prevRot = value; } }

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
