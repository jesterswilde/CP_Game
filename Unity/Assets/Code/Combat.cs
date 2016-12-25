using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

public class Combat : MonoBehaviour {

    [SerializeField]
    float accuracyLossForAllVitals;
    static float _accuracyLossForAllVitals;
    public static float AccuracyLossForAllVitals { get { return _accuracyLossForAllVitals; } }

    void Awake()
    {
        _accuracyLossForAllVitals = accuracyLossForAllVitals; 
    }

}
