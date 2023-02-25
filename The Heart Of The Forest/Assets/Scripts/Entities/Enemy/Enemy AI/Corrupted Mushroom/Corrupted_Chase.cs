/*
 * Date Created: 24/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.CorruptedMushroom
{
    [RequireComponent(typeof(Corrupted_StateMachine))]
    public class Corrupted_Chase : Chase
    {
        private Corrupted_Wander m_wander;
        private Corrupted_Climb m_climb;
        private ChaseArea m_chaseArea;

        [Tooltip("True if the player is in the chase radius")]
        private bool m_playerInChaseRadius;
        [Tooltip("True if the player has jumped over the enemy")]
        private bool m_playerJumpedOver;

        [Header("Chase Settings")]
        [Tooltip("The radius of the chase trigger")]
        [SerializeField, Range(0, 20)] private float m_chaseRadius;
        [Tooltip("The speed which the enemy chases")]
        [SerializeField] private float m_chaseSpeed = 1.2f;
        [Tooltip("How long the enemy keeps running when the player jumps over the enemy")]
        [SerializeField] private float m_runPastPlayerTimer = 2.5f;

        //Getters and Setters
        public float ChaseRadius { get { return m_chaseRadius; } set { m_chaseRadius = value; } }
        public ChaseArea ChaseArea { get { return m_chaseArea; } set { m_chaseArea = value; } }

        protected override void Awake()
        {
            base.Awake();

            //Cache components
            m_wander = GetComponent<Corrupted_Wander>();
            m_climb = GetComponent<Corrupted_Climb>();

            InstantiateChaseTrigger();
        }

        public override void Enter()
        {
            base.Enter();

            m_animator.CrossFade("Chase", .07f, -1, 0);

            //Set the running direction
            m_stateMachine.FacingLeft = m_playerTransform.position.x < m_transform.position.x;

            m_playerJumpedOver = false;
        }

        public override void UpdateLogic()
        {
            //Move and rotate enemy in the correct direction
            switch (m_stateMachine.FacingLeft)
            {
                case true:
                    m_transform.position -= new Vector3(m_chaseSpeed, 0, 0) * Time.deltaTime;
                    m_transform.eulerAngles = new Vector3(0, 0, 0);
                    //If the player hasn't jumped over and the player is to the right of the enemy...
                    if (!m_playerJumpedOver && m_playerTransform.position.x > m_transform.position.x)
                    {
                        StopAllCoroutines();
                        StartCoroutine(RunningTimer());
                        m_playerJumpedOver = true;
                    }
                    break;
                case false:
                    m_transform.position += new Vector3(m_chaseSpeed, 0, 0) * Time.deltaTime;
                    m_transform.eulerAngles = new Vector3(0, 180, 0);
                    //If the player hasn't jumped over and the player is to the left of the enemy...
                    if (!m_playerJumpedOver && m_playerTransform.position.x < m_transform.position.x)
                    {
                        StopAllCoroutines();
                        StartCoroutine(RunningTimer());
                        m_playerJumpedOver = true;
                    }
                    break;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public void StopChaseCoroutines()
        {
            StopAllCoroutines();
        }

        public override void EnterChaseTrigger()
        {
            m_playerInChaseRadius = true;

            switch (m_stateMachine.CurrentState)
            {
                case NPCStateTypeEnum.CONFUSED:
                    if (m_stateMachine.LastState == NPCStateTypeEnum.CLIMB)
                    {
                        //Get player direction
                        m_stateMachine.FacingLeft = m_playerTransform.position.x < m_transform.position.x;

                        //If the player direction is NOT the same as the way the enemy just climbed...
                        if (m_stateMachine.FacingLeft != m_climb.ClimbingToLeft)
                        {
                            //Climb the other way
                            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                        }
                    }
                    break;
                case NPCStateTypeEnum.CHASE:
                case NPCStateTypeEnum.TIRED:
                //Do nothing
                    break;
                case NPCStateTypeEnum.STARE:
                case NPCStateTypeEnum.IDLE:
                    switch (m_wander.HitWanderPoint)
                    {
                        case true:
                            switch (m_stateMachine.FacingLeft)
                            {
                                case true:
                                    if (m_playerTransform.position.x > m_transform.position.x)
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                                    else
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.STARE);
                                    break;
                                case false:
                                    if (m_playerTransform.position.x < m_transform.position.x)
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                                    else
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.STARE);
                                    break;
                            }
                            break;
                        case false:
                            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                            break;
                    }
                    break;
                default:
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                    break;

            }
        }

        public override void ExitChaseTrigger()
        {
            m_playerInChaseRadius = false;

            switch (m_stateMachine.CurrentState)
            {
                case NPCStateTypeEnum.STARE:
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WANDER);
                    break;
            }
        }

        IEnumerator RunningTimer()
        {
            yield return new WaitForSeconds(m_runPastPlayerTimer);
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.TIRED);
        }

        /// <summary>
        /// Instantiates a trigger used for detecting the player
        /// </summary>
        protected override void InstantiateChaseTrigger()
        {
            //Instantiate gamobject and add components
            GameObject chaseObj = new GameObject("Chase area");
            chaseObj.AddComponent<ChaseArea>();
            chaseObj.AddComponent<CircleCollider2D>();

            //Cache components
            Transform chaseTransform = chaseObj.GetComponent<Transform>();
            m_chaseArea = chaseObj.GetComponent<ChaseArea>();
            CircleCollider2D col = chaseObj.GetComponent<CircleCollider2D>();

            //Set parent and position
            chaseTransform.SetParent(m_transform);
            chaseTransform.position = m_transform.position;
            //Set the layer to "enemy"
            chaseObj.layer = PLAYER_LAYER;
            //Set size of the collider
            col.radius = m_chaseRadius;
            //Set the collider to a trigger
            col.isTrigger = true;

            //Set chase area variables
            ChaseArea.SetCollider(col);
            ChaseArea.SetChase(this);
        }

        /// <summary>
        /// Checks if the player is in the Chase radius
        /// </summary>
        /// <returns>True if the distance between the enemy and the player is smaller than the radius</returns>
        public override bool PlayerInChaseRadius()
        {
            return m_playerInChaseRadius;
        }
    }
}
