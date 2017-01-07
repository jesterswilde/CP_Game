using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthHex : MonoBehaviour
{
    [SerializeField]
    Image[] _hexExterior;
    [SerializeField]
    Image[] _hexInterior;
    bool _extHidden = false;
    bool _intHidden = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){
            _extHidden = !_extHidden; 
            SetExtVisibility(_extHidden); 
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            _intHidden = !_intHidden; 
            SetIntVisibility(_intHidden); 
        }

    }

    public void SetExtVisibility(bool visibility)
    {
        for (int i = 0; i < _hexExterior.Length; i++)
        {
            _hexExterior[i].enabled = visibility;
        }
        SetIntVisibility(visibility);        
    }

    public void SetIntVisibility(bool visibility)
    {
        for(int i = 0; i < _hexInterior.Length; i++)
        {
            _hexInterior[i].enabled = visibility;
        }
    }

}


