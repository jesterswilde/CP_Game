using UnityEngine;
using System.Collections;

public class Util {

    public static float AngleBetweenVector3(Vector3 vec1, Vector3 vec2)
    {
        Vector3 difference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector3.Angle(Vector3.right, difference) * sign;
    }
    public static bool LayerMaskContainsLayer(LayerMask _mask, int _layer)
    {
        return (_mask.value & 1 << _layer) > 0; 
    }
}
