using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    static float _gameTime = 0;
    public static float GameTime { get { return _gameTime; } }
    static Character _activeCharacter;
    public static Character ActiveCharacter { get { return _activeCharacter; } }
    [SerializeField]
    static List<Character> _characters = new List<Character>();
    static int _characterIndex = 0;
    [SerializeField]
    ObserverCam ob;
    static ObserverCam _obCam;
    static bool _isPlaying = true; 

    static ICamera _camController;
    static ICamera _nextCamController;

    #region Camera
    static void SwitchCamera(ICamera _next)
    {
        if(_camController != null)
        {
            _nextCamController = _next; 
            _camController.ExitCamera();
        }else
        {
            _camController = _next;  
            _camController.StartCamera();
        }
    }
    public static void ExitedCamera()
    {
        Debug.Log("exited"); 
        _camController = _nextCamController;
        _camController.StartCamera(); 
    }
    static void StartCamera()
    {
        _camController.StartCamera(); 
    }
    #endregion


    static void RewindAbs(int _subtract)
    {
        _gameTime = Mathf.Max(0, _gameTime - _subtract); 
        foreach(Character _character in _characters)
        {
            _character.RewindToTime(); 
        }
    }
    static void BeginPlaying()
    {
        _isPlaying = true; 
    }
    static void BeginRewinding()
    {
        _isPlaying = false;
    }
    static void Play(float _speed)
    {
        _gameTime += _speed * Time.deltaTime; 
        foreach(Character _character in _characters)
        {
            _character.Play(); 
        }
    }
    static void StartRewinding()
    {

    }
    static void Rewind(float _speed)
    {
        _isPlaying = false; 
        _gameTime -= _speed * Time.deltaTime; 
        if(_gameTime < 0)
        {
            _gameTime = 0; 
        }
        foreach(Character _character in _characters)
        {
            _character.Rewind(); 
        }
    }

    #region Character
    public static void SetActiveCharacter(int index)
    {
        _characterIndex = index;
        SetActiveCharacter(_characters[index]); 
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
            SwitchCamera(_activeCharacter.Cam); 
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
    #endregion
    static void SetActions()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            BeginRewinding(); 
        }
        if (Input.GetMouseButtonUp(0))
        {
            BeginPlaying();  
        }
        if (Input.GetMouseButtonDown(1))
        {
            RewindAbs(1); 
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter(); 
        }
        if(_activeCharacter != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressForward));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressLeft));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressBack));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _activeCharacter.SetAction(new Action(ActionType.PressRight));
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseForward));
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseLeft));
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseBack));
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                _activeCharacter.SetAction(new Action(ActionType.ReleaseRight));
            }
            if (_activeCharacter.WillRotate())
            {
                _activeCharacter.SetAction(new ValueAction(ActionType.Rotation, _activeCharacter.RotationDifference()));
            }
        }
    }

    void Observe()
    {
        if(_activeCharacter == null)
        {
            SetActiveCharacter(_characters[0]);
        }else
        {
            SwitchCamera(_obCam); 
            SetActiveCharacter(null);
        }
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Observe();
        }
        SetActions();
        if (_isPlaying)
        {
            Play(1);
        }else
        {
            Rewind(GameSettings.RewindSpeed); 
        }
        TimeCounter.UpdateTime(_gameTime);
        _camController.UpdateCamera();
	}
    void Awake()
    {
        _obCam = ob; 
    }
    void Start()
    {
        SetActiveCharacter(_characters[_characterIndex]);
        BeginPlaying(); 
    }


}
