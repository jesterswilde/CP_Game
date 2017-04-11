using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField]
    Image[] _hexExterior;
    [SerializeField]
    Image[] _hexInterior;
    [SerializeField]
    Text[] _text;

    public void SetText(string _content)
    {
        for (int i = 0; i < _text.Length; i++)
        {
            _text[i].text = _content;
        }
    }

    public void SetMainVisibility(bool visibility)
    {
        for (int i = 0; i < _hexExterior.Length; i++)
        {
            _hexExterior[i].enabled = visibility;
        }
        for(int i = 0; i < _text.Length; i++)
        {
            _text[i].enabled = visibility;
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


