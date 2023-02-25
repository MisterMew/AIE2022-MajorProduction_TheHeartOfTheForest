/*
 * Date Created: 05/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.SporeEnemy
{
    public class Spore_StateMachine : NPC_StateMachine
    {
        private Spore_Idle m_idle;
        private Spore_Attack m_attack;

        protected override void Awake()
        {
            //Cache components
            m_idle = GetComponent<Spore_Idle>();
            m_attack = GetComponent<Spore_Attack>();

            //Set state machines
            m_idle.SetStateMachine(this);
            m_attack.SetStateMachine(this);
        }

        protected override State GetInitialState()
        {
            CurrentState = NPCStateTypeEnum.IDLE;
            return m_idle;
        }

        public override void ChangeStateByType(NPCStateTypeEnum state)
        {
            LastState = CurrentState;

            switch (state)
            {
                case NPCStateTypeEnum.IDLE:
                    CurrentState = NPCStateTypeEnum.IDLE;
                    ChangeState(m_idle);
                    break;
                case NPCStateTypeEnum.ATTACK:
                    CurrentState = NPCStateTypeEnum.ATTACK;
                    ChangeState(m_attack);
                    break;
            }
        }
    }
}
