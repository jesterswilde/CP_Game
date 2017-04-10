using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartUI : MonoBehaviour {

    [SerializeField]
    Text[] _description;
    [SerializeField]
    Text[] _conditions;
    [SerializeField]
    Text[] _allText;
    [SerializeField]
    Image _bkg;
    [SerializeField]
    string _descContent;
    [SerializeField]
    string _condContent;
    bool _closed = false;

    void Start ()
    {
        CreateText();
	}

    void Update ()
    {
        if (Input.anyKeyDown && _closed == false)
        {
            EnableVisbility(false);
            _closed = true;
        }
    }

    public void CreateText()
    {
        for (int i = 0; i < _description.Length; i++)
        {
            _description[i].text = _descContent;
        }
        for (int i = 0; i < _conditions.Length; i++)
        {
            _conditions[i].text = _condContent;
        }
    }

    public void EnableVisbility(bool visibility)
    {
        for (int i = 0; i < _allText.Length; i++)
        {
            _allText[i].enabled = visibility;
            _bkg.enabled = visibility;
            _description[i].enabled = visibility;
            _conditions[i].enabled = visibility;
        }
    }
}
