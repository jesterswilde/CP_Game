using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActionLogUI : MonoBehaviour {

    [SerializeField]
    Text[] _textUI;
    //Example call function
    //LogAction("Text" + " (" + GameManager.GameTime.ToString("0.00") + ")");

    /* public void LogAction (string _logText)
     {
         _content2 = _content1;
         _content1 = _content0;
         _content0 = _logText;

         for (int i = 0; i < _a2Text.Length; i++)
         {
             _a2Text[i].text = _content2;
         }

         for (int i = 0; i < _a1Text.Length; i++)
         {
             _a1Text[i].text = _content1;
         }

         for (int i = 0; i < _a0Text.Length; i++)
         {
             _a0Text[i].text = _content0;
         }


         if (_content0 == "0")
         {
             SetLogVisibility(false, _a0Text);
         }
         else
         {
             SetLogVisibility(true, _a0Text);
         }

         if (_content1 == "0")
         {
             SetLogVisibility(false, _a1Text);
         }
         else
         {
             SetLogVisibility(true, _a1Text);
         }

         if(_content2 == "0")
         {
             SetLogVisibility(false, _a2Text);
         }
         else
         {
             SetLogVisibility(true, _a2Text);
         }
     }
 */

    public void SetText(string _content, float _time)
    {
        for (int i = 0; i < _textUI.Length; i++)
        {
            _textUI[i].text = _content + " (" + _time.ToString("0.00") + ")";
    }
    }

    public void SetLogVisibility(bool visibility)
    {
        for (int i = 0; i < _textUI.Length; i++)
        {
            _textUI[i].enabled = visibility;
        }
    }

}