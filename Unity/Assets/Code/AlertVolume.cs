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

    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _count++;
            if (_count == 1)
            {
                Character _character = _coll.gameObject.GetComponent<Character>(); 
                if(_character == null)
                {
                    _character = GetComponentInParent<Character>(); 
                }
                foreach(AlertActions _target in _alerts)
                {
                    _target.Interactable.ExternalTrigger(_target.Task, _target.Enter, _character); 
                }
            }
        }
    }
    void OnTriggerExit(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _count--;
            if (_count == 0)
            {
                Character _character = _coll.gameObject.GetComponent<Character>();
                if (_character == null)
                {
                    _character = GetComponentInParent<Character>();
                }
                foreach (AlertActions _target in _alerts)
                {
                    _target.Interactable.ExternalTrigger(_target.Task, _target.Exit, _character);
                }
            }
        }
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
    Task _task;
    [SerializeField]
    TriggerType _enter;
    [SerializeField]
    TriggerType _exit; 
    public Interactable Interactable { get { return _interact; } }
    public Task Task { get { return _task; } }
    public TriggerType Enter { get { return _enter;  } }
    public TriggerType Exit { get { return _exit;  } }
    AlertActions(Interactable interact, Task task, TriggerType enter, TriggerType exit)
    {
        _interact = interact;
        _task = task;
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
    Null
}
