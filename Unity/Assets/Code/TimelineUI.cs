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
    [SerializeField]
    Slider _slider;
    string _playerName;
    bool _isActive;
    float _characterTime;

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
        _charaTime.enabled = visibility;
        _slider.gameObject.SetActive(visibility);
    }

    public void SetCharaHexVisibility(bool visibility)
    {
        for (int i = 0; i < _activeCharaHex.Length; i++)
        {
            _activeCharaHex[i].enabled = visibility;
        }
    }
    public void SwitchedToCharacter(Character _character)
    {
        _isActive = _character.name == _playerName;
         SetCharaHexVisibility(_isActive);
        if (_isActive)
        {
            _timelineFill.color = TimeCounter.AColor;
        }
        else
        {
            _timelineFill.color = TimeCounter.IColor;
        }
    }
    public void UpdateTimeline(float _currentTime, float _maxTime)
    {
        if (_isActive)
        {
            _characterTime = _currentTime;
        }
        _slider.value = _characterTime / _maxTime;
        _charaTime.text = System.Math.Round(_characterTime, 2).ToString("0.00");
    }
}
