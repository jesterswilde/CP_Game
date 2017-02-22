using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 


public class StageManager : MonoBehaviour, IManager {

    [SerializeField]
    int _stage = 0; 
    [SerializeField]
    static StageManager t;
    static List<StageComponent> _stageComponents = new List<StageComponent>(); 


    void LoadStage()
    {
        for(int i = 0; i < _stageComponents.Count; i++)
        {
            _stageComponents[i].LoadNewStage(_stage); 
        }
    }
    

    public void StartManager()
    {
        LoadStage(); 
    }

    public void UpdateManager(float _time, float _deltaTime)
    {
    }

    public void FixedUpdateManager(float _time, float _deltaTime)
    {
    }
    void Awake()
    {
        GameManager.RegisterManager(this); 
    }
    void Start()
    {
        t = this; 
    }
    public static void ChangeStage(int _newStage)
    {
        if(t != null && GameManager.CanAcceptPlayerInput)
        {
            t._stage = _newStage;
            GameManager.JumpToTime(0, t.LoadStage);
        }
    }
    public static void RegisterStageComponent(StageComponent _component)
    {
        _stageComponents.Add(_component); 
    }
}
