using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.Rotate(new Vector3(0, Util.AngleBetweenVector3(transform.forward, transform.right, transform.up), 0)); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
