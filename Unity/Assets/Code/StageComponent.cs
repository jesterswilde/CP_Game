using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class StageComponent : MonoBehaviour {

    [SerializeField]
    int[] _activeStages;
    [SerializeField]
    int[] _replayStages; 
    Interactable _interactable;
    Character _character;
    List<Renderer> _renderers = new List<Renderer>();
    List<Collider> _colliders = new List<Collider>();
    StageChangeActivate _activator; 

    bool _isActive = true;
    bool _isReplaying = false;
    bool _isInactive = false;  
    public void LoadNewStage(int _stage)
    {
        Debug.Log("Loading new stage"); 
        if (_activeStages.Contains(_stage))
        {
            Debug.Log("Activating");
            Activate();
        }else
        {
            if (_replayStages.Contains(_stage))
            {
                SetToReplay();
            }else
            {
                Debug.Log("Deactivating"); 
                Deactivate(); 
            }
        }
    }
    void Activate()
    {
        if (!_isActive)
        {
            if (_isInactive)
            {
                for(int i = 0; i < _colliders.Count; i++)
                {
                    _colliders[i].enabled = true; 
                }
                for(int i = 0; i < _renderers.Count; i++)
                {
                    _renderers[i].enabled = true; 
                }
                if(_interactable != null)
                {
                    GameManager.RegisterWibblyWobbly(_interactable); 
                }
                if(_character != null)
                {
                    GameManager.RegisterWibblyWobbly(_character);
                    GameManager.RegisterCharacter(_character); 
                }
                if(_activator != null)
                {
                    _activator.CanActivate = true; 
                }
            }
            if (_isReplaying)
            {
                if (_character != null)
                {
                    GameManager.RegisterCharacter(_character);
                }
            }
            _isActive = true;
            _isReplaying = false;
            _isInactive = false;  
        }
    }
    void SetToReplay()
    {
        if (!_isReplaying)
        {
            if (_isInactive)
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
                }
                if (_activator != null)
                {
                    _activator.CanActivate = true;
                }
            }
            if (_isActive)
            {
                if(_character != null)
                {
                    GameManager.UnRegisterCharacter(_character); 
                }
            }
            _isReplaying = true;
            _isActive = false;
            _isInactive = false; 
        }
    }
    void Deactivate()
    {
        if (!_isInactive)
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
        _isActive = false;
        _isReplaying = false;
        _isInactive = true; 
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
        StageManager.RegisterStageComponent(this); 
    }
}
