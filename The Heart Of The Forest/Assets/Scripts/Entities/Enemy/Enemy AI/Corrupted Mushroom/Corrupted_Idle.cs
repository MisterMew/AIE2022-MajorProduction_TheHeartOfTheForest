/*
 * Date Created: 22/08/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HotF.AI;

namespace HotF.Enemy.CorruptedMushroom
{
    [RequireComponent(typeof(Corrupted_StateMachine))]
    public class Corrupted_Idle : Idle
    {
        private Corrupted_Wander m_wander;

        protected override void Awake()
        {
            base.Awake();

            m_wander = GetComponent<Corrupted_Wander>();
        }

        public override void Enter()
        {
            base.Enter();

            switch (m_stateMachine.LastState)
            {
                case NPCStateTypeEnum.WANDER:
                    
                    break;
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
