/*
 * Date Created: 04.10.2022
 * Author: Nghia Do
 * Contributors: 
 */

using System.Collections;
using UnityEngine;

namespace HotF.Environment
{
    /// <summary>
    /// Animate grass rustle by rotating it
    /// </summary>
    [RequireComponent(typeof(Tools.DirectionalEventTrigger))]
    public class GrassRustle : MonoBehaviour
    {
        [Tooltip("Rustle animation")]
        [SerializeField] private AnimationCurve rustleCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [Tooltip("Rotation scale in degrees")]
        [SerializeField] private float scaleRotation = 45.0f;
        [Tooltip("Lean distance")]
        [SerializeField] private float scaleLean = 1.0f;
        [Tooltip("Time spent animating")]
        [SerializeField] private float time = 2.0f;
        [Tooltip("Target to rustle")]
        [SerializeField] private Transform rustleTarget;

        /// <summary>
        /// Rotation axis
        /// </summary>
        private Vector3 axis = Vector3.forward;

        /// <summary>
        /// AudioSource
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// DirectionalEventTrigger
        /// </summary>
        private Tools.DirectionalEventTrigger dt;

        /// <summary>
        /// Currently running coroutine
        /// </summary>
        private Coroutine coroutine = null;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            dt = GetComponent<Tools.DirectionalEventTrigger>();
            dt.Setup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Start lean animation
        /// </summary>
        public void StartLean()
        {
            // If the player isn't null and coroutine isn't already running
            if (dt.collidedPlayer && coroutine == null) 
                coroutine = StartCoroutine(Lean());
        }

        /// <summary>
        /// Animate by leaning (Not wokring)
        /// </summary>
        /// <returns></returns>
        IEnumerator Lean()
        {
            Vector3 pos = dt.collidedPlayer.GetComponent<Transform>().position;
            bool reverse = pos.z < transform.position.z;
            float startRotation = transform.eulerAngles.z;
            float startDist = Mathf.Abs(Vector3.Distance(transform.position, pos));
            float dist = startDist;

            // While player is within the trigger
            while (dist>0.01f && dist <= startDist+1)
            {
                dist = Mathf.Abs(Vector3.Distance(transform.position, pos));
                transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(startRotation + scaleRotation, startRotation,  dist/startDist));
                yield return null;
            }

            if (dist <= 0.01f)
            {
                StartRustle(reverse);
            }

            coroutine = null;
        }

        /// <summary>
        /// Start russle animation
        /// </summary>
        /// <param name="reverse">Reverse animation?</param>
        public void StartRustle(bool reverse)
        {
            if (coroutine == null) coroutine =  StartCoroutine(Rustle(reverse?-scaleRotation:scaleRotation));
        }

        /// <summary>
        /// Animate by Rotation
        /// </summary>
        /// <returns></returns>
        IEnumerator Rustle(float scale)
        {
            float timer = 0;
            float prevRot = scale * rustleCurve.Evaluate(timer / time);
            float curRot;
            Quaternion startRotation = rustleTarget.transform.rotation;

            audioSource.PlayOneShot(audioSource.clip);

            while (timer <= time)
            {
                curRot = scale * rustleCurve.Evaluate(timer / time);
                rustleTarget.RotateAround(transform.position, axis, prevRot - curRot);

                yield return null;
                prevRot = curRot;
                timer += Time.deltaTime;
            }

            // Reset transform back to normal
            rustleTarget.rotation = startRotation;

            coroutine = null;
        }
    }

}