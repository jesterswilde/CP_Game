using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostGraidentBG : MonoBehaviour {

    Material _mat;
    float _depth;
    [SerializeField]
    Color _topColor;
    [SerializeField]
    Color _bottomColor; 

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        if (_mat == null)
        {
            _mat = new Material(Shader.Find("Hidden/BGGradient"));
        }
        _mat.SetColor("_Color1", _topColor);
        _mat.SetColor("_Color2", _bottomColor); 
    }

    void OnRenderImage(RenderTexture _source, RenderTexture _destination)
    {
        Graphics.Blit(_source, _destination, _mat); 
    }
}
