using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

[Serializable]
public class CharacterList
{
    [SerializeField]
    Character[] _characters; 
    public Character[] Characters { get { return _characters; } }
}

public class StageManager : MonoBehaviour, IManager {

    [SerializeField]
    int _stage = 0; 
    [SerializeField]
    List<CharacterList> _characters = new List<CharacterList>();
    static StageManager t;
    List<StageComponent> _stageComponents = new List<StageComponent>(); 


    void LoadStage()
    {
        GameManager.ClearCharacters();
        Character[] _cl = _characters[_stage].Characters; 
        for(int i = 0; i < _cl.Length; i++)
        {
            GameManager.RegisterCharacter(_cl[i]); 
        }
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
    void Start()
    {
        t = this; 
    }
    public static void ChangeStage(int _newStage)
    {
        if(t != null)
        {
            t._stage = _newStage;
            t.LoadStage();
        }else
        {
            throw new Exception("No Stage Manager Exists"); 
        }
    }
    public static void RegisterStageComponent(StageComponent _component)
    {
        t._stageComponents.Add(_component); 
    }
}
