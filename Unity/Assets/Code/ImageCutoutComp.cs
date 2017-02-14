using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCutoutComp : MonoBehaviour {

    [SerializeField]
    float _maxDistance;
    Material _mat; 

    void Awake()
    {
        _mat = GetComponent<Renderer>().material; 
    }
    void Update()
    {
        Vector4 _pos = Camera.main.WorldToScreenPoint(transform.position);
        _pos.x = _pos.x / Screen.width - 0.5f;
        _pos.y = _pos.y / Screen.height - 0.5f; 
        float _dist = Mathf.Min((Camera.main.transform.position - transform.position).magnitude, _maxDistance);
        _pos.z = 1 - _dist / _maxDistance;
        _mat.SetVector("_CenterPoint", _pos);
    }


}
