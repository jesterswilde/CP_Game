using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine.SceneManagement; 

[RequireComponent(typeof(GameSettings))]
[RequireComponent(typeof(Combat))]
public class GameManager : MonoBehaviour {

    [SerializeField]
    LayerMask collMask;

	static GameManager t; 
    LayerMask _collMask;
    public static LayerMask CollMask { get { return t._collMask; } }
    float _totalUpdateTime = 0;
    float _totalFixedTime = 0;
    float _gameSpeed = 1;
    float _fixedGameTime = 0; 
    float _jumpDuration = 1;
	float _gameTime; 
    public static float JumpDuration { get { return t._jumpDuration; } }
    public static float GameSpeed { get { return t._gameSpeed; } }
	public static float GameTime { get { return t._gameTime; } }
    public static float TotalFixedTime { get { return t._totalFixedTime; } }
    public static float FixedGameTime { get { return t._fixedGameTime; } }
    Character _activeCharacter;
    public static Character ActiveCharacter { get { return t._activeCharacter; } }
    List<Character> _characters = new List<Character>();
    public static List<Character> Characters { get { return t._characters; } } 
    int _characterIndex = 0;
    List<WibblyWobbly> _timeyWimeys = new List<WibblyWobbly>();
    List<ITargetable> _targetables = new List<ITargetable>();
    List<IManager> _managers = new List<IManager>(); 
    [SerializeField]
    ObserverCam ob;
    ObserverCam _obCam;
    bool _isReplaying = false; 
    public static bool IsReplaying { get { return t._isReplaying; } }
    bool _isPlaying = true; 
    public static bool IsPlaying { get { return t._isPlaying; } }
    bool _isPaused = false; 
    public static bool IsPaused { get { return t._isPaused; } }
    callback _jumpCB; 
    bool _showingCtrlMenu = false; 
    public static bool ShowingCtrlMenu { get { return t._showingCtrlMenu; } }
    bool _canAcceptPlayerInput = true; 
    public static bool CanAcceptPlayerInput { get { return t._canAcceptPlayerInput; } }

    ICamera _camController;
    ICamera _nextCamController;

    #region Camera
    public static ITargetable GetTargeted(float _threshold)
    {
        Ray _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
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
        if(t._camController != null)
        {
            t._nextCamController = _next; 
            t._camController.ExitCamera();
        }else
        {
            t._camController = _next;  
            t._camController.StartCamera();
        }
    }
    public static void ExitedCamera()
    {
        t._camController = t._nextCamController;
        t._camController.StartCamera(); 
    }
    static void StartCamera()
    {
        t._camController.StartCamera(); 
    }
    #endregion

    #region Time
    public static void SetSpeed(float _speed)
    {
        t._gameSpeed = _speed; 
        if(t._gameSpeed > 0)
        {
            if(t._activeCharacter != null && !t._isPlaying)
            {
                t._activeCharacter.SetStateToKeyboard(); 
            }
            t._isPlaying = true;
            t._isPaused = false; 
        }
        if(t._gameSpeed < 0)
        {
            if(t._activeCharacter != null && t._isPlaying)
            {
                t._activeCharacter.ClearState(); 
            }
            t._isPaused = false;
            t._isPlaying = false; 
        }
        if(t._gameSpeed == 0)
        {
            if (t._activeCharacter != null && t._isPlaying)
            {
                t._activeCharacter.ClearState();
            }
            t._isPaused = true;
            t._isPlaying = false;
        }
    }
    public static void ShowCtrlMenu(bool _show)
    {
        if (t._canAcceptPlayerInput)
        {
            if (_show)
            {
                SetSpeed(0);
            }
            else
            {
                SetSpeed(TimeCounter.Speed); 
            }
            t._showingCtrlMenu = _show;
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
        t._activeCharacter.SetAction(new BasicAction(ActionType.Null)); 
        TimeysApplyActions(); 
        float _timeGap = _time - GameManager.GameTime;
        int _dir = (_timeGap >= 0) ? 1 : -1;
        float _jumpSpeed = Mathf.Clamp(Mathf.Abs(_timeGap / GameSettings.MinJumpDuration), GameSettings.MinJumpDuration, GameSettings.MaxJumpSpeed) * _dir;
        t._jumpDuration = _timeGap / _jumpSpeed;
        SetSpeed(_jumpSpeed);
        t._canAcceptPlayerInput = false; 
        GameManager.t.StartJump(t._jumpDuration);
    }
    public static void JumpToTime(float _time, callback _cb)
    {
        JumpToTime(_time);
        t._jumpCB = _cb; 
    }
    void StartJump(float _duration)
    {
        Invoke("EndJump", _duration);
    }
    void EndJump()
    {
        t._gameTime = _fixedGameTime; 
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
                t._activeCharacter != null && t._activeCharacter.IsPastMostRecentAction())
            {
                SetSpeed(0); 
            }
        }else
        {
            if(t._activeCharacter != null && t._activeCharacter.IsPastMostRecentAction() && t._canAcceptPlayerInput &&
                PlayerInput.GetAbsKeyboardState().Aggregate((result, current) => current || result))
            {
                SetSpeed(GameSettings.ForwardSpeed); 
            }
        }
    }
    static void Play()
    {
        if(GameSettings.MaxLevelTime > 0 && t._fixedGameTime > GameSettings.MaxLevelTime)
        {
            SetSpeed(0);
            return; 
        }
        foreach(WibblyWobbly _character in t._timeyWimeys)
        {
            _character.Play(t._fixedGameTime); 
        }
        EffectsManager.Play(t._fixedGameTime); 
    }
    static void Rewind()
    {
        if(t._fixedGameTime < 0)
        {
            t._fixedGameTime = 0;
			SetSpeed (0); 
        }
        foreach(WibblyWobbly _character in t._timeyWimeys)
        {
            _character.Rewind(t._fixedGameTime); 
        }
        EffectsManager.Play(t._fixedGameTime); 
    }
    #endregion

    #region Character
    public static void SwitchCharacter()
    {
        t._characterIndex++; 
        if(t._characterIndex >= t._characters.Count)
        {
            t._characterIndex = 0; 
        }
        Character _nextCharacter = t._characters[t._characterIndex];
        JumpToTime(_nextCharacter.GetHeadTimestamp()); 
        SetActiveCharacter(_nextCharacter);
        TimeCounter.SwitchedToCharacter(_nextCharacter); 
    }
    public static void SetActiveCharacter(int index)
    {
        t._characterIndex = index;
        SetActiveCharacter(t._characters[index]); 
    }
    public static void SetActiveCharacter(Character character)
    {
        if(t._activeCharacter != null)
        {
            t._activeCharacter.SetAsInactivePlayer(); 
        }
        t._activeCharacter = character;
        if(t._activeCharacter != null)
        {
            t._activeCharacter.SetAsActivePlayer(); 
            SwitchCamera(t._activeCharacter.Cam); 
        }
    }

    public static void RegisterCharacter(Character _character)
    {
        t._characters.Add(_character); 
    }
    public static void UnRegisterCharacter(Character _character)
    {
        t._characters.Remove(_character); 
    }
    public static void ClearCharacters()
    {
        t._characters.Clear(); 
    }
    public static void RegisterWibblyWobbly(WibblyWobbly _timey)
    {
        t._timeyWimeys.Add(_timey); 
    }
    public static void UnRegisterWibblyWobbly(WibblyWobbly _timey)
    {
        t._timeyWimeys.Remove(_timey); 
    }
    public static void RegisterTargetable(ITargetable _target)
    {
        t._targetables.Add(_target); 
    }
    public static void UnRegisterTargetable(ITargetable _target)
    {
        t._targetables.Remove(_target); 
    }
    public static void RegisterManager(IManager _manager)
    {
        t._managers.Add(_manager); 
    }
    public static void UnRegisterManager(IManager _manager)
    {
        t._managers.Remove(_manager); 
    }
    public static void TimeysApplyActions()
    {
        for(int i = 0; i < t._timeyWimeys.Count; i++)
        {
            t._timeyWimeys[i].ApplyActions(); 
        }
    }
    #endregion

    
    public static float GetGameTimeFixed()
    {
        return t._gameTime + t._totalFixedTime - t._totalUpdateTime; 
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
	static bool _loadLevel;
	static string _levelToLoad;
	public static void StartLoadingLevel(string _levelName){
		_loadLevel = true; 
		_levelToLoad = _levelName; 
	}
	public static void LoadNextLevel(){
		t.CancelInvoke (); 
		SceneManager.LoadSceneAsync (_levelToLoad); 
		_loadLevel = false; 
		_levelToLoad = ""; 
	}
    // Update is called once per frame
    void Update () {
		ShouldPause (); 
		_totalUpdateTime += Time.deltaTime;
		_gameTime = _fixedGameTime;
		for (int i = 0; i < _managers.Count; i++) {
			_managers [i].UpdateManager (_gameTime, Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.Backspace)) {
			Observe ();
		}
		if (_activeCharacter != null) {
			_activeCharacter.Target (); 
		}
		if (_canAcceptPlayerInput) {
			PlayerInput.CheckSpeedButtons (); 
		}
		if (_canAcceptPlayerInput && _activeCharacter != null && _isPlaying) {
			_activeCharacter.FaceCamera (); 
			_activeCharacter.SetStateToKeyboard ();
			_activeCharacter.SetAction (PlayerInput.ActionButtons (_activeCharacter)); 
			_activeCharacter.ApplyActions (); 
		}
		foreach (WibblyWobbly _timey in _timeyWimeys) {
			if (_gameSpeed >= 0) {
				_timey.PlayVisuals (_gameTime);
			} else {
				_timey.RewindVisuals (_gameTime); 
			}
		}
		TimeCounter.UpdateTime (_fixedGameTime);
		_camController.UpdateCamera ();
		UpdateFixedTimestep ();
		if (_loadLevel) {
			LoadNextLevel (); 
		}
    }
    
    void FixedUpdate()
    {
		TimeysApplyActions ();
		if (!_isPaused) {
			_totalFixedTime += GameSettings.FixedTimestep;
			int _dir = (_gameSpeed >= 0) ? 1 : -1; 
			_fixedGameTime += GameSettings.FixedTimestep * _dir;
			_fixedGameTime = Mathf.Max (_fixedGameTime, 0); 
		}
		for (int i = 0; i < _managers.Count; i++) {
			_managers [i].FixedUpdateManager (_fixedGameTime, Time.fixedDeltaTime); 
		}
		if (_isPlaying && !_isPaused) {
			Play ();
		}
		if (!_isPaused && !_isPlaying) {
			Rewind ();
		}
        
    }
    void Awake()
    {
		if (t != null) {
			Destroy (t); 
		}
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
