/*
 * Date Created: 22.08.2022
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
    /// Plays audio
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        [Tooltip("AudioSource to play audio from")]
        [SerializeField] public AudioSource audioSource;
        [Tooltip("Clip to play")]
        [SerializeField] public AudioClip audioClip;
        [Tooltip("MixerGroup for clip")]
        [SerializeField] public AudioMixerGroup mixerGroup;

        /// <summary>
        /// Setup Audio Player
        /// </summary>
        public void Setup()
        {
            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = mixerGroup;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        /// <summary>
        /// Stop current audio
        /// </summary>
        public void Stop() => audioSource.Stop();

        /// <summary>
        /// Play audio
        /// </summary>
        public void Play() => audioSource.Play();

        /// <summary>
        /// Play one shot
        /// </summary>
        public void PlayOneShot() => audioSource.PlayOneShot(audioClip);
        
        public void PlayTest(AudioClip clip) => audioSource.PlayOneShot(clip);
        
        /// <summary>
        /// Play audio from the inspector
        /// </summary>
        public void PlayInspector()
        {
            Setup();
            PlayOneShot();
        }

        /// <summary>
        /// Play audio with a random pitch
        /// </summary>
        /// <param name="range">+/- range of pitch</param>
        public void PlayRandPitch(float range)
        {
            audioSource.pitch = Random.Range(-range, range);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for AudioPlayer
    /// </summary>
    [CustomEditor(typeof(AudioPlayer))]
    [CanEditMultipleObjects]
    public class AudioPlayerEditor : Editor
    {
        /// <summary>
        /// This data
        /// </summary>
        private AudioPlayer data;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable() => data = (AudioPlayer)target;

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(data);

            // Play audio
            if (GUILayout.Button("Play")) data.PlayInspector();

            // Stop audio
            if (GUILayout.Button("Stop")) data.Stop();

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}