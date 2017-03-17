using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 


public class StageManager : MonoBehaviour, IManager {

    [SerializeField]
    int _stage = 0;
    static StageManager t;
    List<StageComponent> _stageComponents = new List<StageComponent>(); 
	public static int CurrentStage{ get { return t._stage; } }
	List<StageChangeActivate> _changeListeners = new List<StageChangeActivate>(); 

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
		t = this; 
    }
    void Start()
    {
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
        t._stageComponents.Add(_component); 
    }
	public static void RegisterEventListener(StageChangeActivate _listener){
		t._changeListeners.Add (_listener); 
	}
	public static void BroadcastEvent(){
		foreach (StageChangeActivate _listener in t._changeListeners) {
			
		}

	}
}
