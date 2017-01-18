using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    ParticleSystem _ps;
    bool _reverse;
    float _time; 

	// Use this for initialization
	void Start () {
        _ps = GetComponent<ParticleSystem>();
        _ps.randomSeed = 2;
        _ps.Stop(); 
    }
	
	// Update is called once per frame
	void Update () {
        if(_ps.time > 4 && !_reverse)
        {
            _reverse = true; 
        }
        if(_ps.time < 1 && _reverse)
        {
            _reverse = false; 
        }
        if (_reverse)
        {
            _time -= Time.deltaTime;
        }else
        {
            _time += Time.deltaTime; 
        }
         _ps.Simulate(_time); 
        Debug.Log(_ps.time); 
	}
}
