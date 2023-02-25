/*
 * Date Created: 21/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.CorruptedMushroom
{
    public class Corrupted_Stare : NPC_State
    {
        private Corrupted_Wander m_wander;
        private Corrupted_Chase m_chase;

        private bool m_playerJumpedOver;

        protected override void Awake()
        {
            base.Awake();

            //Cache components
            m_wander = GetComponent<Corrupted_Wander>();
            m_chase = GetComponent<Corrupted_Chase>();
        }

        public override void Enter()
        {
            base.Enter();

            m_animator.CrossFade("Idle", .07f, -1, 0);
            m_playerJumpedOver = false;
        }

        public override void UpdateLogic()
        {
            switch (m_stateMachine.FacingLeft)
            {
                //If the enemy is facing left
                case true:
                    //If the player has not jumped over, and the player is to the right of the enemy
                    if (!m_playerJumpedOver && m_playerTransform.position.x > m_transform.position.x)
                    {
                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                        m_playerJumpedOver = true;
                    }
                    break;
                //If the enemy is facing right
                case false:
                    //If the player has not jumped over, and the player is to the left of the enemy
                    if (!m_playerJumpedOver && m_playerTransform.position.x < m_transform.position.x)
                    {
                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                        m_playerJumpedOver = true;
                    }
                    break;
            }
        }
    }
}
