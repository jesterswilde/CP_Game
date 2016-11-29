using UnityEngine;
using System.Collections;

public class InteractableVolume : MonoBehaviour {

    [SerializeField]
    Interactable _interactable;
    [SerializeField]
    LayerMask _collMask;
    [SerializeField]
    Task _enterTask;
    [SerializeField]
    Task _exitTask;
    [SerializeField]
    int _count = 0;
 

    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _count++;
            if (_count == 1)
            {
                _interactable.SetTask(_enterTask);
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
                _interactable.SetTask(_exitTask);
            }
        }
    }
}