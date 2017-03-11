using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq; 

public class ExtractionTrigger : MonoBehaviour, ITargetable {

    [SerializeField]
    float _minDistanceToActivate = 1f;
    Stack<Character> _extractedCharacters = new Stack<Character>();  
    Renderer _renderer;
	Material _baseMaterial; 

    public CombatState Combat { get { return null; } }
    public GameObject Go { get { return gameObject; } }
    public bool isActivatable { get { return true; } }
    public bool isAttackable { get { return false; } }
    public bool IsVisible { get { return _renderer.isVisible; } }
    public float MinDistanceToActivate { get { return _minDistanceToActivate; } }
    public Vector3 Position { get { return transform.position; } }

	public List<Action> Activate(Character _character, Vector3 _dir)
    {
        _extractedCharacters.Push(_character);
        Debug.Log(_extractedCharacters.Aggregate("Extracting...\n", (_result, _char) => _result += _char.PrintInventory())); 
        return new List<Action> { new BasicAction(ActionType.Extract, true) }; 
    }

    public void RewindActivation(Action _action)
    {
        _extractedCharacters.Pop();
    }

    public void SetAction(Action _action)
    {
        throw new NotImplementedException();
    }

    public void SetAction(Action _action, bool _evaluatSource)
    {
        throw new NotImplementedException();
    }

	public void Targeted(float _dist)
    {
		if (_dist < _minDistanceToActivate) {
			_renderer.material = ColorManager.ExtractionMaterial; 
		} else {
			_renderer.material = _baseMaterial;
		}
		_renderer.material.SetFloat ("_OutlineWidth", ColorManager.OutlineWidth); 
    }

    public void UnTargeted()
    {
        _renderer.material = _baseMaterial; 
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _baseMaterial = _renderer.material; 
    }
    void Start()
    {
        GameManager.RegisterTargetable(this); 
    }
}
