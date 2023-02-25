/*
 * Date Created: 06.10.2022
 * Author: Nghia
 * Contributors: 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF
{
    /// <summary>
    /// Manages controller haptics
    /// </summary>
    public class HapticsManager : MonoBehaviour
    {
        [Tooltip("Haptics Low Frequency"), Range(0,1)]
        [SerializeField] public float lowFreq = 0.5f;
        [Tooltip("Haptics High Frequency"), Range(0,1)]
        [SerializeField] public float highFreq = 0.5f;

        private Gamepad gamepad;
        private Coroutine coroutine = null;

        // Start is called before the first frame update
        void Start()
        {
            gamepad = Gamepad.current;
            gamepad?.ResetHaptics();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Pause gamepad haptics
        /// </summary>
        public void PauseHaptics() => gamepad?.PauseHaptics();

        /// <summary>
        /// Resume gamepad haptics
        /// </summary>
        public void ResumeHaptics() => gamepad?.ResumeHaptics();

        /// <summary>
        /// Start controller haptics
        /// </summary>
        /// <param name="time">Time spend viabrating</param>
        public void StartHaptics(float time)
        {
            if (coroutine == null) coroutine = StartCoroutine(Rumbble(time));
        }

        /// <summary>
        /// Start controller haptics
        /// </summary>
        /// <param name="time">Time spend viabrating</param>
        /// <returns></returns>
        public IEnumerator Rumbble(float time)
        {
            // Get currently conected gamepad
            if (gamepad == null) gamepad = Gamepad.current;

            // Start gamepad rumble
            gamepad?.SetMotorSpeeds(lowFreq, highFreq);
            yield return new WaitForSeconds(time);

            // Stop gamepad rumble
            gamepad?.ResetHaptics();
            coroutine = null;
        }
    }

}