using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : MonoBehaviour
{
    [SerializeField]
    LayerMask _collMask;
    [SerializeField]
    Weapon _damage;
    [SerializeField]
    bool _damageOnce;
    bool _canDamage = true;

    void OnTriggerEnter(Collider _coll)
    {
        if (Util.LayerMaskContainsLayer(_collMask, _coll.gameObject.layer))
        {
            CombatState _combat = Util.GetComponentInHierarchy<CombatState>(_coll.gameObject); 
            if (_combat != null && _canDamage)
            {
                if (_damageOnce)
                {
                    _canDamage = false;
                }
                _combat.SetAction(new WeaponAction(ActionType.ShotBy, _damage, true), true);
            }
        }
    }
}
