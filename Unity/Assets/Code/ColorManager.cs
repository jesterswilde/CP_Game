using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

    static ColorManager t; 
    [SerializeField]
    Color _extractionColor;
    public static Color ExtractionColor { get { return (t._extractionColor != null) ? t._extractionColor : Color.blue; } }
    [SerializeField]
    Color _enemyTargetColor;
    public static Color EnemyTargetColor { get { return (t._enemyTargetColor != null) ? t._enemyTargetColor : Color.red; } }
    [SerializeField]
    Color _interactableTargetColor;
    public static Color InteractableTargetColor { get { return (t._interactableTargetColor != null) ? t._interactableTargetColor : Color.yellow; } }
    [SerializeField]
    Color _stageTargetColor;
    public static Color StageTargetColor { get { return (t._stageTargetColor != null) ? t._stageTargetColor : Color.green; } }
    [SerializeField]
    Color _itemColor;
    public static Color ItemColor { get { return (t._itemColor != null) ? t._itemColor : Color.yellow; } }
    [SerializeField]
    Color _disabledItem; 
    public static Color DisabledItem { get { return (t._disabledItem != null) ? t._disabledItem : Color.grey; } }

    void Awake()
    {
        t = this; 
    }
}
