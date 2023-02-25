/*
 * Date Created: 12.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: Lewis Comstive
 */

/*
 * CHANGE LOG:
 * Nghia: added audio functionality
 */

using System.Collections;
using UnityEngine;

namespace HotF.Environment.Platform
{

    /// <summary>
    /// Handles the rotation of platforms
    /// </summary>
    public class RotatingPlatform : MonoBehaviour
    {
        /* Variables */
        [Header("Incremental Rotation")]
        [SerializeField] private bool rotateConstantly = false;
        [Tooltip("The degree that the platform will snap to")]
        [SerializeField, Range(0F, 360F)] private float snapDegree = 0F;
        [Tooltip("Time in seconds between rotations")]
        [SerializeField, Range(0F, 360F)] private float rotationDelay = 0F;
        [Tooltip("Rotation speed in degrees per second")]
        [SerializeField, Range(0F, 360F)] private float rotationSpeed = 0F;

        /// <summary>
        /// Audio source
        /// </summary>
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            if (!rotateConstantly) //For incremental rotation
            {
                audioSource.loop = false;
                audioSource.Stop();
                StartCoroutine(RotatePlatform());
            }
            else
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }


        private void Update()
        {
            if (rotateConstantly) //For constant rotation
                transform.localEulerAngles -= Vector3.forward * rotationSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Rotates the platform incrementally
        /// </summary>
        /// <returns></returns>
        IEnumerator RotatePlatform()
        {
            float currentAngle = 0F; //Stores the current angle
            float desiredAngle = transform.localEulerAngles.z + snapDegree;

            audioSource.Play();

            while (currentAngle < snapDegree) //While the current angle is less than the snapDegree
            {
                currentAngle += rotationSpeed * Time.deltaTime; //Rotation in one loop

                transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime; //Increment rotation

                yield return new WaitForEndOfFrame();
            }
            transform.localEulerAngles = Vector3.forward * desiredAngle;

            yield return new WaitForSeconds(rotationDelay); //Wait for seconds

            StartCoroutine(RotatePlatform());
        }

        /// <summary>
        /// Resets the rotation origin to 0
        /// </summary>
        private void ResetRotationOrigin()
        {
            transform.localEulerAngles = Vector3.zero;
        }
    }
}