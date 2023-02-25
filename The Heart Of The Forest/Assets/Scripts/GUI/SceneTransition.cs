/*
 * Date Created: 06.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace HotF
{
    /// <summary>
    /// Handles the transitioning between scenes
    /// </summary>
    public class SceneTransition : MonoBehaviour
    {
        /* Variables */
        [Header("Scene Transition")]
        [SerializeField] private Animator crossfadeAnimation = default;
        [SerializeField] private bool disableCrossfade = false;
        [SerializeField, Range(0F, 100F)] private float sceneTransitionTime = 0F;
        private int sceneToLoad = 0; //The Mian game scene (0 in build index)

        /// <summary>
        /// Start transition to next scene.
        /// </summary>
        public void TransitionToScene(int sceneIndex)
        {
            sceneToLoad = sceneIndex; //Sets the sceme index to be loaded

            StartCoroutine(Transition()); //Method to start crossfade scene transition
        }

        /// <summary>
        /// Time until crossfade starts
        /// </summary>
        /// <param name="sceneIndex"></param>
        /// <returns></returns>
        IEnumerator Transition()
        {
            yield return new WaitForSeconds(sceneTransitionTime); //Wait for seconds

            if (disableCrossfade) //If crossfade is disabled
            { 
                OnCrossfadeComplete(); //Skip fade and load scene
            }
            else if (!disableCrossfade)
            {
                crossfadeAnimation.SetTrigger("startCrossfade"); //Sets the animation trigger that starts the crossfade
            }
        }

        /// <summary>
        /// Complete crossfade
        /// </summary>
        public void OnCrossfadeComplete()
        {
            SceneManager.LoadScene(sceneToLoad); //Loads the next scene
            Time.timeScale = 1.0F;
        }
    }
}