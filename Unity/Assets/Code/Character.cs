using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Character : MonoBehaviour {

    [SerializeField]
    float speed = 5; 

    Queue<Action> _actions = new Queue<Action>();
    Queue<Action> _remainigActions = new Queue<Action>(); 

    [SerializeField]
    bool _playerControlled = false;
    int _forward = 0;
    int _left = 0;
    int _back = 0;
    int _right = 0;
    Vector3 _oldVec; 

    public void Rewind()
    {
        float _oldTime = 0;
        DefaultState(); 
        _remainigActions = new Queue<Action>(_actions);
        while (true)
        {
            if(_remainigActions.Count > 0)
            {
                if (_remainigActions.Peek().Time < GameManager.GameTime)
                {
                    Action _currentAction = _remainigActions.Dequeue(); 
                    _oldVec = _currentAction.Position;
                    _oldTime = _currentAction.Time;  
                    UseAction(_currentAction);
                }
                else
                {
                    transform.position = _oldVec;
                    Act(GameManager.GameTime - _oldTime); 
                    break; 
                }
            }
            else
            {
                transform.position = _oldVec;
                Act(GameManager.GameTime - _oldTime);
                break;
            }
        }
        if (_playerControlled)
        {
            OverwriteFuture();
            DefaultState(); 
        }
    }

    void OverwriteFuture()
    {
        Queue<Action> _savedActions = new Queue<Action>();
        while (_actions.Count > 0 && _actions.Peek().Time < GameManager.GameTime)
        {
            _savedActions.Enqueue(_actions.Dequeue());
        }
        _actions = _savedActions; 
    }

    void DefaultState()
    {
        _right = 0;
        _forward = 0;
        _left = 0;
        _back = 0; 
    }

    public void SetAction(Action _action)
    {
        _actions.Enqueue(_action);
        UseAction(_action); 
    }

    public void UseAction(Action _action)
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
                _back = 1;
                break;
            case ActionType.ReleaseBack:
                _back = 0;
                break;
            case ActionType.PressRight:
                _right = 1;
                break;
            case ActionType.ReleaseRight:
                _right = 0;
                break;
            default:
                _forward = 0;
                _right = 0;
                _left = 0;
                _back = 0;
                break;
        }
    }

    public void Act()
    {
        float newY = transform.position.y + speed * (_forward - _back) * Time.deltaTime;
        float newX = transform.position.x + speed * (_right - _left) * Time.deltaTime;
        transform.position = new Vector3(newX, newY, transform.position.z); 
    }
    public void Act(float _deltaTime)
    {
        float newY = transform.position.y + speed * (_forward - _back) * _deltaTime;
        float newX = transform.position.x + speed * (_right - _left) * _deltaTime;
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    public void SetAsActivePlayer()
    {
        DefaultState();
        OverwriteFuture(); 
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
        _oldVec = transform.position;
	}
    void Awake()
    {
        GameManager.RegisterCharacter(this); 
    }
	
	// Update is called once per frame
	void Update () {
        if (!_playerControlled)
        {
            while (true)
            {
                if(_remainigActions.Count == 0)
                {
                    break;
                }
                if (_remainigActions.Peek().Time < GameManager.GameTime)
                {
                    UseAction(_remainigActions.Dequeue());
                }
                else
                {
                    break; 
                }
            }
        }
        Act(); 
    }
}
