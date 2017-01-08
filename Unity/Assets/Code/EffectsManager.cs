using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EffectsManager : MonoBehaviour {

    [SerializeField]
    Object[] _effectPrefabs; 
    static Dictionary<string, Object> Effects;
    static uint _seed = 0;
    static List<Effect> _runningEffects = new List<Effect>();
    static Stack<Effect> _endedEffects = new Stack<Effect>();
    static float _playSpeed = 1; 
    public static float PlaySpeed { get { return _playSpeed; } }

    public static GameObject CreateEffect(string _name)
    {
        return Instantiate(Effects[_name]) as GameObject;
    }
    public static GameObject CreateEffect(string _name, Vector3 _loc, Quaternion _rot)
    {
        return Instantiate(Effects[_name], _loc, _rot) as GameObject; 
    }
    public static uint GetSeed()
    {
        _seed++;
        return _seed; 
    }
    public static void RewindGetSeed()
    {
        _seed--;
    }
    public static void RegisterEffect(Effect _effect)
    {
        _runningEffects.Add(_effect);
    }
    public static void UnregisterEffect(Effect _effect)
    {
        _runningEffects.Remove(_effect);
    }
    public static void EffectEnded(Effect _effect)
    {
        _endedEffects.Push(_effect); 
    }
    public static void Play(float _time)
    {
        foreach(Effect _effect in _runningEffects)
        {
            _effect.Play(_time); 
        }
        while (true)
        {
            if(_endedEffects.Count > 0 && _endedEffects.Peek().EndedAt < _time)
            {
                _endedEffects.Pop().ReloadEffect(_time); 
            }else
            {
                break;
            }
        }
    }
    public static void ChangePlaySpeed(float _newSpeed)
    {
        if(_newSpeed == 0 && _playSpeed != 0)
        {
            foreach(Effect _effect in _runningEffects)
            {
                _effect.Pause(); 
            }
        }
        else if(_newSpeed != 0 && _playSpeed == 0)
        {
            foreach (Effect _effect in _runningEffects)
            {
                _effect.Resume();
            }
        }
        if(_newSpeed != _playSpeed)
        {
            foreach (Effect _effect in _runningEffects)
            {
                _playSpeed = _newSpeed; 
                _effect.ChangeSpeed(_playSpeed);
            }
        }
    }

    void Awake()
    {
        for(int i = 0; i < _effectPrefabs.Length; i++)
        {
            Effects[_effectPrefabs[i].name] = _effectPrefabs[i]; 
        }
    }
}
/*
 * 
 * keeping for reference, found a better way to do this though.
 * But damn, I Just spent a couple hours getting drawers and custom inspectors to 
 * immediately come up wtih a better way to do it.
 * 
[CustomEditor(typeof(EffectsManager)), CanEditMultipleObjects]
public class Change_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        // Grab the script.
        EffectsManager myTarget = target as EffectsManager;
        // Set the indentLevel to 0 as default (no indent).
        EditorGUI.indentLevel = 0;
        // Update
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        //  >>> THIS PART RENDERS THE ARRAY
        SerializedProperty _effects = this.serializedObject.FindProperty("_effects");
        EditorGUILayout.LabelField("Effects"); 
        EditorGUILayout.PropertyField(_effects.FindPropertyRelative("Array.size"));
        EditorGUI.indentLevel = 1; 
        for (int i = 0; i < _effects.arraySize; i++)
        {
            EditorGUILayout.PropertyField(_effects.GetArrayElementAtIndex(i), GUIContent.none);
        }
        //  >>>
        EditorGUI.indentLevel = 0; 
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        // Apply.
        serializedObject.ApplyModifiedProperties();
    }
}
*/