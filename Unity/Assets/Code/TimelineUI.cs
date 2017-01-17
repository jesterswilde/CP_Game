using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimelineUI : MonoBehaviour {

    [SerializeField]
    Image[] _activeCharaHex;
    [SerializeField]
    Image _timelineFill;
    [SerializeField]
    Text _charaTime;
    [SerializeField]
    Image[] _frozenElements;
    string _playerName;

    void Awake()
    {
        TimeCounter.RegisterTimeline(this);
    }

    public void SetCharacterNames(Character _character)
    {
        if (_character != null)
        {
            _playerName = _character.name;
        }
        else
        {
            SetTimelineVisibility(false);
            SetCharaHexVisibility(false);
        }
    }

    public void SetTimelineVisibility (bool visibility)
    {
        for (int i = 0; i < _frozenElements.Length; i++)
        {
            _frozenElements[i].enabled = visibility;
        }
        _timelineFill.enabled = visibility;
        _charaTime.enabled = visibility;
    }

    public void SetCharaHexVisibility(bool visibility)
    {
        for (int i = 0; i < _activeCharaHex.Length; i++)
        {
            _activeCharaHex[i].enabled = visibility;
        }
    }
}
