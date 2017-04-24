using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharaDisplay : MonoBehaviour, IRequire
{
    [SerializeField]
    List<Character> _displayCharacters;

    Interactable _interactable;
    Character _character;
    List<Renderer> _renderers = new List<Renderer>();
    List<Collider> _colliders = new List<Collider>();
    StageChangeActivate _activator;
    bool _isActive = true; 

    #region IRequire implementation

    public string UIText { get { return ""; } }
    public bool AllowActivation(Character _character)
    {
        return _isActive; 
    }


    public List<Action> ActivationConsequences()
    {
        return null;
    }


    #endregion

    public void SwitchedCharacter(Character _character)
    {
        if (_displayCharacters.Contains(_character))
        {
            _isActive = true;
            Activate();
        }else
        {
            _isActive = false;
            Deactivate(); 
        }
    }

    void Activate()
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = true;
        }
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].enabled = true;
        }
        if (_interactable != null)
        {
            GameManager.RegisterWibblyWobbly(_interactable);
        }
        if (_character != null)
        {
            GameManager.RegisterWibblyWobbly(_character);
            GameManager.RegisterCharacter(_character);
        }
        if (_activator != null)
        {
            _activator.CanActivate = true;
        }
    }

    void Deactivate()
    {
        for (int i = 0; i < _colliders.Count; i++)
        {
            _colliders[i].enabled = false;
        }
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].enabled = false;
        }
        if (_activator != null)
        {
            _activator.CanActivate = false;
        }
        if (_interactable != null)
        {
            GameManager.UnRegisterWibblyWobbly(_interactable);
        }
        if (_character != null)
        {
            GameManager.UnRegisterWibblyWobbly(_character);
            GameManager.UnRegisterCharacter(_character);
        }
    }

    void Awake()
    {
        _renderers.AddRange(Util.GetComponents<Renderer>(gameObject).Where((Renderer _rend) =>
        {
            return _rend.enabled;
        }));
        _colliders.AddRange(Util.GetComponents<Collider>(gameObject).Where((Collider _coll) =>
        {
            return _coll.enabled;
        }));
        _interactable = GetComponent<Interactable>();
        _character = GetComponent<Character>();
        _activator = GetComponent<StageChangeActivate>();
    }
    void Start()
    {
        GameManager.RegsisterDisplayChara(this);
        SwitchedCharacter(GameManager.ActiveCharacter); 
    }
}
