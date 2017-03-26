using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimelineUI : MonoBehaviour {

    [SerializeField]
    Image _timelineFill;
    [SerializeField]
    Text _charaTime;
    [SerializeField]
    Image _pointer;
    [SerializeField]
    Slider _slider;
    [SerializeField]
    int _order;
    public static TimelineUI tui;
    string _playerName;
    bool _isActive;
    float _characterTime;
    public int Order { get { return tui._order; } }
    

    void Awake()
    {
        tui = (this);
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
        }
    }

    public void SetTimelineVisibility (bool visibility)
    {
        _charaTime.enabled = visibility;
		if (_pointer != null) {
			_pointer.enabled = visibility;
		}
        _slider.gameObject.SetActive(visibility);
    }

    public void SwitchedToCharacter(Character _character)
    {
        _isActive = _character.name == _playerName;
        if (_isActive)
        {
			if (_pointer != null) {
				_pointer.color = TimeCounter.AColor;
			}
            _charaTime.color = TimeCounter.AColor;
            _timelineFill.enabled = true;
        }
        else
        {
			if (_pointer != null) {
				_pointer.color = TimeCounter.IColor;
			}
            _charaTime.color = TimeCounter.IColor;
            _timelineFill.enabled = false;
        }
    }
    public void UpdateTimeline(float _currentTime, float _maxTime)
    {
        if (_isActive)
        {
            _characterTime = _currentTime;
        }
        _slider.value = _characterTime / _maxTime;
        _charaTime.text = _playerName + " (" + System.Math.Round(_characterTime, 2).ToString("0.00") + ")";
    }
}
