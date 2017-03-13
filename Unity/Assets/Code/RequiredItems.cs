using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class RequiredItems : MonoBehaviour, IRequire {

    [SerializeField]
    List<StringIntKVP> _requiredItems;
    [SerializeField]
    bool _consumesItems = false;
	[SerializeField]
	string _uiDescription; 

	#region IRequire implementation

	public string UIText { get { return _uiDescription; } }
	public bool AllowActivation (Character _character)
	{
		return HasRequiredItems (_character); 
	}


	public List<Action> ActivationConsequences ()
	{
		return UseItems (); 
	}


	#endregion

 

    public bool HasRequiredItems(Character _character)
    {
        return _requiredItems.All((StringIntKVP _kvp) => _character.Inventory.HasItem(_kvp.Key, _kvp.Value));
    }
    public List<Action> UseItems()
    {
        if (_consumesItems)
        {
            List<Action> _actions = new List<Action>(); 
            for(int i = 0; i < _requiredItems.Count; i++)
            {
                _actions.Add(new ItemAction(ActionType.PutDown, new InvenItem(_requiredItems[i].Key, _requiredItems[i].Value)));
            }
            return _actions; 
        }
        return null; 
    }
}
