﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class Util {

    public static float AngleBetweenVector3(Vector3 vec1, Vector3 vec2, Vector3 up)
    {
        float sign = AngleDir(vec1, vec2, up);
        return Vector3.Angle(vec1, vec2) * sign;
    }
    static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        return (dir >= 0f) ? 1f : -1f; 
    }
    public static bool LayerMaskContainsLayer(LayerMask _mask, int _layer)
    {
        return (_mask.value & 1 << _layer) > 0; 
    }
    public static T GetComponentInHierarchy<T>(GameObject _go)
    {
        T _t = _go.GetComponent<T>();
        if(_t == null)
        {
            _t = _go.GetComponentInChildren<T>(); 
        }
        return _t; 
    }
    public static List<T> GetComponents<T>(GameObject _go)
    {
        List<T> _list = new List<T>();
        T _component = _go.GetComponent<T>();
        if(_component != null)
        {
            _list.Add(_component); 
        }
        _list.AddRange(_go.GetComponentsInChildren<T>());
        return _list; 
    }
}


public static class IEnumerableExtensions
{
	public static void AddTo<T>(this IEnumerable<T> self, List<T> destination)
	{
		if(self != null) destination.AddRange(self);
	}

}
