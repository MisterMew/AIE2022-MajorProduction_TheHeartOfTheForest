/*
 * Date Created: 24.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotF.Tools
{
    /// <summary>
    /// Scales 3D audio ranges of an audioSource
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class Audio3DScaler : MonoBehaviour
    {
        [Tooltip("Distance for volume falloff")]
        [SerializeField] private float fallOff = 3f;
        [Tooltip("minDistance offset")]
        [SerializeField, Range(0,float.MaxValue)] private float minOffset = 0f;

        /// <summary>
        /// AudioSource with 3D audio
        /// </summary>
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            
            ScaleAudio3D();
        }

        /// <summary>
        /// Scale 3D Sound Settings of minDistance and maxDistance 
        /// </summary>
        private void ScaleAudio3D()
        {
            // Find the larger scale
            float scale = transform.localScale.x >= transform.localScale.y ? transform.localScale.x : transform.localScale.y;
            scale = scale * 0.5f + minOffset;

            // Scale audio
            audioSource.maxDistance = scale + fallOff;
            audioSource.minDistance = scale;
        }
    }

}