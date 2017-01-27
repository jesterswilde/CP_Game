using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    void Start()
    {
        Debug.Log(GetComponent<Interactable>()); 
    }
}
