using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TargetableTrigger : MonoBehaviour, ITargetable
{

    public float MinDistanceToActivate { get { return _minDistanceToActivate;  } }
    [SerializeField]
    Interactable _interactable;
    [SerializeField]
    LayerMask _collMask;
    [SerializeField]
    List<Task> _tasks = new List<Task>();
    [SerializeField]
    int _index = 0; 
    [SerializeField]
    float _minDistanceToActivate = 3f;
    [SerializeField]
    bool _loop = false; 

    Material _mat;
    Color _startingColor;
    Renderer _renderer;
    public bool IsVisible { get { return _renderer.isVisible; } }

    public Vector3 Position { get { return transform.position; } }

    public bool isActivatable { get { return true;} }
    public bool isAttackable { get { return false;} }
    public CombatState Combat { get { return null; } }
    public GameObject Go { get { return gameObject; } }

    public void Activate()
    {
        if(_tasks.Count == 0)
        {
            return; 
        }
        if(_index < _tasks.Count)
        {
            _interactable.SetTask(_tasks[_index]);
            _index++;
        }else
        {
            if (_loop)
            {
                _index = 0;
                Activate(); 
            }
        } 
    }
    public void RewindActivation()
    {
        _index--;
        if (_loop)
        {
            if(_index < 0)
            {
                _index = _tasks.Count -1    ; 
            }
        }
    }
    public void Targeted()
    {
        _mat.color = Color.yellow; 
    }
    public void UnTargeted()
    {
        _mat.color = _startingColor; 
    }

    void Awake()
    {
        _mat = GetComponent<Renderer>().material;
        _startingColor = _mat.color; 
        if(_interactable == null)
        {
            throw new System.Exception(gameObject.name + " has no interactable object"); 
        }
        _renderer = GetComponent<Renderer>();
    }
    void Start()
    {
        GameManager.RegisterTargetable(this); 
    }

    public void SetAction(IAction _action)
    {
        throw new NotImplementedException();
    }

    public void SetAction(IAction _action, bool _evaluatSource)
    {
        throw new NotImplementedException();
    }
}
