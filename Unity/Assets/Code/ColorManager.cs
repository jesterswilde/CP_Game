using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour {

    static ColorManager t; 
    [SerializeField]
	Material _extractionMaterial;
	public static Material ExtractionMaterial { get { return t._extractionMaterial; } }
    [SerializeField]
	Material _enemyTargetMaterial;
	public static Material EnemyTargetMaterial { get { return t._enemyTargetMaterial; } }
    [SerializeField]
	Material _interactableTargetMaterial;
    public static Material InteractableTargetMaterial { get { return t._interactableTargetMaterial; } }
    [SerializeField]
	Material _stageTargetMaterial;
	public static Material StageTargetMaterial { get { return t._stageTargetMaterial; } }
    [SerializeField]
	Material _itemMaterial;
	public static Material ItemMaterial { get { return t._itemMaterial; } }
    [SerializeField]
	Material _disabledMaterial;
	public static Material DisabledMaterial { get { return t._disabledMaterial; } }
	[SerializeField]
	float _outlineWidth; 
	public static float OutlineWidth { get { return t._outlineWidth; } }

    void Awake()
    {
        t = this;  
    }
}
