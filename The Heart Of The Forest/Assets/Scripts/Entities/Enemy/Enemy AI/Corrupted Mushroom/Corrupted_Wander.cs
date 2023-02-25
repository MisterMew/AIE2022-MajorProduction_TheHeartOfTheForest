/*
 * Date Created: 22/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotF.AI;

namespace HotF.Enemy.CorruptedMushroom
{
    [RequireComponent(typeof(Corrupted_StateMachine))]
    public class Corrupted_Wander : Wander
    {
        private Corrupted_Chase m_chase;

        protected override void Awake()
        {
            base.Awake();

            m_chase = GetComponent<Corrupted_Chase>();
        }

        public override void HandlePointCollision()
        {
            base.HandlePointCollision();

            m_chase.StopChaseCoroutines();

            switch (m_stateMachine.CurrentState)
            {
                case NPCStateTypeEnum.WANDER:
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.IDLE);
                    break;
                case NPCStateTypeEnum.CHASE:
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.TIRED);
                    break;
            }
        }
    }
}
