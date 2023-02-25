/*
 * Date Created: 04/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using HotF.Enemy.CorruptedMushroom;
using UnityEngine;

namespace HotF.Enemy.ToothEnemy
{
    public class Tooth_PostAttack : NPC_State
    {
        private Vector2 m_originalPosition;

        private Tooth_Attack m_attack;

        [Tooltip("The speed the Tooth enemy rises")]
        [SerializeField] private float m_raiseSpeed;

        protected override void Awake()
        {
            base.Awake();

            StartCoroutine(SetOriginalPosition());

            //Cache objects
            m_attack = GetComponent<Tooth_Attack>();
        }

        public override void Enter()
        {
            base.Enter();

            //Set the velocity
            m_rigidBody.velocity = Vector2.up * m_raiseSpeed;
        }

        public override void UpdateLogic()
        {
            //If the current y position is greater than or equal to the original y position...
            if (m_transform.position.y >= m_originalPosition.y)
            {
                //Set the state to idle
                m_stateMachine.ChangeStateByType(NPCStateTypeEnum.IDLE);
            }

            //Set the tendril position
            m_attack.Tendril.transform.position = m_attack.TendrilStartPosition;

            //Set the tendril to the correct scale
            Vector3 tendrilScale = m_attack.Tendril.transform.lossyScale;
            tendrilScale.y = Vector3.Distance(m_attack.TendrilStartPosition, m_transform.position) / 2;
            m_attack.Tendril.transform.localScale = tendrilScale;
        }

        public override void Exit()
        {
            base.Exit();

            m_rigidBody.velocity = Vector2.zero;
        }

        /// <summary>
        /// Sets the original position variable
        /// </summary>
        IEnumerator SetOriginalPosition()
        {
            yield return new WaitForEndOfFrame();

            //Cache the original position of the enemy
            //(We wait for the end of the first frame to do this just in-case
            //the collider was originally set inside a wall, and was pushed out. This way,
            //it will, get the position after the collider has been pushed out.)
            m_originalPosition = m_transform.position;
        }
    }
}
