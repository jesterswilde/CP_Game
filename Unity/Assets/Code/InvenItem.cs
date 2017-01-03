using UnityEngine;
using System.Collections;

public class InvenItem {
    string _itemName = "McGuffin";
    public string ItemName { get { return _itemName; } }
    float _amount = 1;
    public float Amount { get { return _amount; } }
    bool _isPhysical = false;
    public bool IsPhysical { get { return _isPhysical; } }
    Item _original;
    public Item Original { get { return _original;  } } 
    public void PutDown()
    {
        _original.PutDown(); 
    }
    public void PutDown(Vector3 _pos, Quaternion _rot)
    {
        _original.PutDown(_pos, _rot); 
    }
    public void PickUp()
    {
        Debug.Log("intermdiary pick up "); 
        _original.PickedUp(); 
    }

    public InvenItem(Item _item)
    {
        _itemName = _item.ItemName;
        _amount = _item.Amount;
        _isPhysical = _item.IsPhysical;
        _original = _item; 
    }
    public InvenItem(string name, float amount)
    {
        _itemName = name;
        _amount = amount;
    }
}
