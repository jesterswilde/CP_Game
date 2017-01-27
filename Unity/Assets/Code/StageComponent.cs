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
    List<Renderer> _renderers;
    List<Collider> _colliders;

    bool _isActive = true;
    bool _isReplaying = false;
    bool _isInactive = false;  
    public void LoadNewStage(int _stage)
    {

        if (_activeStages.Contains(_stage))
        {
            Activate();
        }else
        {
            if (_replayStages.Contains(_stage))
            {
                SetToReplay();
            }else
            {
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
        _renderers = Util.GetComponents<Renderer>(gameObject);
        _colliders = Util.GetComponents<Collider>(gameObject);
        _interactable = GetComponent<Interactable>();
        _character = GetComponent<Character>(); 
    }
}
