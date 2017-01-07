using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthHex : MonoBehaviour
{
    [SerializeField]
    Image[] _hexExterior;
    [SerializeField]
    Image[] _hexInterior;

    
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


