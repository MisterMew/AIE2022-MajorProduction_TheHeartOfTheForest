/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF.AI
{
    public class Climb : NPC_State
    {
        private float waitTime;
        private float elapsedTime;

        private ClimbableSurface m_climbableSurface;
        private bool m_readyToClimb = false;

        [Tooltip("If the NPC is currently climbing")]
        protected bool m_isClimbing;
        [Tooltip("If the NPC is climbing to the left or right")]
        protected bool m_climbingToLeft;

        [Tooltip("The position of the NPC when it first enters the climbable surface")]
        protected Vector3 m_firstPosition;

        [Tooltip("The length of the climb animation")]
        [SerializeField] protected float m_animTimer = 2.0f;


        //Getters and Setters
        public ClimbableSurface ClimbableSurface { get { return m_climbableSurface; } }
        public bool IsClimbing { get { return m_isClimbing; } set { m_isClimbing = value; } }
        public bool ClimbingToLeft { get { return m_climbingToLeft; } }
        public bool ReadyToClimb { get { return m_readyToClimb; } }
        public void SetClimbableSurface(ClimbableSurface surface) { m_climbableSurface = surface; }

        public override void Enter()
        {
            base.Enter();

            //Set climbing to true
            m_isClimbing = true;

            //Set original position
            m_firstPosition = m_transform.position;

            //Play the "Climb" animation
            m_animator.CrossFade("Climb", 0, -1, 0);

            //Start the climb coroutine based on which way the enemy is going
            if (m_transform.position.x > m_climbableSurface.transform.position.x)
            {
                StartCoroutine(ClimbCoroutine(m_climbableSurface.m_leftPoint));
                m_climbingToLeft = true;
                m_transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                StartCoroutine(ClimbCoroutine(m_climbableSurface.m_rightPoint));
                m_climbingToLeft = false;
                m_transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        IEnumerator ClimbCoroutine(Vector3 toPos)
        {
            //Set the elapsed and wait times
            elapsedTime = 0;
            waitTime = m_animTimer;

            //While elapsed time is less than wait time...
            while (elapsedTime < waitTime)
            {
                //Add delta time to elapsed
                elapsedTime += Time.deltaTime;

                //Lerp to the new position
                m_transform.position = Vector3.Slerp(m_firstPosition, toPos, elapsedTime / waitTime);

                yield return null;
            }

            //Not climbing anymore
            m_isClimbing = false;

            ChangeToAppropriateState();
        }

        public virtual void EnterClimbTrigger()
        {
            m_readyToClimb = true;
        }

        public virtual void ExitClimbTrigger()
        {
            m_readyToClimb = false;
        }

        /// <summary>
        /// Changes to the correct state after the climbing state has exited
        /// </summary>
        protected virtual void ChangeToAppropriateState()
        {
            InstantTurnAround();
        }

        /// <summary>
        /// Makes the NPC climb in the other direction instantaneously when needed
        /// </summary>
        protected virtual void InstantTurnAround() { }
    }
}

