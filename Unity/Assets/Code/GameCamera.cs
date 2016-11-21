using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {


    static Transform _point;
    static Transform _lookAt;
    static UnityStandardAssets.ImageEffects.BlurOptimized _blur; 
    static float _blurSpeed;
    static bool _maintainLookat;
    static bool _maintainLock;
    static GameCamera t; 

    void Awake()
    {
        _blur = GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>();
        t = this; 
    }
    void LateUpdate()
    {
        if (_maintainLock)
        {
            transform.position = _point.position; 
        }
        if (_maintainLookat)
        {
            transform.LookAt(_lookAt.position);
        }
    }
    public static void NewController(Transform point, Transform lookat, bool maintainLookat, bool maintainLock)
    {
        _maintainLookat = maintainLookat;
        _maintainLock = maintainLock;
        _point = point;
        _lookAt = lookat; 
    }
    public static void NewController(Transform _trans, bool maintainLock)
    {
        _maintainLock = maintainLock;
        _maintainLookat = false;
        _lookAt = null;
        _point = _trans;
        t.transform.rotation = _trans.rotation; 
    }
    #region blur
    public static float Blur()
    {
        t.StartBlurring(); 
        return _blurSpeed; 
    }
    void StartBlurring()
    {
        StartCoroutine("Blurring"); 
    }
    IEnumerator Blurring()
    {
        float _timer = 0;
        _blur.blurSize = 0;
        _blur.enabled = true;
        _blurSpeed = GameManager.JumpDuration / 2; 
        while (_timer < _blurSpeed)
        {
            _timer += Time.deltaTime;
            _blur.blurSize = _timer * 10 / _blurSpeed;
            yield return null; 
        }
        _timer = _blurSpeed;
        while(_timer > 0)
        {
            _timer -= Time.deltaTime;
            _blur.blurSize = _timer * 10 / _blurSpeed;
            yield return null; 
        }
        _blur.enabled = false; 
    }
    #endregion

}
