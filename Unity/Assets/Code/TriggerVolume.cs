using UnityEngine;
using System.Collections;

public class TriggerVolume : MonoBehaviour {


    [SerializeField]
    LayerMask _collMask; 
    [SerializeField]
    ActionType _enter;
    [SerializeField]
    ActionType _exit;
    WibblyWobbly _timeyWimey;
    [SerializeField]
    int _count = 0; 

    void Awake()
    {
        _timeyWimey = GetComponentInParent<WibblyWobbly>(); 
    }
    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _count++; 
            Debug.Log("entering " + _enter + " | " + _count);
            if(_count == 1)
            {
                _timeyWimey.SetAction(new Action(_enter, true));
            }
        }
    }
    void OnTriggerExit(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            Debug.Log("exiting " + _exit + " | " + _count); 
            _count--;
            if (_count == 0)
            {
                _timeyWimey.SetAction(new Action(_exit, true));
            }
        }
    }
}
