/*
 * Date Created: 02/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.ToothEnemy
{
    public class Tooth_StateMachine : NPC_StateMachine
    {
        private Tooth_Idle m_idle;
        private Tooth_Attack m_attack;
        private Tooth_PostAttack m_postAttack;

        protected override void Awake()
        {
            //Cache components
            m_idle = GetComponent<Tooth_Idle>();
            m_attack = GetComponent<Tooth_Attack>();
            m_postAttack = GetComponent<Tooth_PostAttack>();

            //Set state machine for states
            m_idle.SetStateMachine(this);
            m_attack.SetStateMachine(this);
            m_postAttack.SetStateMachine(this);
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
                    ChangeState(m_idle);
                    CurrentState = NPCStateTypeEnum.IDLE;
                    break;
                case NPCStateTypeEnum.ATTACK:
                    ChangeState(m_attack);
                    CurrentState = NPCStateTypeEnum.ATTACK;
                    break;
                case NPCStateTypeEnum.POST_ATTACK:
                    ChangeState(m_postAttack);
                    CurrentState = NPCStateTypeEnum.POST_ATTACK;
                    break;
            }
        }
    }
}
