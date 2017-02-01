using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

[Serializable]
public class GenericKVP<K, V>
{
    public K Key;
    public V Value;
}
[Serializable]
public class StringObjectKVP : GenericKVP<string, UnityEngine.Object> { }
[Serializable]
public class StringIntKVP : GenericKVP<string, int> { }; 

[CustomPropertyDrawer(typeof(StringObjectKVP))]
public class StringObjectDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        int unit = Mathf.FloorToInt(Screen.width / 100f * 42f); 
        Rect keyRect = new Rect(position.x, position.y, unit, position.height);
        Rect valueRect = new Rect(position.x + unit, position.y, unit, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("Key"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}