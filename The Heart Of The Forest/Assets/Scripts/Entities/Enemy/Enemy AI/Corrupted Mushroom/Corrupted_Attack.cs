/*
 * Date Created: 24/08/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.CorruptedMushroom
{
    [RequireComponent(typeof(Corrupted_StateMachine))]
    public class Corrupted_Attack : Attack
    {
        private AttackArea m_attackArea;
        private Corrupted_Climb climbState;

        [Header("Settings")]

        [Tooltip("The radius of the attack range")]
        [SerializeField ,Range(0, 5)] private float m_attackRadius;
        [Tooltip("The speed of the enemy while attacking")]
        [SerializeField] private float m_attackMovementSpeed = 1.0f;

        //Getters and Setters
        public float AttackRadius { get { return m_attackRadius; } set { m_attackRadius = value; } }
        public AttackArea AttackArea { get{return m_attackArea;} set{m_attackArea = value;} }

        protected override void Awake()
        {
            base.Awake();

            //Cache components
            climbState = GetComponent<Corrupted_Climb>();

            InstantiateAttackTrigger();
        }

        public override void Enter()
        {
            base.Enter();

            m_animator.CrossFade("Attack", 0, -1, 0);
        }

        public override void UpdateLogic()
        {
            if (m_playerTransform.position.x < m_transform.position.x)
            {
                m_transform.position -= new Vector3(m_attackMovementSpeed, 0, 0) * Time.deltaTime;
                m_transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                m_transform.position += new Vector3(m_attackMovementSpeed, 0, 0) * Time.deltaTime;
                m_transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void EnterAttackTrigger()
        {
            //Set the enemy state to Attack
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.ATTACK);

        }

        public override void ExitAttackTrigger()
        {
            //Set the enemy state to Chase
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);

        }

        protected override void InstantiateAttackTrigger()
        {
            //Instantiate and add components
            GameObject attackObj = new GameObject("Attack Area");
            attackObj.AddComponent<AttackArea>();
            attackObj.AddComponent<CircleCollider2D>();

            //Cache components
            Transform attackTransform = attackObj.GetComponent<Transform>();
            m_attackArea = attackObj.GetComponent<AttackArea>();
            CircleCollider2D col = attackObj.GetComponent<CircleCollider2D>();

            //Set parent and position
            attackTransform.SetParent(m_transform);
            attackTransform.position = m_transform.position;
            //Set the layer to "enemy"
            attackObj.layer = PLAYER_LAYER;
            //Set the size of the collider
            col.radius = m_attackRadius;
            //Set the collider to a trigger
            col.isTrigger = true;

            //Set attack area variables
            m_attackArea.SetCollider(col);
            m_attackArea.SetAttack(this);
        }

        /// <summary>
        /// Checks if the player is in the attack radius
        /// </summary>
        /// /// <returns>True if the distance between the enemy and the player is smaller than the radius</returns>
        public bool PlayerInAttackRadius()
        {
            //Get the distance
            float dist = Vector3.Distance(m_transform.position, m_playerTransform.position);

            return dist < m_attackRadius;
        }
    }
}
