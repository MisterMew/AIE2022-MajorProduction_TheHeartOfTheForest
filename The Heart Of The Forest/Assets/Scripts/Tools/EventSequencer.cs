/*
 * Date Created: 25.10.2022
 * Author: Nghia Do
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Tools
{
    /// <summary>
    /// Runs events in order based on certain conditions
    /// </summary>
    public class EventSequencer : MonoBehaviour
    {
        [System.Serializable]
        public struct EventBlock
        {
            [Tooltip("Amount of time to run event")]
            [SerializeField] public  float timer;
            [Tooltip("Event to run")]
            [SerializeField] public UnityEvent myEvent;
        }

        [Tooltip("Events to run in order")]
        [SerializeField] EventBlock[] eventBlocks;

        /// <summary>
        /// Currently running timer coroutine
        /// </summary>
        private Coroutine timerCoroutine = null;

        /// <summary>
        /// Run events in order after designated time
        /// </summary>
        public void Run()
        {
            StartCoroutine(RunEvents());
        }

        /// <summary>
        /// Run events in order after designated time
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunEvents()
        {
            for (int idx = 0; idx < eventBlocks.Length; idx++)
            {
                timerCoroutine = StartCoroutine(Timer(eventBlocks[idx].timer)); ;
                eventBlocks[idx].myEvent.Invoke();

                while (timerCoroutine != null) yield return null;
            }
        }

        /// <summary>
        /// Countdown timer
        /// </summary>
        /// <param name="time"> Start time</param>
        /// <returns></returns>
        private IEnumerator Timer(float time)
        {
            float timer = time;

            while (timer > 0)
            {
                yield return null;
                timer -= Time.deltaTime;
            }

            timerCoroutine = null;
        }
    }
}