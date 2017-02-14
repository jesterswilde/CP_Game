using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq; 

[RequireComponent(typeof(GameSettings))]
[RequireComponent(typeof(Combat))]
public class GameManager : MonoBehaviour {

    [SerializeField]
    LayerMask collMask;
    static LayerMask _collMask;
    public static LayerMask CollMask { get { return _collMask; } }
    static GameManager t; 
    static float _gameTime = 0;
    static float _totalUpdateTime = 0;
    static float _totalFixedTime = 0;
    static float _gameSpeed = 1;
    static float _fixedGameTime = 0; 
    static float _jumpDuration = 1; 
    public static float JumpDuration { get { return _jumpDuration; } }
    public static float GameSpeed { get { return _gameSpeed; } }
    public static float GameTime { get { return _gameTime; } }
    public static float TotalFixedTime { get { return _totalFixedTime; } }
    public static float FixedGameTime { get { return _fixedGameTime; } }
    static Character _activeCharacter;
    public static Character ActiveCharacter { get { return _activeCharacter; } }
    static List<Character> _characters = new List<Character>();
    public static List<Character> Characters { get { return _characters; } } 
    static int _characterIndex = 0;
    static List<WibblyWobbly> _timeyWimeys = new List<WibblyWobbly>();
    static List<ITargetable> _targetables = new List<ITargetable>();
    static List<IManager> _managers = new List<IManager>(); 
    [SerializeField]
    ObserverCam ob;
    static ObserverCam _obCam;
    static bool _isReplaying = false; 
    public static bool IsReplaying { get { return _isReplaying; } }
    static bool _isPlaying = true; 
    public static bool IsPlaying { get { return _isPlaying; } }
    static bool _isPaused = false; 
    public static bool IsPaused { get { return _isPaused; } }
    static callback _jumpCB; 
    static bool _showingCtrlMenu = false; 
    public static bool ShowingCtrlMenu { get { return _showingCtrlMenu; } }
    static bool _canAcceptPlayerInput = true; 
    public static bool CanAcceptPlayerInput { get { return _canAcceptPlayerInput; } }

    static ICamera _camController;
    static ICamera _nextCamController;

    #region Camera
    public static ITargetable GetTargeted(float _threshold)
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit; 
        if(Physics.Raycast(_ray, out _hit,GameSettings.MaxTargetDistance, GameSettings.TargetMask)){
            GameObject _go = _hit.collider.gameObject;
            Component[] _comps = _go.GetComponents<Component>();
            foreach(Component _comp in _comps)
            {
                if(_comp is ITargetable)
                {
                    return _comp as ITargetable; 
                }
            }
        }
        return null; 
    }
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

    #region Time
    public static void SetSpeed(float _speed)
    {
        _gameSpeed = _speed; 
        if(_gameSpeed > 0)
        {
            if(_activeCharacter != null && !_isPlaying)
            {
                _activeCharacter.SetStateToKeyboard(); 
            }
            _isPlaying = true;
            _isPaused = false; 
        }
        if(_gameSpeed < 0)
        {
            if(_activeCharacter != null && _isPlaying)
            {
                _activeCharacter.ClearState(); 
            }
            _isPaused = false;
            _isPlaying = false; 
        }
        if(_gameSpeed == 0)
        {
            if (_activeCharacter != null && _isPlaying)
            {
                _activeCharacter.ClearState();
            }
            _isPaused = true;
            _isPlaying = false;
        }
    }
    public static void ShowCtrlMenu(bool _show)
    {
        if (_canAcceptPlayerInput)
        {
            if (_show)
            {
                SetSpeed(0);
            }
            else
            {
                SetSpeed(TimeCounter.Speed); 
            }
            _showingCtrlMenu = _show;
        }
    }
    void UpdateFixedTimestep()
    {
        if (_gameSpeed == 0)
        {
            Time.fixedDeltaTime = 0.02f;
        }
        else
        {
            Time.fixedDeltaTime = GameSettings.FixedTimestep / Mathf.Abs(_gameSpeed);
        }
    }

    public static void JumpToTime(float _time)
    {
        _activeCharacter.SetAction(new BasicAction(ActionType.Null)); 
        TimeysApplyActions(); 
        float _timeGap = _time - GameManager.GameTime;
        int _dir = (_timeGap >= 0) ? 1 : -1;
        float _jumpSpeed = Mathf.Clamp(Mathf.Abs(_timeGap / GameSettings.MinJumpDuration), GameSettings.MinJumpDuration, GameSettings.MaxJumpSpeed) * _dir;
        _jumpDuration = _timeGap / _jumpSpeed;
        SetSpeed(_jumpSpeed);
        _canAcceptPlayerInput = false;
        Debug.Log("Jumping " + _jumpSpeed + " | " + _jumpDuration); 
        GameManager.t.StartJump(_jumpDuration);
    }
    public static void JumpToTime(float _time, callback _cb)
    {
        JumpToTime(_time);
        _jumpCB = _cb; 
    }
    void StartJump(float _duration)
    {
        Invoke("EndJump", _duration);
    }
    void EndJump()
    {
        _gameTime = _fixedGameTime; 
        SetSpeed(0);
        _canAcceptPlayerInput = true;
        if(_jumpCB != null)
        {
            _jumpCB();
            _jumpCB = null; 
        }
    }
    static void ShouldPause()
    {
        if (GameSpeed > 0 && !Input.GetKey(KeyCode.Space))
        {
            if(!PlayerInput.GetAbsKeyboardState().Aggregate((result, current) => current || result) &&
                _activeCharacter != null && _activeCharacter.IsPastMostRecentAction())
            {
                SetSpeed(0); 
            }
        }else
        {
            if(_activeCharacter != null && _activeCharacter.IsPastMostRecentAction() && _canAcceptPlayerInput &&
                PlayerInput.GetAbsKeyboardState().Aggregate((result, current) => current || result))
            {
                SetSpeed(GameSettings.ForwardSpeed); 
            }
        }
    }
    static void Play(float _time)
    {
        foreach(WibblyWobbly _character in _timeyWimeys)
        {
            _character.Play(_time); 
        }
        EffectsManager.Play(_time); 
    }
    static void Rewind(float _time)
    {
        if(_gameTime < 0)
        {
            _gameTime = 0; 
        }
        foreach(WibblyWobbly _character in _timeyWimeys)
        {
            _character.Rewind(_time); 
        }
        EffectsManager.Play(_time); 
    }
    #endregion

    #region Character
    public static void SwitchCharacter()
    {
        _characterIndex++; 
        if(_characterIndex >= _characters.Count)
        {
            _characterIndex = 0; 
        }
        Character _nextCharacter = _characters[_characterIndex];
        JumpToTime(_nextCharacter.GetHeadTimestamp()); 
        SetActiveCharacter(_nextCharacter);
        TimeCounter.SwitchedToCharacter(_nextCharacter); 
    }
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
    public static void UnRegisterCharacter(Character _character)
    {
        _characters.Remove(_character); 
    }
    public static void ClearCharacters()
    {
        _characters.Clear(); 
    }
    public static void RegisterWibblyWobbly(WibblyWobbly _timey)
    {
        _timeyWimeys.Add(_timey); 
    }
    public static void UnRegisterWibblyWobbly(WibblyWobbly _timey)
    {
        _timeyWimeys.Remove(_timey); 
    }
    public static void RegisterTargetable(ITargetable _target)
    {
        _targetables.Add(_target); 
    }
    public static void UnRegisterTargetable(ITargetable _target)
    {
        _targetables.Remove(_target); 
    }
    public static void RegisterManager(IManager _manager)
    {
        _managers.Add(_manager); 
    }
    public static void UnRegisterManager(IManager _manager)
    {
        _managers.Remove(_manager); 
    }
    public static void TimeysApplyActions()
    {
        for(int i = 0; i < _timeyWimeys.Count; i++)
        {
            _timeyWimeys[i].ApplyActions(); 
        }
    }
    #endregion

    
    public static float GetGameTimeFixed()
    {
        return _gameTime + _totalFixedTime - _totalUpdateTime; 
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
        ShouldPause(); 
        _totalUpdateTime += Time.deltaTime;
        _gameTime = _fixedGameTime;
        for (int i = 0; i < _managers.Count; i++)
        {
            _managers[i].UpdateManager(_gameTime, Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Observe();
        }
        if (_canAcceptPlayerInput)
        {
            PlayerInput.CheckSpeedButtons(); 
        }
        if (_canAcceptPlayerInput && _activeCharacter != null && _isPlaying)
        {
            _activeCharacter.FaceCamrea(); 
            _activeCharacter.SetStateToKeyboard();
            _activeCharacter.SetAction(PlayerInput.ActionButtons(_activeCharacter)); 
            _activeCharacter.ApplyActions(); 
        }
        TimeCounter.UpdateTime(_fixedGameTime);
        _camController.UpdateCamera();
        UpdateFixedTimestep(); 
    }
    
    void FixedUpdate()
    {
        TimeysApplyActions();
        if (!_isPaused)
        {
            _totalFixedTime += GameSettings.FixedTimestep;
            int _dir = (_gameSpeed >= 0) ? 1 : -1; 
            _fixedGameTime += GameSettings.FixedTimestep * _dir;
            _fixedGameTime = Mathf.Max(_fixedGameTime, 0); 
        }
        for(int i = 0; i < _managers.Count; i++)
        {
            _managers[i].FixedUpdateManager(_fixedGameTime, Time.fixedDeltaTime); 
        }
        if (_isPlaying && !_isPaused)
        {
            Play(_fixedGameTime);
        }
        if (!_isPaused && !_isPlaying)
        {
            Rewind(_fixedGameTime);
        }
        
    }
    void Awake()
    {
        t = this; 
        _obCam = ob;
        _collMask = collMask;
        SRand.Awake(); 
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        SetSpeed(GameSettings.ForwardSpeed); 
        SetActiveCharacter(_characters[_characterIndex]);
        TimeCounter.SwitchedToCharacter(_characters[_characterIndex]);
        for (int i = 0; i < _managers.Count; i++)
        {
            _managers[i].StartManager();
        }
    }
}
