using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    static float _gameTime = 0;
    static float _updateTime = 0;
    static float _fixedTime = 0;
    static float _gameSpeed = 1; 
    public static float GameSpeed { get { return _gameSpeed; } }
    public static float GameTime { get { return _gameTime; } }
    static Character _activeCharacter;
    public static Character ActiveCharacter { get { return _activeCharacter; } }
    [SerializeField]
    static List<Character> _characters = new List<Character>();
    static int _characterIndex = 0;
    static List<WibblyWobbly> _timeyWimey = new List<WibblyWobbly>(); 
    [SerializeField]
    ObserverCam ob;
    static ObserverCam _obCam;
    static bool _isPlaying = true; 
    public static bool IsPlaying { get { return _isPlaying; } }
    static bool _isPaused = false; 
    public static bool IsPaused { get { return _isPaused; } }

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
        _camController = _nextCamController;
        _camController.StartCamera(); 
    }
    static void StartCamera()
    {
        _camController.StartCamera(); 
    }
    #endregion

    static void SetSpeed(float _speed)
    {
        _gameSpeed = _speed; 
        if(_gameSpeed > 0)
        {
            _isPlaying = true;
            _isPaused = false; 
        }
        if(_gameSpeed < 0)
        {
            _isPaused = false;
            _isPlaying = false; 
        }
        if(_gameSpeed == 0)
        {
            _isPaused = true;
            _isPlaying = false; 
        }
    }
    static void Play(float _speed)
    {
        _gameTime += _speed * Time.deltaTime; 
        foreach(WibblyWobbly _character in _timeyWimey)
        {
            _character.Play(); 
        }
    }
    static void Rewind(float _speed)
    {
        _gameTime -= _speed * Time.deltaTime; 
        if(_gameTime < 0)
        {
            _gameTime = 0; 
        }
        foreach(WibblyWobbly _character in _timeyWimey)
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
    public static void RegisterWibblyWobbly(WibblyWobbly _timey)
    {
        _timeyWimey.Add(_timey); 
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SetSpeed(0); 
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            SetSpeed(TimeCounter.Speed); 
        }
        if (Input.GetMouseButtonDown(0))
        {
            SetSpeed(GameSettings.RewindSpeed); 
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetSpeed(GameSettings.ForwardSpeed); 
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
    public static float GetGameTimeFixed()
    {
        return _gameTime + _fixedTime - _updateTime; 
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
        _updateTime += Time.deltaTime; 
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Observe();
        }
        Debug.Log(_gameSpeed + " | " + _isPlaying + "  | " + _isPaused); 
        SetActions();
        if (_isPlaying && !_isPaused)
        {
            Play(_gameSpeed);
        }if (!_isPaused && !_isPlaying)
        {
            Rewind(_gameSpeed * -1); 
        }
        TimeCounter.UpdateTime(_gameTime);
        _camController.UpdateCamera();
    }
    void FixedUpdate()
    {
        _fixedTime += Time.fixedDeltaTime; 
    }
    void Awake()
    {
        _obCam = ob; 
    }
    void Start()
    {
        SetSpeed(GameSettings.ForwardSpeed); 
        SetActiveCharacter(_characters[_characterIndex]);
    }


}
