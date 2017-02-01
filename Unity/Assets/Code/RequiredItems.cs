using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class RequiredItems : MonoBehaviour {

    [SerializeField]
    List<StringIntKVP> _requiredItems;
    [SerializeField]
    bool _consumesItems = false; 

    public bool HasRequiredItems(Character _character)
    {
        return _requiredItems.All((StringIntKVP _kvp) => _character.Inventory.HasItem(_kvp.Key, _kvp.Value));
    }
    public List<IAction> UseItems()
    {
        if (_consumesItems)
        {
            List<IAction> _actions = new List<IAction>(); 
            for(int i = 0; i < _requiredItems.Count; i++)
            {
                _actions.Add(new ItemAction(ActionType.PutDown, new InvenItem(_requiredItems[i].Key, _requiredItems[i].Value)));
            }
            return _actions; 
        }
        return null; 
    }
}
