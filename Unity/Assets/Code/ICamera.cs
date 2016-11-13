using UnityEngine;
using System.Collections;

public interface ICamera {

    void StartCamera();
    void ExitCamera();
    void UpdateCamera(); 
}

public enum CameraTypes
{
    Character,
    Observer
}
