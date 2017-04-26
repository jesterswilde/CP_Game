using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingGlobalItems : MonoBehaviour {
    [SerializeField]
    List<StringIntKVP> _items;

    // Use this for initialization
    void Start()
    {
        foreach (StringIntKVP _item in _items)
        {
            Item.AcquireItem(_item.Key, _item.Value);
        }
    }
}
