using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RequiredMissingItem : MonoBehaviour, IRequire {

    [SerializeField]
    List<StrIntBool> _requiredItems;
    [SerializeField]
    string _uiDescription;

    #region IRequire implementation

    public string UIText { get { return _uiDescription; } }
    public bool AllowActivation(Character _character)
    {
        return MissingItems(_character);
    }


    public List<Action> ActivationConsequences()
    {
        return UseItems();
    }


    #endregion



    public bool MissingItems(Character _character)
    {
        return _requiredItems.All((StrIntBool _kvp) => {
            if (!_kvp.Global)
            {
                return !_character.Inventory.HasItem(_kvp.Key, _kvp.Value);
            }
            else
            {
            Debug.Log(!Item.HasItem(_kvp.Key, _kvp.Value));
                return !Item.HasItem(_kvp.Key, _kvp.Value);
            }
        });
    }
    public List<Action> UseItems()
    {
        return null;
    }
}
