/*
 * Date Created: 24.08.2022
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
[CreateAssetMenu(fileName = "SettingsData", menuName = "HotF/Data/SettingsData")]

public class SettingsData : ScriptableObject
{
    [Header("Audio Settings Data")]
    public float masterVolume = 0.5f;
    public float bgmVolume = 0.5f;
    public float ambienceVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public float guiSfxVolume = 0.5f;

    [Header("Graphics Settings Data")]
    public string screenMode = "Windowed";
    public int screenWidth = 640;
    public int screenHeight = 480;
    public float brightness = 0.5f;
    public float gamma = 0.5f;
    public bool vSync = false;
    public int antiAliasing = 0;
    public int graphicsQuality = 0;

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
