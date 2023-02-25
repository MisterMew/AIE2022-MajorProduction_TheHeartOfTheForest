/*
 * Date Created: 12.10.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Holds playthrough statistics data
/// </summary>
[CreateAssetMenu(fileName = "StatisticsData", menuName = "HotF/Data/StatisticsData")]
public class StatisticsData : ScriptableObject
{
    [Header("Deaths")]
    public int playerTotalDeathCount;

    [Header("Ability use")]
    public int jumpCount = 0;
    public int doubleJumpCount = 0;
    public int burrowCount = 0;
    public int glideCount = 0;
    public int glowCount = 0;
    [Header("Ability Fail")]
    public int burrowFailedCount = 0;
    public int glideFailedCount = 0;
    public int glowFailedCount = 0;
    [Header("Save data")]
    public int totalSaveCount = 0;
    public int[] waypointSaveCount;

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
