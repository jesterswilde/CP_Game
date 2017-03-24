using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimeCounter : MonoBehaviour {
 //   [SerializeField]
 //   Slider _slider; 
    [SerializeField]
    Text[] _currentTime;
 //   [SerializeField]
 //   Text _sliderValue;
 //   [SerializeField]
//    Image _image;
    [SerializeField]
    Image[] _limitImage;
    [SerializeField]
    Text[] _limitText;
    [SerializeField]
    Text[] _timeLimit;
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
    int _maxTime = 0;
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
        SetLevelTime();
    }
    public static void SwitchedToCharacter(Character _character)
    {
		if (_character != null) {
			for (int i = 0; i < t._charaNames.Length; i++) {
				t._charaNames [i].text = _character.name;
			}
			for (int i = 0; i < t._timelines.Count; i++) {
				t._timelines [i].SwitchedToCharacter (_character);
			}
		}

    }

    public static void SetLevelTime ()
    {
        if (t._mostFutureTime != 0)
        {
            for (int i = 0; i < t._timeLimit.Length; i++)
            {
                t._timeLimit[i].text = t._mostFutureTime + " Seconds";
            }
        }
        else
        {
            for (int i = 0; i < t._limitImage.Length; i++)
            {
                t._limitImage[i].enabled = false;
            }
            for (int i = 0; i < t._limitText.Length; i++)
            {
                t._limitText[i].enabled = false;
            }
            for (int i = 0; i < t._timeLimit.Length; i++)
            {
                t._timeLimit[i].enabled = false;
            }
        }
    }

   /* public static void TookDamage(Character _character)
    {
		for (int i = 0; i < t._hexes.Length; i++)
        {
			t._hexes[i].SetIntVisibility(i < _character.Combat.CurrentHealth);
        }
    }
    */

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
		if (t._currentTime != null)
        {
            for (int i = 0; i < t._currentTime.Length; i++)
            {
                t._currentTime[i].text = System.Math.Round(_time, 2).ToString("0.00");
            }
        }
/*		if (GameManager.ShowingCtrlMenu && t._slider != null && GameSettings.Speeds.Length > 0)
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
*/    }
}
/*
 * Get ahold of hexes*
 * On character change, hide excess hexes (in excess of max health of active character)
 * On damage (or character change) hide inner hex
 * 
 * */

 
