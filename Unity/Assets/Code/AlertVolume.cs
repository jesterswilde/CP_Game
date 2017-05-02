using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System;
using System.Linq;  

public class AlertVolume : MonoBehaviour {

    [SerializeField]
    AlertActions[] _alerts; 
    [SerializeField]
    LayerMask _collMask;
    int _count = 0;
    List<IRequire> _activationRequirements = new List<IRequire>();


    bool MeetsRequirements(Character _character)
    {
        return _activationRequirements.All((_req) => _req.AllowActivation(_character));
    }
    void OnTriggerEnter(Collider _coll)
    {
       Character _character = Util.GetComponentInHierarchy<Character>(_coll.gameObject); 
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer) && MeetsRequirements(_character))
        {
            _count++;
            if (_count == 1)
            {
                Debug.Log("Alerting"); 
                foreach(AlertActions _target in _alerts)
                {
                    _target.Interactable.ExternalTrigger(_target.EnterTask, _target.Enter, _character); 
                }
                List<Action> _actions = new List<Action>();
                foreach (IRequire _req in _activationRequirements)
                {
                    _req.ActivationConsequences().AddTo(_actions);
                }
                _character.SetAction(_actions);
            }
        }
    }
    void OnTriggerExit(Collider _coll)
    {
        Character _character = Util.GetComponentInHierarchy<Character>(_coll.gameObject);
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer) && MeetsRequirements(_character))
        {
            _count--;
            if (_count == 0)
            {
                foreach (AlertActions _target in _alerts)
                {
                    _target.Interactable.ExternalTrigger(_target.ExitTask, _target.Exit, _character);
                }
                List<Action> _actions = new List<Action>();
                foreach (IRequire _req in _activationRequirements)
                {
                    _req.ActivationConsequences().AddTo(_actions);
                }
                _character.SetAction(_actions);
            }
        }
    }
    void Awake()
    {
        _activationRequirements = GetComponents<Component>().Where((Component _comp) => _comp is IRequire).Select((_comp) => _comp as IRequire).ToList();
    }
}
[Serializable]
public class AlertList
{
    public List<AlertActions> alerts;
}
[Serializable]
public struct AlertActions
{
    [SerializeField]
    Interactable _interact;
    [SerializeField]
    Task _enterTask;
    [SerializeField]
    Task _exitTask; 
    [SerializeField]
    TriggerType _enter;
    [SerializeField]
    TriggerType _exit; 
    public Interactable Interactable { get { return _interact; } }
    public Task EnterTask { get { return _enterTask; } }
    public Task ExitTask { get { return _exitTask; } }
    public TriggerType Enter { get { return _enter;  } }
    public TriggerType Exit { get { return _exit;  } }
    AlertActions(Interactable interact, Task enterTask, Task exitTask, TriggerType enter, TriggerType exit)
    {
        _interact = interact;
        _enterTask = enterTask;
        _exitTask = exitTask;
        _enter = enter;
        _exit = exit;  
    }
}
public enum TriggerType
{
    Alert,
    UnAlert, 
    Activate,
    Deactivate,
    Interrupt,
    Resume, 
    Null
}
