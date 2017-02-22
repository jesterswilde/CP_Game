using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfAlertVolume : MonoBehaviour {
    [SerializeField]
    LayerMask _collMask;
    RequiredItems _requiredItems;
    [SerializeField]
    Task _enterTask;
    [SerializeField]
    TriggerType _enterTrigger;
    [SerializeField]
    Task _exitTask;
    [SerializeField]
    TriggerType _exitTrigger; 

    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            Interactable _inter = _coll.gameObject.GetComponent<Interactable>();
            if (_inter != null)
            {
                Debug.Log("enter trigger");
                _inter.ExternalTrigger(_enterTask, _enterTrigger, null); 
            }
        }
    }
    void OnTriggerExit(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            Interactable _inter = _coll.gameObject.GetComponent<Interactable>();
            if (_inter != null)
            {
                _inter.ExternalTrigger(_exitTask, _exitTrigger, null);
            }
        }
    }
    void Awake()
    {
        _requiredItems = GetComponent<RequiredItems>();
    }
}
