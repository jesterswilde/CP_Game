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
 
    public void CompleteQuest(bool visibility)
    {
        for(int i = 0; i < _hexInterior.Length; i++)
        {
            _hexInterior[i].enabled = visibility;
        }
    }

}


