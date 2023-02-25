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
/// Manages game data statistics
/// </summary>
/// <ref>https://www.youtube.com/watch?v=aUi9aijvpgs</ref>
public class StatisticsManager : MonoBehaviour
{
    [Tooltip("Statistics Data")]
    [SerializeField] protected StatisticsData data;

    /// <summary>
    /// Reset all statistics data
    /// </summary>
    public void ResetAllData()
    {
        data.jumpCount = 0;
        data.doubleJumpCount = 0;

        data.burrowCount = 0;
        data.glideCount = 0;
        data.glowCount = 0;

        data.burrowFailedCount = 0;
        data.glideFailedCount = 0;
        data.glowFailedCount = 0;

        data.totalSaveCount = 0;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for StatisticsManager
    /// </summary>
    [CustomEditor(typeof(StatisticsManager))]
    [CanEditMultipleObjects]
    public class StatisticsManagerEditor : Editor
    {
        /// <summary>
        /// This data
        /// </summary>
        private StatisticsManager data;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            data = (StatisticsManager)target;
        }

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(data);

            GUILayout.BeginHorizontal();
            // Reset data
            if (GUILayout.Button("Reset Statistics")) data.ResetAllData();
            GUILayout.EndHorizontal();

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
