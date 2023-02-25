/*
 * Date Created: 26.08.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hotF
{
    /// <summary>
    /// Manages time
    /// </summary>
    public class TimeManager : MonoBehaviour
    {
        [Tooltip("Default time scale")]
        [SerializeField] private float defaultTimeScale = 1;
        [Tooltip("Default fixed time scale (0.02 recommended)")]
        [SerializeField] private float defaultFixedTimeScale = 0.02f;

        [Header("Time Alter")]
        [Tooltip("Total duration to alter time")]
        [SerializeField] public float alterTimeDuration;
        [Tooltip("Time alter curve")]
        [SerializeField] private AnimationCurve timeCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        /// <summary>
        /// Time scale of game
        /// </summary>
        public float TimeScale
        {
            set 
            { 
                Time.timeScale = value < 0 ? 0 : value; // alter regular time
                Time.fixedDeltaTime = defaultFixedTimeScale * value/defaultTimeScale; // alter physics time
            }
            get { return Time.timeScale; }
        }

        /// <summary>
        /// Setup time manager
        /// </summary>
        public void Setup()
        {
            StartTime();
        }

        /// <summary>
        /// Set time to its default
        /// </summary>
        public void StartTime() => TimeScale = defaultTimeScale;

        /// <summary>
        /// Stop time
        /// </summary>
        public void StopTime() => TimeScale = 0f;

        /// <summary>
        /// Start altering time
        /// </summary>
        public void StartAlterTime()
        {
            StartCoroutine(AlterTime(alterTimeDuration));
        }

        /// <summary>
        /// Coroutine to alter time
        /// </summary>
        /// <param name="alterTimeDuration">Total duration to alter time</param>
        /// <returns></returns>
        IEnumerator AlterTime(float alterTimeDuration)
        {
            float timer = 0;

            while (timer < alterTimeDuration)
            {
                TimeScale = defaultTimeScale * timeCurve.Evaluate(timer / alterTimeDuration);
                
                yield return new WaitForSecondsRealtime(0);
                timer += Time.unscaledDeltaTime;
            }
        }
    }

}