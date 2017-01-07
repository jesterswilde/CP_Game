using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    static Text Text;
    static Slider Slider;
    static Text SliderValue;
    static Image Image;
    static HealthHex[] Hexes;
    static float _speed = 1; 
    public static float Speed { get { return _speed; } }
    void Awake()
    { 
        Text = _text;
        Slider = _slider;
        SliderValue = _sliderValue;
        Image = _image;
        Hexes = _hexes; 
    }
    public static void SwitchedToCharacter(Character _character)
    {
        for (int i = 0; i < Hexes.Length; i++)
        {
            Hexes[i].SetExtVisibility(i < _character.Combat.MaximumHealth);
        }
        TookDamage(_character);
    }
    public static void TookDamage(Character _character)
    {
        for (int i = 0; i < Hexes.Length; i++)
        {
            Hexes[i].SetIntVisibility(i < _character.Combat.CurrentHealth);
        }
    }
    public static void UpdateTime(string _time)
    {
        if(Text != null)
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

 
