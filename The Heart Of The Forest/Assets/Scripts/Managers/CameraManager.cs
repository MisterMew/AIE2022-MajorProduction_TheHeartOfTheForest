/*
 * Date Created: 25.08.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace HotF
{
    /// <summary>
    /// Main player camera and cutscene camera manager
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        [Tooltip("Main virtual camera")]
        [SerializeField] private CinemachineVirtualCamera mainVcam;
        [Tooltip("Player Transform")]
        [SerializeField] Transform player;

        [Header("Camera Shake")]
        [Tooltip("Initial shake strength")]
        [SerializeField] private float amplitude;
        [Tooltip("Initial shake duration")]
        [SerializeField] private float duration;
        [Tooltip("Shake curve")]
        [SerializeField] private AnimationCurve shakeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Camera Fade")]
        [Tooltip("Time taken to fade completely")]
        [SerializeField] float fadetime = 1;
        /// <summary>
        /// Camera's current target
        /// </summary>
        private Transform currentTarget;

        /// <summary>
        /// Camera's current target
        /// </summary>
        public Transform CurrentTarget
        {
            get { return currentTarget; }
            set { currentTarget = value; }
        }
        
        /// <summary>
        /// Current camera shake coroutine running
        /// </summary>
        private Coroutine cameraShakeCoroutine;

        /// <summary>
        /// Camera noise (for camera shake)
        /// </summary>
        private CinemachineBasicMultiChannelPerlin noise;

        /// <summary>
        /// Setup main vcam to follow and look at player
        /// </summary>
        public void Setup()
        {
            mainVcam.Priority = 1;
            noise = mainVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (!player) player = FindObjectOfType<Player.PlayerMovement>().transform;

            SetCamTarget(CurrentTarget);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Set the camera to look at and follow a target
        /// </summary>
        /// <param name="target">Object to look at and follow</param>
        public void SetCamTarget(Transform target)
        {
            CurrentTarget = target;
            mainVcam.Follow = CurrentTarget;
        }

        /// <summary>
        /// Turn camera shake on
        /// </summary>
        public void StartCameraShake()
        {
            if (cameraShakeCoroutine != null) StopCoroutine(cameraShakeCoroutine);
            cameraShakeCoroutine = StartCoroutine(CameraShake(amplitude, duration));
        }

        /// <summary>
        /// Turn camera shake on
        /// </summary>
        /// <param name="time">Camera shake time</param>
        public void StartCameraShake(float time)
        {
            if (cameraShakeCoroutine != null) StopCoroutine(cameraShakeCoroutine);
            cameraShakeCoroutine = StartCoroutine(CameraShake(amplitude, time));
        }

        /// <summary>
        /// Turn camera shake off
        /// </summary>
        public void StopCameraShake() => noise.m_AmplitudeGain = 0f;

        /// <summary>
        /// Coroutine to shake camera
        /// </summary>
        /// <param name="amplitude">Shake strength</param>
        /// <param name="duration">Shake duration</param>
        public IEnumerator CameraShake(float amplitude, float time)
        {
            float timer = 0;

            // Turn on shake
            noise.m_AmplitudeGain = amplitude;

            while (timer < time)
            {
                noise.m_AmplitudeGain = Mathf.SmoothStep(amplitude, 0f, shakeCurve.Evaluate(timer / time));
                
                yield return null;
                timer += Time.deltaTime;
            }

            StopCameraShake();
        }

        /// <summary>
        /// Switch to a different Camera
        /// </summary>
        /// <param name="vCam">Camera to switch to</param>
        public void SwitchCamera(CinemachineVirtualCamera vCam)
        {
            mainVcam.Priority = 0;
            vCam.Priority = 1;
            mainVcam = vCam;
        }

        /// <summary>
        /// Fade camera Image in
        /// </summary>
        /// <param name="sb"></param>
        public void VcamFadeImageIn(CinemachineStoryboard sb)
        {
            Fade(sb, true);
        }

        /// <summary>
        /// Fade camera Image out
        /// </summary>
        /// <param name="sb"></param>
        public void VcamFadeImageOut(CinemachineStoryboard sb)
        {
            Fade(sb, false);
        }

        /// <summary>
        /// Fade in/out an Image
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="isFadeIn"></param>
        private void Fade(CinemachineStoryboard sb, bool isFadeIn)
        {
            float timer = fadetime;
            float start = isFadeIn ? 0 : 1;
            float end = isFadeIn ? 1 : 0;

            while (timer > 0)
            {
                sb.m_Alpha = Mathf.Lerp(end, start, timer / fadetime);

                timer -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Toggle storyboard mute
        /// </summary>
        /// <param name="sb"></param>
        public void VcamToggleMute(CinemachineStoryboard sb)
        {
            sb.m_MuteCamera = !sb.m_MuteCamera;
        }
    }

}