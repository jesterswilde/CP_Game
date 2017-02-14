using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    static float _rewindSpeed;
    static float _forwardSpeed; 
    static float[] _speeds;
    static float _minJumpDuration;
    static float _maxJumpSpeed;
    static float _fixedTimestep;
    static float _maxTargetDistance; 
    static LayerMask _bulletCollLayers;
    static Material _inactiveMaterial;
    static LayerMask _targetMask;
    static int _maxLevelTime; 
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
    [SerializeField]
    float maxTargetDistance = 0.1f;
    [SerializeField]
    Material inactiveMaterial;
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    int maxLevelTime; 
    public static float RewindSpeed { get { return _rewindSpeed; } }
    public static float[] Speeds { get { return _speeds; } }
    public static float ForwardSpeed { get { return _forwardSpeed; } }
    public static float MinJumpDuration { get { return _minJumpDuration; } }
    public static float MaxJumpSpeed { get { return _maxJumpSpeed; } }
    public static float FixedTimestep { get { return _fixedTimestep; } }
    public static LayerMask BulletCollLayers { get { return _bulletCollLayers; } }
    public static float MaxTargetDistance { get { return _maxTargetDistance; } }
    public static Material InactiveMaterial { get { return _inactiveMaterial; } }
    public static LayerMask TargetMask { get { return _targetMask; } }
    public static int MaxLevelTime { get { return _maxLevelTime; } }

	void Awake()
    {
        _maxLevelTime = maxLevelTime; 
        _targetMask = targetMask; 
        _rewindSpeed = rewindSpeed;
        _speeds = speeds;
        _forwardSpeed = forwardSpeed;
        _maxJumpSpeed = maxJumpSpeed;
        _minJumpDuration = minJumpDuration;
        _fixedTimestep = fixedTimstep;
        _bulletCollLayers = bulletCollLayers;
        _maxTargetDistance = maxTargetDistance;
        _inactiveMaterial = inactiveMaterial; 
    }
}
