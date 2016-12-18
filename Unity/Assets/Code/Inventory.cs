using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    List<Weapon> _weapons = new List<Weapon>();
    int _weaponIndex = 0; 
    public Weapon SelectedWeapon { get { return (_weapons.Count > 0) ? _weapons[_weaponIndex] : null; } }

    public void SwitchWeapon(int _mod)
    {
        _weaponIndex += _mod;
        if(_weapons.Count == 0)
        {
            _weaponIndex = 0;
            return; 
        }
        while(_weaponIndex < 0)
        {
            _weaponIndex += _weapons.Count; 
        }
        while(_weaponIndex >= _weapons.Count)
        {
            _weaponIndex -= _weapons.Count; 
        }
    }

    void Awake()
    {
        _weapons.AddRange(GetComponentsInChildren<Weapon>());
    }
}
