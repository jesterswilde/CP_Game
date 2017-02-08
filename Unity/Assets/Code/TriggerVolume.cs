using UnityEngine;
using System.Collections;

public class TriggerVolume : MonoBehaviour {

    [SerializeField]
    WibblyWobbly _timeyWimey;
    [SerializeField]
    LayerMask _collMask; 
    [SerializeField]
    ActionType _enter;
    [SerializeField]
    ActionType _exit;
    [SerializeField]
    int _count = 0;

    [SerializeField]
    Transform _enterTrans;
    [SerializeField]
    Transform _exitTrans;
    [SerializeField]
    float _enterValue;
    [SerializeField]
    float _exitValue; 

    void Awake()
    {
        if(_timeyWimey == null)
        {
            _timeyWimey = GetComponentInParent<WibblyWobbly>(); 
        }
    }
    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _count++; 
            if(_count == 1)
            {
                if(_enterTrans != null)
                {
                    _timeyWimey.SetExternalAction(new VectorAction(_enter, _enterTrans.position - _timeyWimey.transform.position, true));
                    return;
                }
                if(_enterValue != 0)
                {
                    _timeyWimey.SetExternalAction(new ValueAction(_enter, _enterValue, true));
                    return;
                }
                _timeyWimey.SetExternalAction(new BasicAction(_enter, true));
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
                if(_exitTrans != null)
                {
                    Debug.DrawRay(_timeyWimey.transform.position, _exitTrans.transform.position - _timeyWimey.transform.position, Color.red, 10f);  
                    _timeyWimey.SetExternalAction(new VectorAction(_exit, _exitTrans.position - _timeyWimey.transform.position, true));
                    return;
                }
                if(_exitValue != 0)
                {
                    _timeyWimey.SetExternalAction(new ValueAction(_exit, _exitValue, true));
                    return; 
                }
                _timeyWimey.SetExternalAction(new BasicAction(_exit, true));
            }
        }
    }
}
