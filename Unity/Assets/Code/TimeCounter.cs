using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimeCounter : MonoBehaviour {
 //   [SerializeField]
 //   Slider _slider; 
    [SerializeField]
    Text[] _currentTime;
    /*/   [SerializeField]
    //   Text _sliderValue;
    //   [SerializeField]
    //    Image _image;
    */
    [SerializeField]
    Image[] _limitImage;
    [SerializeField]
    Text[] _limitText;
    [SerializeField]
    Text[] _timeLimit;
    [SerializeField]
    Text[] _charaNames;
    [SerializeField]
    ActionLogUI[] _actionUI;
    List<string> _actionLog = new List<string>();
    List<float> _actionTime = new List<float>();
    [SerializeField]
    ObjectiveUI[] _objectiveUI;
    [SerializeField]
    string[] _objText;
    bool _objComplete;
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
        EnableObjectives();
        for (int i = 0; i < _actionUI.Length; i++)
        {
            _actionUI[i].SetLogVisibility(false);
        }
    }
    void Update()
    {
        UpdateAction();
        if (Input.GetKeyDown (KeyCode.L))
        {
            LogAction("I pressed L", GameManager.GameTime);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            _objText[0] = "Get to da choppa";
            EnableObjectives();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ObjectiveComplete(0, true);
        }
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
    public void LogAction (string _content, float _currentTime)
    {
        _actionLog.Add(_content);
        _actionTime.Add(_currentTime);
    }
    public void UpdateAction ()
    {
        if (_actionTime.Count >= 1)
        {
            for (int i = _actionTime.Count - 1; i >= 0; i--)
            {
                Debug.Log("i " + i);
                Debug.Log("_actionTime " + _actionTime.Count);
                if (_actionTime.Count - 1 >= 3 && _actionTime[i] <= GameManager.GameTime)
                {
                    _actionUI[0].SetLogVisibility(true);
                    _actionUI[0].SetText(_actionLog[i], _actionTime[i]);
                    _actionUI[1].SetLogVisibility(true);
                    _actionUI[1].SetText(_actionLog[i - 1], _actionTime[i - 1]);
                    _actionUI[2].SetLogVisibility(true);
                    _actionUI[2].SetText(_actionLog[i - 2], _actionTime[i - 2]);
                    return;
                }
                else if (_actionTime.Count - 1 == 2 && _actionTime[i] <= GameManager.GameTime)
                {
                    _actionUI[0].SetLogVisibility(true);
                    _actionUI[0].SetText(_actionLog[i], _actionTime[i]);
                    _actionUI[1].SetLogVisibility(true);
                    _actionUI[1].SetText(_actionLog[i - 1], _actionTime[i - 1]);
                    _actionUI[2].SetLogVisibility(false);
                    return;
                }
                else if (_actionTime.Count - 1 == 1 && _actionTime[i] <= GameManager.GameTime)
                {
                    _actionUI[0].SetLogVisibility(true);
                    _actionUI[0].SetText(_actionLog[i], _actionTime[i]);
                    _actionUI[1].SetLogVisibility(false);
                    _actionUI[2].SetLogVisibility(false);
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < _actionUI.Length; i++)
            {
                _actionUI[i].SetLogVisibility(false);
            }
        }
    }
    public void EnableObjectives()
    {
        for (int i = 0; i < _objectiveUI.Length; i++)
        {
            if (_objText[i] == "0")
            {
                //_objectiveUI[i].SetMainVisibility(false);
            }
            else
            {
                /*_objectiveUI[i].SetMainVisibility(true);
                _objectiveUI[i].SetText(_objText[i]);
                _objectiveUI[i].CompleteQuest(false);*/
            }
        }
    }
    public void ObjectiveComplete (int number, bool visibility)
    {
        _objectiveUI[number].CompleteQuest(visibility);
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

/*if (_actionTime.Count >= 3)
            {
               _actionUI[0].SetText(_actionLog[i], _actionTime[i]);
               _actionUI[1].SetText(_actionLog[i-1], _actionTime[i-1]);
               _actionUI[2].SetText(_actionLog[i-2], _actionTime[i - 2]);
            }
            else if (_actionTime.Count == 2)
            {
                _actionUI[0].SetText(_actionLog[i], _actionTime[i]);
                _actionUI[1].SetText(_actionLog[i-1], _actionTime[i-1]);
                _actionUI[2].SetLogVisibility(false);
            }
            else if (_actionTime.Count == 1)
            {
                _actionUI[0].SetText(_actionLog[i], _actionTime[i]);
                _actionUI[1].SetLogVisibility(false);
                _actionUI[2].SetLogVisibility(false);
            }
            i++;
            return;*/


