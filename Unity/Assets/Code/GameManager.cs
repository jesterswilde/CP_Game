using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic; 

public class GameManager : MonoBehaviour {

    static float _gameTime = 0;
    public static float GameTime { get { return _gameTime; } }
    static Character _activeCharacter;
    [SerializeField]
    static List<Character> _characters = new List<Character>();
    static int _characterIndex = 0;


    static void Rewind(int subtract)
    {
        _gameTime = Mathf.Max(0, _gameTime - subtract); 
        foreach(Character _character in _characters)
        {
            _character.Rewind(); 
        }
    }

    public static void SetActiveCharacter(Character character)
    {
        if(_activeCharacter != null)
        {
            _activeCharacter.SetAsInactivePlayer(); 
        }
        _activeCharacter = character;
        if(_activeCharacter != null)
        {
            _activeCharacter.SetAsActivePlayer(); 
        }
    }

    public static void RegisterCharacter(Character _character)
    {
        _characters.Add(_character); 
    }
    static void SwitchCharacter()
    {
        _characterIndex++; 
        if(_characterIndex >= _characters.Count)
        {
            _characterIndex = 0; 
        }
        SetActiveCharacter(_characters[_characterIndex]);
    }

    static void SetActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Rewind(10); 
        }
        if (Input.GetMouseButtonDown(1))
        {
            Rewind(1); 
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter(); 
        }
        if(_activeCharacter != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressForward, _activeCharacter));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressLeft, _activeCharacter));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressBack, _activeCharacter));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressRight, _activeCharacter));
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseForward, _activeCharacter));
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseLeft, _activeCharacter));
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseBack, _activeCharacter));
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseRight, _activeCharacter));
            }
        }
    }

	// Update is called once per frame
	void Update () {
        _gameTime += Time.deltaTime;
        SetActions();
        TimeCounter.UpdateTime(_gameTime); 
	}
    void Start()
    {
        SetActiveCharacter(_characters[_characterIndex]); 
    }
    
}
