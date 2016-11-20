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
    static Text Text;
    static Slider Slider;
    static Text SliderValue;
    static Image Image;  
    static float _speed = 1; 
    public static float Speed { get { return _speed; } }
    void Awake()
    {
        Text = _text;
        Slider = _slider;
        SliderValue = _sliderValue;
        Image = _image;  
    }

    public static void UpdateTime(float _time)
    {
        Text.text = _time.ToString();
        if (GameManager.IsPaused)
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
