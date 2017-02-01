using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour {

    List<PosSphere> _spheres =  new List<PosSphere>();
    float _timer = 0;
    [SerializeField]
    float timeToChange = 3;
    [SerializeField]
    int numberOfSpheres = 50;
    [SerializeField]
    int gridSize = 10;
    [SerializeField]
    float spiralSize = 5; 
    int _index = 0; 
    public int Index { get { return _index; } }

    static SphereManager t; 
    public static void RegisterSphere(PosSphere _sphere)
    {
        t._spheres.Add(_sphere);
        _sphere.SetPositions(t._index % t.gridSize, Mathf.Floor(t._index / t.gridSize), Mathf.Cos(t.Index) * t.Index, Mathf.Sin(t.Index) * t.Index);
        t._index++; 
    }
	// Use this for initialization
	void Awake() {
        t = this; 
	}
    void Start()
    {
        for(int i = 0; i < numberOfSpheres; i ++) {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.AddComponent<PosSphere>(); 
        }
    }
	
	// Update is called once per frame
	void Update () {
        _timer += Time.deltaTime; 
        if(_timer > timeToChange)
        {
            _timer = 0; 
            for(int i  = 0; i < _spheres.Count; i++)
            {
                _spheres[i].Lineup(); 
            }
        }
	}
}
