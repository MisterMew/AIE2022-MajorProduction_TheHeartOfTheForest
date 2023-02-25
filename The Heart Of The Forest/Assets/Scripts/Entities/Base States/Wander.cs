/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.AI
{
    public class Wander : NPC_State
    {
        [Tooltip("The left collider gameobject")]
        private GameObject m_leftCollider;
        [Tooltip("The right collider gameobject")]
        private GameObject m_rightCollider;

        [Tooltip("True if the NPC has just hit a wander point")]
        private bool m_hitWanderPoint = false;

        [Header("Settings")]

        [Tooltip("The movement speed")]
        [SerializeField] private float m_speed = 1.0f;
        [Tooltip("Start facing left?")]
        [SerializeField] private bool startFacingLeft = true;

        [Tooltip("The left most point the enemy can wander to")]
        public Vector3 m_leftPoint;
        [Tooltip("The right most point the enemy can wander to")]
        public Vector3 m_rightPoint;

        //Getters and Setters
        public Vector3 LeftPoint { get { return m_leftPoint; } set { m_leftPoint = value; } }
        public Vector3 RightPoint { get { return m_rightPoint; } set { m_rightPoint = value; } }
        public bool HitWanderPoint { get { return m_hitWanderPoint; } }

        protected override void Awake()
        {
            base.Awake();

            //Instantiate the colliders
            InstantiateWanderColliders(WanderPointTypeEnum.LEFT_POINT);
            InstantiateWanderColliders(WanderPointTypeEnum.RIGHT_POINT);
        }

        protected override void Start()
        {
            base.Start();

            //Set the starting facing rotation
            m_stateMachine.FacingLeft = startFacingLeft;
        }

        public override void Enter()
        {
            base.Enter();

            //If the NPC has just hit a wander point, swap the facing direction
            if (m_hitWanderPoint)
                m_stateMachine.SwapFacingDirection();

            m_animator.CrossFade("Walk", .07f, -1, 0);
            m_hitWanderPoint = false;
        }

        public override void UpdateLogic()
        {
            //If the enemy is facing left, move to the left, otherwise move to the right
            if (m_stateMachine.FacingLeft)
            {
                m_transform.position -= new Vector3(m_speed, 0, 0) * Time.deltaTime;
                m_transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                m_transform.position += new Vector3(m_speed, 0, 0) * Time.deltaTime;
                m_transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        public override void Exit()
        {
            base.Exit();

            StopAllCoroutines();
        }

        /// <summary>
        /// Creates the colliders based on the left and right points
        /// </summary>
        private void InstantiateWanderColliders(WanderPointTypeEnum type)
        {
            //Instantiate and add components
            GameObject point = new GameObject(gameObject.name + ": " + type.ToString());
            point.AddComponent<WanderPointType>();
            point.AddComponent<BoxCollider2D>();

            //Cache components
            WanderPointType pointType = point.GetComponent<WanderPointType>();
            BoxCollider2D boxCollider = point.GetComponent<BoxCollider2D>();
            Transform pointTransform = point.transform;

            //Set the type
            pointType.m_pointType = type;
            //Set the NPC object
            pointType.SetNPC(gameObject);

            //Set the size of the collider
            boxCollider.size = new Vector2(0.3f, 2.0f);

            //Set the collider to a trigger
            boxCollider.isTrigger = true;

            //Set the position of the gameobject based on the type of collider
            Vector2 newPos = new Vector2();
            newPos.x = type == WanderPointTypeEnum.LEFT_POINT ? m_leftPoint.x : m_rightPoint.x;
            newPos.y = m_transform.position.y;
            pointTransform.position = newPos;

            if (type == WanderPointTypeEnum.LEFT_POINT)
                m_leftCollider = point;
            else
                m_rightCollider = point;
        }

        /// <summary>
        /// Handles the event when the enemy walks into the stop points
        /// </summary>
        public virtual void HandlePointCollision()
        {
            m_hitWanderPoint = true;
        }

        /// <summary>
        /// Handles what happens when the NPC exits a wander point
        /// </summary>
        public virtual void HandleExitWanderPoint()
        {
            m_hitWanderPoint = false;
        }
    }
}
