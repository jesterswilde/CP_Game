using UnityEngine;
using System.Collections;
using System;

public class CharacterCam : MonoBehaviour, ICamera
{

    [SerializeField]
    float _rotationSpeed;
    [SerializeField]
    float _camDistance; 
    [SerializeField]
    Transform _cameraSpot;
    [SerializeField]
    Transform _pivotPoint; 
    public Transform CameraSpot { get { return _cameraSpot; } }
    public float XRot { get { return _mouseX; } }
    float _mouseX = 0;
    float _mouseY = 0; 

    public Vector3 CameraForward(float _x)
    {
        float _posX = Convert.ToSingle(Math.Sin(_x * _rotationSpeed / Math.PI / 360));
        float _posZ = Convert.ToSingle(Math.Cos(_x * _rotationSpeed / Math.PI / 360));
        return new Vector3(_posX, 0, _posZ) * -1; 
    }
    void RotateCam()
    {
         _mouseX += Input.GetAxisRaw("Mouse X");
         _mouseY += Input.GetAxisRaw("Mouse Y");
        float _posX = Convert.ToSingle(Math.Sin(_mouseX * _rotationSpeed / Math.PI / 360));
        float _posY = Convert.ToSingle(Math.Sin(_mouseY * _rotationSpeed / Math.PI / 360));
        float _posZ = Convert.ToSingle(Math.Cos(_mouseX * _rotationSpeed / Math.PI / 360));
        _cameraSpot.position = new Vector3(_posX, _posY * -1, _posZ).normalized * _camDistance + _pivotPoint.position;
    }

    public void StartCamera()
    {
        GameCamera.NewController(_cameraSpot, _pivotPoint, true, true); 
        UpdateCamera(); 
    }

    public void ExitCamera()
    {
        float waitTime = GameCamera.Blur();
        Invoke("ExitedCamera", waitTime);
    }
    void ExitedCamera()
    {
        GameManager.ExitedCamera();
    }

    public void UpdateCamera()
    {
        RotateCam();
    }
    void Awake()
    {
        if(_pivotPoint == null)
        {
            _pivotPoint = transform; 
        }
    }
}
