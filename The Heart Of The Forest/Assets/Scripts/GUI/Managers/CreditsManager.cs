/*
 * Date Created: 10.10.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF.GUI
{
    /// <summary>
    /// Handles the afk timer and button animation for the return button
    /// </summary>
    public class CreditsManager : MonoBehaviour
    {
        /* Variables */
        private SceneTransition sceneTransition;
        private CanvasGroup canvasGroup;

        [Header("Input Detection")]
        [SerializeField] private InputActionReference anyKeyInput = null;
        [SerializeField] private int timeoutTime = 0;
        public int time = 0;

        [Header("Animation Curves")]
        [SerializeField, Range(0F, 1F)] private float lerpSpeed = 0.1F;
        [SerializeField] private AnimationCurve showAnimation = default;
        [SerializeField] private AnimationCurve hideAnimation = default;
        private bool animActive = false;
        private float lerpTime = 0F;

        [Header("UI Elements")]
        [SerializeField] private GameObject returnButton = null;
        private bool onHover = false;

        private void Start() => canvasGroup = returnButton.GetComponent<CanvasGroup>();

        //private void Update()
        //{
        //    if (!anyKeyInput.action.triggered) //If thers no current input
        //        DoTimer(true);

        //    /* ACTIVE */
        //    else //If input detected
        //    {
        //        time = 0; //Reset timer
        //        StartCoroutine(AnimateAFK(0, 1, showAnimation)); //Show button
        //    }

        //    /* AFK */
        //    if (time == timeoutTime) //If user has gone AFK
        //    {
        //        StartCoroutine(AnimateAFK(1, 0, hideAnimation));
        //    }
        //}

        /// <summary>
        /// Handles the afk countdown timer
        /// </summary>
        /// <param name="doTimer"></param>
        private void DoTimer(bool doTimer)
        {
            if (doTimer && !onHover)
                time += 1;

            else if (!doTimer || onHover)
                time = 0;
        }

        /// <summary>
        /// Animates the buttons alpha when going or returning from being afk
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="animCurve"></param>
        private IEnumerator AnimateAFK(float start, float end, AnimationCurve animCurve)
        {
            if (animActive) yield return null; //Exit function if animation is currently active
            
            animActive = true;

            lerpTime = 0F;
            float maxLerpTime = animCurve.keys[animCurve.keys.Length - 1].time;
            while (lerpTime < maxLerpTime) //While the animation is active
            {
                canvasGroup.alpha = Mathf.Lerp(start, end, animCurve.Evaluate(lerpTime));

                yield return new WaitForEndOfFrame(); //Wait

                lerpTime += Time.deltaTime * lerpSpeed; //Apply speed
            }

            /* Validate end size */
            if (animCurve == hideAnimation)
                canvasGroup.alpha = 0;
            else if (animCurve == showAnimation)
                canvasGroup.alpha = 1;

            animActive = false;
        }

        /// <summary>
        /// Overrides the AFK state while being hovered
        /// </summary>
        public void OnHover(bool isHovering)
        {
            onHover = isHovering; //Set onHover bool

            if (!onHover) return;
            canvasGroup.alpha = 1; //Set canvasGroup to visible
        }

        private void ReturnMainMenu() => sceneTransition.TransitionToScene(0); //Transition to main menu
    }
}