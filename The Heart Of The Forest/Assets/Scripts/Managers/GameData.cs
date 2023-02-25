/*
 * Date Created: 13.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Holds settings data
/// </summary>
[CreateAssetMenu(fileName = "GameData", menuName = "HotF/Data/GameData")]

public class GameData : ScriptableObject
{
    [Header("Game Data")]
    public string version = "version number";
    public Vector3 wayPoint;
    public int heartFragmentsReturned = 0;
    public bool isNewGame = false;

    /// <summary>
    /// Save asset (for perforce)
    /// </summary>
    public void SaveAsset()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
    }
}
