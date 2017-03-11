using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System.Linq; 

public class StageChangeActivate : MonoBehaviour, ITargetable {

    [SerializeField]
    int _stage; 
	[SerializeField]
	string _levelName; 
    [SerializeField]
    float _minDistance = 5f; 
	[SerializeField]
	bool _targetable; 
    Renderer _renderer;
	Material _baseMaterial;
	List<IRequire> _activationRequiremnts = new List<IRequire> ();

    public GameObject Go { get { return gameObject; } }
    public bool IsVisible { get { return _renderer.isVisible; } }
    public CombatState Combat { get { return null; } }
	public bool isActivatable { get { return _targetable; } }
    public bool isAttackable { get { return false; } }
	bool _canActivate = true; 
	public bool CanActivate { get { return _canActivate; } set { _canActivate = value; } }
    public float MinDistanceToActivate { get { return _minDistance; } }

    public Vector3 Position { get { return transform.position; } }

	bool MeetsRequirements(Character _character){
		return _activationRequiremnts.All ((_req) => _req.AllowActivation (_character)); 
	}
	public List<Action> Activate(Character _character, Vector3 _dir)
    {
		if (MeetsRequirements(_character) )
        {
			if (_levelName != "") {
				GameManager.StartLoadingLevel (_levelName);
			} else {
				List<Action> _actions = new List<Action> { new BasicAction (ActionType.Null) }; //adding null to not rewind. 
				StageManager.ChangeStage (_stage); 
				foreach (IRequire _req in _activationRequiremnts) {
					_req.ActivationConsequences ().AddTo (_actions); 
				}
				return _actions;
			}
        }
        return null; 
    }

    public void RewindActivation(Action _action)
    {
        
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
		if (_dist < _minDistance) {
			_renderer.material = ColorManager.StageTargetMaterial; 
		} else {
			_renderer.material = _baseMaterial; 
		}
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }

    public void UnTargeted()
    {
        _renderer.material = _baseMaterial; 
		_renderer.material.SetFloat ("_OutlineWidth", 0); 
    }

    void Awake()
    {
		_activationRequiremnts = GetComponents<Component>().Where((Component _comp)=> _comp is IRequire).Select((_comp)=> _comp as IRequire).ToList(); 
        _renderer = GetComponent<Renderer>(); 
        _baseMaterial = _renderer.material;
    }
	void Start(){
		GameManager.RegisterTargetable(this); 
	}
}



