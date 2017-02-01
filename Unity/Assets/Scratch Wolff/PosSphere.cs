using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosSphere : MonoBehaviour {

    float _xPosGrid;
    float _yPosGrid;
    float _xPosSpiral;
    float _yPosSPiral;
    bool isGrid = false; 

    public void SetPositions(float _xGrid, float _yGrid, float _xSpiral, float _ySpiral)
    {
        _xPosGrid = _xGrid;
        _yPosGrid = _yGrid;
        _xPosSpiral = _xSpiral;
        _yPosSPiral = _ySpiral;
    }
    public void Lineup()
    {
        isGrid = !isGrid;
        if (isGrid)
        {
            transform.position = new Vector3(_xPosGrid, _yPosGrid, 0);
        }else
        {
            transform.position = new Vector3(_xPosSpiral, _yPosSPiral, 0); 
        }
    }
    void Start()
    {
        SphereManager.RegisterSphere(this); 
    }
}
