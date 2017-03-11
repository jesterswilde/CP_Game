using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimeCounter : MonoBehaviour {
    [SerializeField]
    Slider _slider; 
    [SerializeField]
    Text _text;
    [SerializeField]
    Text _sliderValue;
    [SerializeField]
    Image _image;
    [SerializeField]
    HealthHex[] _hexes;
    [SerializeField]
    Text[] _charaNames;
    [SerializeField]
    Color _activeColor;
    [SerializeField]
    Color _inactiveColor;
	public static TimeCounter t; 
    List<TimelineUI> _timelines = new List<TimelineUI>();
    float _speed = 1;
    float _mostFutureTime = 0;
    public static float Speed { get { return t._speed; } }
	public static Color AColor { get { return t._activeColor; } }
	public static Color IColor { get { return t._inactiveColor; } }
    void Awake()
    { 
		t = this;
    }
    void Start()
    {
        SetCharaToTimeline();
        _mostFutureTime = GameSettings.MaxLevelTime; 
    }
    public static void SwitchedToCharacter(Character _character)
    {
		if (_character != null) {
			for (int i = 0; i < t._hexes.Length; i++) {
				t._hexes [i].SetExtVisibility (i < _character.Combat.MaximumHealth);
			}
			TookDamage (_character);
			for (int i = 0; i < t._charaNames.Length; i++) {
				t._charaNames [i].text = _character.name;
			}
			for (int i = 0; i < t._timelines.Count; i++) {
				t._timelines [i].SwitchedToCharacter (_character);
			}
		}

    }

    public static void TookDamage(Character _character)
    {
		for (int i = 0; i < t._hexes.Length; i++)
        {
			t._hexes[i].SetIntVisibility(i < _character.Combat.CurrentHealth);
        }
    }

    static void UpdateFuturePoint()
    {
        if (GameManager.FixedGameTime > t._mostFutureTime)
        {
            t._mostFutureTime = GameManager.FixedGameTime;
        }
    }
    static void SetCharaToTimeline()
    {
        for (int i = 0; i < t._timelines.Count; i++)
        {
            if(GameManager.Characters.Count > i)
            {
                t._timelines[i].SetCharacterNames(GameManager.Characters[i]);
            }
            else
            {
                t._timelines[i].SetCharacterNames(null); 
            }

        }
    }
    public static void RegisterTimeline(TimelineUI _timeline)
    {
        t._timelines.Add(_timeline);
    }
    static void UpdateTimelines(float _time)
    {
        for (int i = 0; i < t._timelines.Count; i++)
        {
            t._timelines[i].UpdateTimeline(_time, t._mostFutureTime);
        }
    }
	public static void ClearLevel(){
		t = null; 
	}
    public static void UpdateTime(float _time)
    {
        UpdateFuturePoint();
        UpdateTimelines(_time);
		if (t._text != null)
        {
			t._text.text = _time.ToString();
        }
		if (GameManager.ShowingCtrlMenu && t._slider != null && GameSettings.Speeds.Length > 0)
        {
			t._image.gameObject.SetActive(true);
			t._slider.maxValue = GameSettings.Speeds[GameSettings.Speeds.Length - 1];
			t._slider.minValue = GameSettings.Speeds[0]; 
            float _maxY = Screen.height;
            float _increment = _maxY / GameSettings.Speeds.Length; 
            float _y = Input.mousePosition.y;
            int _index = Mathf.FloorToInt(_y / _increment);
			t._slider.value = GameSettings.Speeds[Mathf.Clamp(_index, 0, GameSettings.Speeds.Length - 1)]; 
			t._speed = t._slider.value;
			t._sliderValue.text = t._speed.ToString(); 
        }else
        {
			t._image.gameObject.SetActive(false);
        }
    }
}
/*
 * Get ahold of hexes*
 * On character change, hide excess hexes (in excess of max health of active character)
 * On damage (or character change) hide inner hex
 * 
 * */

 
