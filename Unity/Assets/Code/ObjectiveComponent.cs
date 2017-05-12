using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq; 

public class ObjectiveComponent : MonoBehaviour {

    [SerializeField]
    string _objective;
    [SerializeField]
    int _priority; 
    public int Priority { get { return _priority; } }

    bool _isComplete;
    bool _willComplete;

    MatchEvaluator _parser = (Match _match) =>
    {
        /*
         * c - character inventory (c, characterName, item)
         * g - global inventory (g, item)
        */
        string[] _tokens = _match.Value.Split(',');
        switch (_tokens[0])
        {
            case "c":
                return GameManager.CharByName(_tokens[1]).Inventory.NumHeld(_tokens[2]).ToString();
        }

        return ""; 
    }; 
  
}
