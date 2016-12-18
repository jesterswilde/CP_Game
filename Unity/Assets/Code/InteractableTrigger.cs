using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class InteractableTrigger : MonoBehaviour
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
    public void Target()
    {
        _mat.color = Color.yellow; 
    }
    public void UnTarget()
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
        GameManager.RegisterInteractableTrigger(this); 
    }
}
