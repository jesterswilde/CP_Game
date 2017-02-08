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
    Color _baseColor; 
    public CombatState Combat { get { return null; } }
    public GameObject Go { get { return gameObject; } }
    public bool isActivatable { get { return true; } }

    public bool isAttackable { get { return false; } }

    public bool IsVisible { get { return _renderer.isVisible; } }

    public float MinDistanceToActivate { get { return _minDistanceToActivate; } }

    public Vector3 Position { get { return transform.position; } }

    public List<Action> Activate(Character _character)
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

    public void Targeted()
    {
        _renderer.material.color = Color.blue; 
    }

    public void UnTargeted()
    {
        _renderer.material.color = _baseColor; 
    }

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _baseColor = _renderer.material.color; 
    }
    void Start()
    {
        GameManager.RegisterTargetable(this); 
    }
}
