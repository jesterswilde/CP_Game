using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectiveManager : MonoBehaviour {

    static ObjectiveManager t; 
    List<StringIntKVP> _currentObjectives = new List<StringIntKVP>(); 
    public static void AddObjective(string _text, int _rank)
    {
        
        t._currentObjectives.Add(new StringIntKVP(_text, _rank));
        t._currentObjectives.Sort(delegate(StringIntKVP _a, StringIntKVP _b) {
            return (_a.Value > _b.Value) ? 1 : -1; 
        });
    }
    void RepopulateList()
    {
        while(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

}
