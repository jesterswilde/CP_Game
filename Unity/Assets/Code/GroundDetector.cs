using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class GroundDetector : MonoBehaviour {

    Character _character;
    [SerializeField]
    LayerMask _collMask;
    List<Collider> _surfaces = new List<Collider>();
    [SerializeField]
    int count; 
    void Awake()
    {
        _character = GetComponentInParent<Character>();
    }
    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _surfaces.Add(_coll);
            if(_surfaces.Count == 1)
            {
                _character.SetExternalAction(new BasicAction(ActionType.IsGrounded, true));
            }
            _character.SetExternalAction(new VectorAction(ActionType.StandingOnSurface, _coll.transform.up, true)); 
        }
        count = _surfaces.Count;   
    }
    void OnTriggerExit(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            _surfaces.Remove(_coll);
            if (_surfaces.Count == 0)
            {
                _character.SetExternalAction(new VectorAction(ActionType.LeavingSurface, _coll.transform.up, true)); 
                _character.SetExternalAction(new BasicAction(ActionType.IsFalling, true));
            }else
            {
                _character.SetExternalAction(new VectorAction(ActionType.StandingOnSurface, _surfaces.Last().transform.up, true)); 
            }
        }
        count = _surfaces.Count;

    }
}
