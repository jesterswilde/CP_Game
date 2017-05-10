using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCone : MonoBehaviour {

    Projector _projector;
    [SerializeField]
    Shader _shader;
    Material _mat; 

	// Use this for initialization
	void Start () {
        _projector = GetComponent<Projector>();
        //_projector.material = new Material(_shader);
        _mat = _projector.material;  
	}

    // Update is called once per frame
    void Update()
    {
        _mat.SetVector("_Normal", transform.forward);
    }
}
