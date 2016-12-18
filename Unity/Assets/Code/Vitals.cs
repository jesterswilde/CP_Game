using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 

public class Vitals  {
    List<Transform> _vitalPoints = new List<Transform>();
    float _amountPerHit; 

    public void SetVitals(Transform[] _points)
    {
        _vitalPoints.AddRange(_points);
        _amountPerHit = 1 / _vitalPoints.Count; 
    }
    public void SetVitals(Transform _point)
    {
        _vitalPoints.Add(_point); 
    }
    public float AmountOfBodyExposed(Vector3 _gunPos)
    {
        if(_vitalPoints.Count == 0)
        {
            return 0; 
        }
        IEnumerable<bool> _hits = _vitalPoints.Select(_point =>
        {
            Vector3 _dir = _point.position - _gunPos; 
            Ray _ray = new Ray(_gunPos, _dir);
            return !Physics.Raycast(_ray, _dir.magnitude, GameSettings.BulletCollLayers);
        });
        return _hits.Aggregate(0f, (_sum, _didHit) => _didHit ? _sum + _amountPerHit : _sum);
    }

}
