using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    List<Weapon> _weapons = new List<Weapon>();
    Dictionary<string, float> _items = new Dictionary<string, float>(); 
    int _weaponIndex = 0; 
    public Weapon SelectedWeapon { get { return (_weapons.Count > 0) ? _weapons[_weaponIndex] : null; } }

    public string PrintInventory()
    {
        string result = ""; 
        foreach(KeyValuePair<string, float> key in _items)
        {
            Debug.Log(key.Key + " | " + key.Value); 
            if(key.Value > 0)
            {
                result += key.Key + ": " + key.Value.ToString() + "\n"; 
            }
        }
        return result; 
    }
    public bool HasItem(string _name)
    {
        return _items.ContainsKey(_name) && _items[_name] > 0; 
    }
    public bool HasItem(string _name, float _amount)
    {
        return _items.ContainsKey(_name) && _items[_name] >= _amount;
    }
    public void PickUpItem(InvenItem _item)
    {
        Debug.Log(_item); 
        if (_items.ContainsKey(_item.ItemName))
        {
            _items[_item.ItemName] += _item.Amount;
        }else
        {
            _items[_item.ItemName] = _item.Amount; 
        }
        _item.PickUp(); 
    }
    public bool RemoveItem(InvenItem _item)
    {
        _item.PutDown(); 
        return RemoveItem(_item.ItemName, _item.Amount); 
    }
    public bool RemoveItem(string _name, float _amount)
    {
        if(!HasItem(_name, _amount))
        {
            return false; 
        }
        _items[_name] -= _amount;
        return true; 
    }
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
