using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    static float _rewindSpeed;
    static float _forwardSpeed; 
    static float[] _speeds;
    static float _minJumpDuration;
    static float _maxJumpSpeed;
    static float _fixedTimestep;
    static LayerMask _bulletCollLayers; 
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
    [SerializeField]
    float fixedTimstep = 0.02f;
    [SerializeField]
    LayerMask bulletCollLayers;
    public static float RewindSpeed { get { return _rewindSpeed; } }
    public static float[] Speeds { get { return _speeds; } }
    public static float ForwardSpeed { get { return _forwardSpeed; } }
    public static float MinJumpDuration { get { return _minJumpDuration; } }
    public static float MaxJumpSpeed { get { return _maxJumpSpeed; } }
    public static float FixedTimestep { get { return _fixedTimestep; } }
    public static LayerMask BulletCollLayers { get { return _bulletCollLayers; } }

	void Awake()
    {
        _rewindSpeed = rewindSpeed;
        _speeds = speeds;
        _forwardSpeed = forwardSpeed;
        _maxJumpSpeed = maxJumpSpeed;
        _minJumpDuration = minJumpDuration;
        _fixedTimestep = fixedTimstep;
        _bulletCollLayers = bulletCollLayers; 
    }
}
