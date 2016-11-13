using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {

    static float _rotThreshold;
    static float _rewindSpeed; 
    [SerializeField]
    float rotationThreshold;
    [SerializeField]
    float rewindSpeed; 
    public static float RewindSpeed { get { return _rewindSpeed; } }
    public static float RotThreshold { get { return _rotThreshold; } }

	void Awake()
    {
        _rotThreshold = rotationThreshold;
        _rewindSpeed = rewindSpeed; 
    }
}
