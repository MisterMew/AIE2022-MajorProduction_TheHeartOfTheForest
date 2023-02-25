/*
 * Date Created: 23.08.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

namespace HotF.Tools
{
    /// <summary>
    /// Creates Audioplayer Prefabs
    /// </summary>
    public class AudioPlayerCreator : MonoBehaviour
    {
        [Tooltip("Prefab with AudioPlayer component")]
        [SerializeField] public GameObject apPrefab;
        [Tooltip("Parent to add new Prefabs to")]
        [SerializeField] public GameObject apParent;
        [Tooltip("Audioclips to add to created AudioPlayer prefabs")]
        [SerializeField] public AudioClip[] audioClips;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for AudioPlayerCreator
    /// </summary>
    [CustomEditor(typeof(AudioPlayerCreator))]
    public class AudioPlayerCreatorEditor : Editor
    {
        /// <summary>
        /// This GameData
        /// </summary>
        private AudioPlayerCreator m_apData;

        // This function is called when the object becomes enabled and active
        private void OnEnable() => m_apData = (AudioPlayerCreator)target;

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(m_apData);

            // Create AudioPlayers
            if (GUILayout.Button("Create Audio Player/s"))
            {
                for (int idx = 0; idx < m_apData.audioClips.Length; idx++)
                {
                    GameObject go = Instantiate(m_apData.apPrefab, m_apData.apParent.transform);
                    go.GetComponent<AudioPlayer>().audioClip = m_apData.audioClips[idx];
                    go.name = m_apData.audioClips[idx].name + "_AP";
                }
            }

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}