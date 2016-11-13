using UnityEngine;
using System.Collections;
using System;

public class ObserverCam : MonoBehaviour, ICamera
{

    public void ExitCamera()
    {
        float waitTime = GameCamera.Blur();
        Invoke("ExitedCamera", waitTime);
    }
    void ExitedCamera()
    {
        GameManager.ExitedCamera();
    }

    public void StartCamera()
    {
        GameCamera.NewController(transform, true); 
        UpdateCamera(); 
    }

    public void UpdateCamera()
    {
        
    }
}
