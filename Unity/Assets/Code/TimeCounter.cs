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
    Text[] _charaName;
    static Text Text;
    static Slider Slider;
    static Text SliderValue;
    static Image Image;
    static HealthHex[] Hexes;
    static Text[] CharaName;
    static List<TimelineUI> _timelines = new List<TimelineUI>();
    static float _speed = 1;
    static float _mostFutureTime = 0;
    public static float Speed { get { return _speed; } }
    void Awake()
    { 
        Text = _text;
        Slider = _slider;
        SliderValue = _sliderValue;
        Image = _image;
        Hexes = _hexes;
        CharaName = _charaName;
    }
    void Start()
    {
        SetCharaToTimeline();
    }
    public static void SwitchedToCharacter(Character _character)
    {
        for (int i = 0; i < Hexes.Length; i++)
        {
            Hexes[i].SetExtVisibility(i < _character.Combat.MaximumHealth);
        }
        TookDamage(_character);
        for (int i = 0; i < CharaName.Length; i++)
        {
            CharaName[i].text = _character.name;
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
    public static void UpdateTime(string _time)
    {
        UpdateFuturePoint();
        if (Text != null)
        {
            Text.text = _time.ToString();
        }
        if (GameManager.ShowingCtrlMenu)
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

 
