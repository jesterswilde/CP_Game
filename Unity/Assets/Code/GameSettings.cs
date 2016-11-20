using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    static float _rotThreshold;
    static float _rewindSpeed;
    static float _forwardSpeed; 
    static float[] _speeds;  
    [SerializeField]
    float rotationThreshold;
    [SerializeField]
    float rewindSpeed;
    [SerializeField]
    float forwardSpeed; 
    [SerializeField]
    float[] speeds; 
    public static float RewindSpeed { get { return _rewindSpeed; } }
    public static float RotThreshold { get { return _rotThreshold; } }
    public static float[] Speeds { get { return _speeds; } }
    public static float ForwardSpeed { get { return _forwardSpeed; } }

	void Awake()
    {
        _rotThreshold = rotationThreshold;
        _rewindSpeed = rewindSpeed;
        _speeds = speeds;
        _forwardSpeed = forwardSpeed;
    }
}
