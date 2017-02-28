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
    static Text Text;
    static Slider Slider;
    static Text SliderValue;
    static Image Image;
    static HealthHex[] Hexes;
    static Text[] CharaNames;
    static List<TimelineUI> _timelines = new List<TimelineUI>();
    static Color ActiveColor;
    static Color InactiveColor;
    static float _speed = 1;
    static float _mostFutureTime = 0;
    public static float Speed { get { return _speed; } }
    public static Color AColor { get { return ActiveColor; } }
    public static Color IColor { get { return InactiveColor; } }
    void Awake()
    { 
        Text = _text;
        Slider = _slider;
        SliderValue = _sliderValue;
        Image = _image;
        Hexes = _hexes;
        CharaNames = _charaNames;
        ActiveColor = _activeColor;
        InactiveColor = _inactiveColor;
    }
    void Start()
    {
        SetCharaToTimeline();
        _mostFutureTime = GameSettings.MaxLevelTime; 
    }
    public static void SwitchedToCharacter(Character _character)
    {
        for (int i = 0; i < Hexes.Length; i++)
        {
            Hexes[i].SetExtVisibility(i < _character.Combat.MaximumHealth);
        }
        TookDamage(_character);
        for (int i = 0; i < CharaNames.Length; i++)
        {
            CharaNames[i].text = _character.name;
        }
        for (int i = 0; i < _timelines.Count; i++)
        {
            _timelines[i].SwitchedToCharacter(_character);
        }
     

    }
    public static void TookDamage(Character _character)
    {
        for (int i = 0; i < Hexes.Length; i++)
        {
            Hexes[i].SetIntVisibility(i < _character.Combat.CurrentHealth);
        }
    }

    static void UpdateFuturePoint()
    {
        if (GameManager.FixedGameTime > _mostFutureTime)
        {
            _mostFutureTime = GameManager.FixedGameTime;
        }
    }
    static void SetCharaToTimeline()
    {
        for (int i = 0; i < _timelines.Count; i++)
        {
            if(GameManager.Characters.Count > i)
            {
                _timelines[i].SetCharacterNames(GameManager.Characters[i]);
            }
            else
            {
                _timelines[i].SetCharacterNames(null); 
            }

        }
    }
    public static void RegisterTimeline(TimelineUI _timeline)
    {
        _timelines.Add(_timeline);
    }
    static void UpdateTimelines(float _time)
    {
        for (int i = 0; i < _timelines.Count; i++)
        {
            _timelines[i].UpdateTimeline(_time, _mostFutureTime);
        }
    }
    public static void UpdateTime(float _time)
    {
        UpdateFuturePoint();
        UpdateTimelines(_time);
        if (Text != null)
        {
            Text.text = _time.ToString();
        }
        if (GameManager.ShowingCtrlMenu && Slider != null && GameSettings.Speeds.Length > 0)
        {
            Image.gameObject.SetActive(true);
            Slider.maxValue = GameSettings.Speeds[GameSettings.Speeds.Length - 1];
            Slider.minValue = GameSettings.Speeds[0]; 
            float _maxY = Screen.height;
            float _increment = _maxY / GameSettings.Speeds.Length; 
            float _y = Input.mousePosition.y;
            int _index = Mathf.FloorToInt(_y / _increment);
            Slider.value = GameSettings.Speeds[Mathf.Clamp(_index, 0, GameSettings.Speeds.Length - 1)]; 
            _speed = Slider.value;
            SliderValue.text = _speed.ToString(); 
        }else
        {
            Image.gameObject.SetActive(false);
        }
    }
}
/*
 * Get ahold of hexes*
 * On character change, hide excess hexes (in excess of max health of active character)
 * On damage (or character change) hide inner hex
 * 
 * */

 
