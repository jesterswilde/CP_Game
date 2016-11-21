using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    static float _rewindSpeed;
    static float _forwardSpeed; 
    static float[] _speeds;
    static float _timeJumpDuration;  
    [SerializeField]
    float rewindSpeed;
    [SerializeField]
    float forwardSpeed; 
    [SerializeField]
    float[] speeds;
    [SerializeField]
    float timeJumpDuration; 
    public static float RewindSpeed { get { return _rewindSpeed; } }
    public static float[] Speeds { get { return _speeds; } }
    public static float ForwardSpeed { get { return _forwardSpeed; } }
    public static float TimeJumpDuration { get { return _timeJumpDuration; } }

	void Awake()
    {
        _rewindSpeed = rewindSpeed;
        _speeds = speeds;
        _forwardSpeed = forwardSpeed;
        _timeJumpDuration = timeJumpDuration; 
    }
}
