/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace HotF.AI
{
    public class Idle : NPC_State
    {
        private float waitTime;
        private float elapsedTime;

        [Header("Settings")]
        [Tooltip("Ignores the idle timer, and continues the idle state infinitely")]
        [SerializeField] private bool m_infiniteIdle;
        [Tooltip("Range for how long the enemy will be idle for")]
        [SerializeField] private Vector2 m_idleTimer;

        [Header("Idle Events")]
        [Tooltip("When the Idle timer ends")]
        [SerializeField] private UnityEvent OnIdleTimerEnd;

        public override void Enter()
        {
            base.Enter();

            //Play the idle animation
            m_animator.CrossFade("Idle", .2f, -1, 0);

            //If infinite idle is false, limit the idle state by the timer
            if (!m_infiniteIdle)
                StartCoroutine(IdleTimer());
        }

        public override void Exit()
        {
            base.Exit();

            StopAllCoroutines();
        }

        /// <summary>
        /// Runs the timer for the idle state
        /// </summary>
        /// <returns></returns>
        protected IEnumerator IdleTimer()
        {
            //Get random number
            float randNum = Random.Range(m_idleTimer.x, m_idleTimer.y);

            //Set timer
            waitTime = randNum;
            elapsedTime = 0;

            while (elapsedTime < waitTime)
            {
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            OnIdleTimerEnd.Invoke();
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WANDER);
        }
    }
}
