using UnityEngine;
using System.Collections;

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
}
