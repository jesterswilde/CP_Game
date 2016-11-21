using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    static float _rewindSpeed;
    static float _forwardSpeed; 
    static float[] _speeds;
    static float _minJumpDuration;
    static float _maxJumpSpeed;   
    [SerializeField]
    float rewindSpeed = -5;
    [SerializeField]
    float forwardSpeed = 1; 
    [SerializeField]
    float[] speeds;
    [SerializeField]
    float minJumpDuration = 1;
    [SerializeField]
    float maxJumpSpeed = 15; 
    public static float RewindSpeed { get { return _rewindSpeed; } }
    public static float[] Speeds { get { return _speeds; } }
    public static float ForwardSpeed { get { return _forwardSpeed; } }
    public static float MinJumpDuration { get { return _minJumpDuration; } }
    public static float MaxJumpSpeed { get { return _maxJumpSpeed; } }

	void Awake()
    {
        _rewindSpeed = rewindSpeed;
        _speeds = speeds;
        _forwardSpeed = forwardSpeed;
        _maxJumpSpeed = maxJumpSpeed;
        _minJumpDuration = minJumpDuration;
    }
}
