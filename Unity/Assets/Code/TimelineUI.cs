using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimelineUI : MonoBehaviour {

    [SerializeField]
    Image _timelineFill;
    [SerializeField]
    Text _topTime;
    [SerializeField]
    Text _bottomTime;
    [SerializeField]
    Image[] _topHex;
    [SerializeField]
    Image[] _bottomHex;
    [SerializeField]
    Image _topPortrait;
    [SerializeField]
    Image _bottomPortrait;
    [SerializeField]
    Sprite _hacker;
    [SerializeField]
    Sprite _office;
    [SerializeField]
    Sprite _merc;
    [SerializeField]
    Sprite _otaku;
    [SerializeField]
    Sprite _generic;
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
    void Start()
    {
        SetPortraitChara();
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
    public void SetPortrait(Sprite sprite)
    {
        _topPortrait.sprite = sprite;
        _bottomPortrait.sprite = sprite;
    }
    public void SetPortraitChara()
    {
        if (_playerName == "._.anTHraX._.")
        {
            SetPortrait(_hacker);
        }
        else if (_playerName == "Michael")
        {
            SetPortrait(_office);
        }
        else if (_playerName == "Gold")
        {
            SetPortrait(_merc);
        }
        else if (_playerName == "Tadachi")
        {
            SetPortrait(_otaku);
        }
        else
        {
            SetPortrait(_generic);
        }
    }

    public void SetTimelineVisibility (bool visibility)
    {
		if (_topTime != null && _bottomTime != null) {
			_topTime.enabled = visibility;
			_bottomTime.enabled = visibility;
		}
        for (int i = 0; i < _topHex.Length; i++)
        {
            _topHex[i].enabled = visibility;
        }
        for (int i = 0; i < _bottomHex.Length; i++)
        {
            _bottomHex[i].enabled = visibility;
        }
        _slider.gameObject.SetActive(visibility);
    }
    public void SetTopVisibility(bool visibility)
    {
		if (_topTime != null) {
			_topTime.enabled = visibility;
		}
        for (int i = 0; i < _topHex.Length; i++)
        {
            _topHex[i].enabled = visibility;
        }
    }
    public void SetBottomVisibility(bool visibility)
    {
		if (_bottomTime != null) {
			_bottomTime.enabled = visibility;
		}
        for (int i = 0; i < _bottomHex.Length; i++)
        {
            _bottomHex[i].enabled = visibility;
        }
    }

    public void SwitchedToCharacter(Character _character)
    {
        _isActive = _character.name == _playerName;
        if (_isActive)
        {
            SetTopVisibility(true);
            SetBottomVisibility(false);
            _timelineFill.enabled = true;
        }
        else
        {
            SetTopVisibility(false);
            SetBottomVisibility(true);
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
		if (_topTime != null && _bottomTime != null) {
			_topTime.text = System.Math.Round (_characterTime, 2).ToString ("0.00");
			_bottomTime.text = System.Math.Round (_characterTime, 2).ToString ("0.00");
		}
    }
}
