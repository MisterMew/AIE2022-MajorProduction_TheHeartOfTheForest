
/*
 * Date Created: 01.10.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF.Environment
{
    /// <summary>
    /// Switches BGM using a DirectionalEventTrigger
    /// </summary>
    [RequireComponent(typeof(Tools.DirectionalEventTrigger))]
    public class AudioSwitcherEvent : MonoBehaviour
    {
        [Header("Audio")]
        [Tooltip("AudioManager")]
        [SerializeField] private AudioManager audioManager;

        [Header("Settings")]
        [Tooltip("Time to fade in bgm")]
        [SerializeField] private float fadeInTime;
        [Tooltip("Time to fade out bgm")]
        [SerializeField] private float fadeOutTime;

        /// <summary>
        /// AudioFadeOut Coroutine
        /// </summary>
        Coroutine fadeOutCoroutine = null;
        /// <summary>
        /// AudioFadeIn Coroutine
        /// </summary>
        Coroutine fadeInCoroutine = null;

        /// <summary>
        /// DirectionalEventTrigger
        /// </summary>
        Tools.DirectionalEventTrigger dTrigger;

        // Start is called before the first frame update
        void Start()
        {
            if(!audioManager) audioManager = FindObjectOfType<AudioManager>();
            dTrigger = GetComponent<Tools.DirectionalEventTrigger>();
            dTrigger.Setup();
        }

        /// <summary>
        /// Start FadeOut
        /// </summary>
        public void StartFadeOut()
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            if (fadeOutCoroutine == null) fadeOutCoroutine = StartCoroutine(AudioFadeOut(audioManager.bgm, fadeOutTime));
        }

        /// <summary>
        /// Start FadeOut
        /// </summary>
        public void StartFadeOutAmb()
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            if (fadeOutCoroutine == null) fadeOutCoroutine = StartCoroutine(AudioFadeOut(audioManager.ambience, fadeOutTime));
        }

        /// <summary>
        /// Start FadeIn BGM
        /// </summary>
        /// <param name="clip">Clip to fade in</param>
        public void StartFadeIn(AudioClip clip)
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = StartCoroutine(AudioFadeIn(audioManager.bgm, fadeInTime, clip));
        }

        /// <summary>
        /// Start FadeIn BGM
        /// </summary>
        /// <param name="source">AudioSource with clip to fade in</param>
        public void StartFadeIn(AudioSource source)
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = StartCoroutine(AudioFadeIn(audioManager.bgm, fadeInTime, source.clip));
        }

        /// <summary>
        /// Start FadeIn AMB
        /// </summary>
        /// <param name="source">AudioSource with clip to fade in</param>
        public void StartFadeInAmb(AudioSource source)
        {
            if (fadeInCoroutine != null) StopCoroutine(fadeInCoroutine);
            fadeInCoroutine = StartCoroutine(AudioFadeIn(audioManager.ambience, fadeInTime, source.clip));
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Fade volume out
        /// </summary>
        /// <param name="audioSource">Volume to fade</param>
        /// <param name="fadeTime">Time taken to fade</param>
        /// <returns></returns>
        private IEnumerator AudioFadeOut(AudioSource audioSource, float fadeTime)
        {
            float timer = fadeTime;

            // Fade BGM out
            while (timer >= 0)
            {
                audioSource.volume = (timer / fadeTime);
                
                yield return null;
                timer -= Time.deltaTime;
            }

            audioSource.volume = 0;
            fadeOutCoroutine = null;
        }

        /// <summary>
        /// Fade volume in
        /// </summary>
        /// <param name="audioSource">Volume to fade</param>
        /// <param name="fadeTime">Time taken to fade</param>
        /// <returns></returns>
        private IEnumerator AudioFadeIn(AudioSource audioSource, float fadeTime, AudioClip clip)
        {
            // Wait for fadeOut to end
            while (fadeOutCoroutine != null) yield return null;

            // If the same audio is already playing, then don't fade in
            if (audioSource.clip != clip || audioSource.volume == 0)
            {
                float timer = 0;
                float startVol = audioSource.volume;
                audioSource.clip = clip;
                audioSource.Play();

                // Fade BGM in
                while (timer <= fadeTime)
                {
                    audioSource.volume = Mathf.Lerp(startVol, 1, (timer / fadeTime));

                    yield return null;
                    timer += Time.deltaTime;
                }
            }

            audioSource.volume = 1;
            fadeInCoroutine = null;
        }
    }

}