/*
 * Date Created: 10.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF
{
    /// <summary>
    /// Player Auido Manager
    /// </summary>
    public class PlayerAudioManager : MonoBehaviour
    {
        [Tooltip("Player SFX AudioSource")]
        [SerializeField] private AudioSource audioSource;
        [Tooltip("Player hurt Audio")]
        [SerializeField] private AudioClip[] hurtAudioList;
        [Tooltip("Player footsteps Audio")]
        [SerializeField] private AudioClip[] footstepsAudioList;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Play a random hurt audio
        /// </summary>
        public void PlayHurtAudio() => PlayRandomAudio(hurtAudioList);

        /// <summary>
        /// Play a random footsteps audio
        /// </summary>
        public void PlayFootstepsAudio() => PlayRandomAudio(footstepsAudioList);

        /// <summary>
        /// Play random audioclip form a list
        /// </summary>
        /// <param name="audioList">List of AudioClips to choose from</param>
        private void PlayRandomAudio(AudioClip[] audioList)
        {
            // Check if list is empty
            if (audioList?.Length <= 0) return;

            int randIdx = Random.Range(0, audioList.Length);
            audioSource.PlayOneShot(audioList[randIdx]);
        }
    }

}