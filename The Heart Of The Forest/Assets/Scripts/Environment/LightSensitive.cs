/*
 * Date Created: 30.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: Nick Connell
 */

using HotF.Abilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Environment
{
    /// <summary>
    /// Handles the light sensitive platforms - their functionality and how they react towards light
    /// </summary>
    public class LightSensitive : MonoBehaviour
    {
        /* Variables */
        [Header("References")]
        [SerializeField] private GlowAbility glowAbilty;
        [SerializeField] private Transform meshParent;
        private GameObject player;

        [Header("Platform Parameters")]
        [SerializeField] private bool reverseSensitivity = false;
        [Tooltip("How fast the object will shrink away.")]
        [SerializeField] private float shrinkSpeed = 0F;
        [Tooltip("How fast the object will regrow.")]
        [SerializeField] private float regrowSpeed = 0F;

        [Tooltip("How long until the object respawns after there is no more light.")]
        [SerializeField, Range(0F, 60F)] private float regrowDelay = 0F;

        [Tooltip("When the mushroom shrinks")]
        [SerializeField] private UnityEvent OnShrink;
        [Tooltip("When the mushroom grows")]
        [SerializeField] private UnityEvent OnGrow;

        [Header("Animations - BROKEN")]
        [SerializeField] private AnimationCurve shrinkAnimation = default;
        [SerializeField] private AnimationCurve regrowAnimation = default;
        private bool animationActive = false;

        private Vector3 maxSize = Vector3.zero;
        private Vector3 minSize = Vector3.zero;
        private float lerpTime = 0F;

        /// <summary>
        /// Called once before start
        /// </summary>
        private void Awake() => player = GameObject.FindWithTag("Player");

        /// <summary>
        /// Called once before start
        /// </summary>
        private void Start()
        {
            maxSize = meshParent.localScale; //set the maximum size

            if (reverseSensitivity) //If the sensitivity is reversed
                meshParent.localScale = minSize;
        }

        /// <summary>
        /// When the platform detects a light trigger
        /// </summary>
        /// <param name="trigger"></param>
        private void OnTriggerEnter2D(Collider2D trigger)
        {
            if (trigger.gameObject.GetComponentInParent<Light>())
            {
                /* Shrink */
                if (meshParent.localScale != minSize && !reverseSensitivity)
                {
                    StartCoroutine(LerpWithSize(meshParent.localScale, minSize, shrinkSpeed));
                    OnShrink.Invoke();
                }

                /* Regrow */
                if (trigger.gameObject.GetComponentInParent<Light>() && meshParent.localScale != maxSize &&
                    reverseSensitivity)
                {
                    StartCoroutine(LerpWithSize(meshParent.localScale, maxSize, shrinkSpeed));
                    OnGrow.Invoke();
                }
            }
        }

        /// <summary>
        /// Called when no light 
        /// </summary>
        /// <param name="trigger"></param>
        private void OnTriggerExit2D(Collider2D trigger)
        {
            if (trigger.gameObject.GetComponent<Light>()) {
                /* Regrow */
                if (meshParent.localScale != maxSize && !reverseSensitivity)
                {
                    StartCoroutine(DelayRegrow(false));
                    OnGrow.Invoke();
                }

                /* Shrink */
                if (trigger.gameObject.GetComponent<Light>() && meshParent.localScale != minSize && reverseSensitivity)
                {
                    StartCoroutine(LerpWithSize(meshParent.localScale, minSize, shrinkSpeed));
                    OnShrink.Invoke();
                }
            } 
        }

        /// <summary>
        /// Used to temporarly create one-way platform
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayRegrow(bool useAnimationCurve)
        {
            yield return new WaitForSeconds(regrowDelay); //Delay before respawning

            if (useAnimationCurve)
                StartCoroutine(LerpWithCurve(meshParent.localScale, maxSize, regrowSpeed, regrowAnimation));

            else if (!useAnimationCurve)
                StartCoroutine(LerpWithSize(meshParent.localScale, maxSize, regrowSpeed));
        }

        /// <summary>
        /// Handle the paltform grow/shrink using animation curves
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <param name="animationSpeed"></param>
        /// <param name="animCurve"></param>
        /// <returns></returns>
        private IEnumerator LerpWithCurve(Vector3 startSize, Vector3 endSize, float animationSpeed, AnimationCurve animCurve)
        {
            animationActive = true; //Flag animation as active
            float maxLerpTime = animCurve.keys[animCurve.keys.Length - 1].time; //cache the last key on horizontal axis

            lerpTime = 0F;
            while (lerpTime < maxLerpTime) //While the animation is still playing
            {
                meshParent.localScale = Vector3.Lerp(startSize, endSize, animCurve.Evaluate(lerpTime)); //Get the key at the current lerp time

                yield return new WaitForEndOfFrame(); //Wait

                lerpTime += Time.deltaTime * animationSpeed; //Apply speed
            }

            meshParent.localScale = endSize;
            animationActive = false; //Flag animation as inactive
        }

        /// <summary>
        /// Handle the paltform grow/shrink using transform 
        /// </summary>
        /// <param name="startSize"></param>
        /// <param name="endSize"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        private IEnumerator LerpWithSize(Vector3 startSize, Vector3 endSize, float speed)
        {
            lerpTime = 0F;
            while (lerpTime < 1F)
            {
                meshParent.localScale = Vector3.Lerp(startSize, endSize, lerpTime);

                lerpTime += Time.deltaTime * speed; //Apply speed
                yield return null;
            }
            meshParent.localScale = endSize;
        }
    }
}