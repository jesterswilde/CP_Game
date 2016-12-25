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
        _amountPerHit = Combat.AccuracyLossForAllVitals / _vitalPoints.Count; 
    }
    public void SetVitals(Transform _point)
    {
        _vitalPoints.Add(_point);
        _amountPerHit = Combat.AccuracyLossForAllVitals / _vitalPoints.Count;
    }

    public bool CanSee(Vector3 _gunPos)
    {
        return _vitalPoints.Aggregate(false, (_hasHit, _vital) =>
        {
            Vector3 _dir = _vital.position - _gunPos;
            Ray _ray = new Ray(_gunPos, _dir);
            bool _hit = !Physics.Raycast(_ray, _dir.magnitude, GameSettings.BulletCollLayers);
            return _hit || _hasHit;
        });
    }
    public bool AmountOfBodyExposed(Vector3 _gunPos, out float _accuracyLoss)
    {
        if(_vitalPoints.Count == 0)
        {
            _accuracyLoss = 0; 
            return true; 
        }
        bool _hasHit = false; 
        IEnumerable<bool> _hits = _vitalPoints.Select(_point =>
        {
            Vector3 _dir = _point.position - _gunPos; 
            Ray _ray = new Ray(_gunPos, _dir);
            bool _hit =  !Physics.Raycast(_ray, _dir.magnitude, GameSettings.BulletCollLayers);
            _hasHit = _hit || _hasHit;
            return _hit; 
        });
        _accuracyLoss = _hits.Aggregate(0f, (_sum, _didHit) => _didHit ? _sum + _amountPerHit : _sum);
        return _hasHit; 
    }
}
