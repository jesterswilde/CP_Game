using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {

    [SerializeField]
    Text _text;
    static Text Text; 
    void Awake()
    {
        Text = _text; 
    }

    public static void UpdateTime(float _time)
    {
        Text.text = _time.ToString(); 
    }
}
