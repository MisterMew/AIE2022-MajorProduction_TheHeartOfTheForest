/*
 * Date Created: 22/08/2022
 * Author: Nicholas Connell
 */

//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotF.AI;

namespace HotF.Enemy.CorruptedMushroom
{
    public class Corrupted_StateMachine : NPC_StateMachine
    {
        private Corrupted_Idle m_idle;
        private Corrupted_Wander m_wander;
        private Corrupted_Chase m_chase;
        private Corrupted_Climb m_climb;
        private Corrupted_Confused m_confused;
        private Corrupted_Tired m_tired;
        private Corrupted_Stare m_stare;

        protected override void Awake()
        {
            //Cache components
            m_idle = GetComponent<Corrupted_Idle>();
            m_wander = GetComponent<Corrupted_Wander>();
            m_chase = GetComponent<Corrupted_Chase>();
            m_climb = GetComponent<Corrupted_Climb>();
            m_confused = GetComponent<Corrupted_Confused>();
            m_tired = GetComponent<Corrupted_Tired>();
            m_stare = GetComponent<Corrupted_Stare>();

            //Set state machine for states
            m_idle.SetStateMachine(this);
            m_wander.SetStateMachine(this);
            m_chase.SetStateMachine(this);
            m_climb.SetStateMachine(this);
            m_confused.SetStateMachine(this);
            m_tired.SetStateMachine(this);
            m_stare.SetStateMachine(this);
        }

        protected override State GetInitialState()
        {
            CurrentState = NPCStateTypeEnum.WANDER;
            return m_wander;
        }

        public override void ChangeStateByType(NPCStateTypeEnum state)
        {
            //Save the last state of the enemy
            LastState = CurrentState;

            if (!m_climb.IsClimbing)
            {
                switch (state)
                {
                    case NPCStateTypeEnum.IDLE:
                        ChangeState(m_idle);
                        CurrentState = NPCStateTypeEnum.IDLE;
                        break;
                    case NPCStateTypeEnum.WANDER:
                        ChangeState(m_wander);
                        CurrentState = NPCStateTypeEnum.WANDER;
                        break;
                    case NPCStateTypeEnum.CHASE:
                        ChangeState(m_chase);
                        CurrentState = NPCStateTypeEnum.CHASE;
                        break;
                    case NPCStateTypeEnum.CLIMB:
                        ChangeState(m_climb);
                        CurrentState = NPCStateTypeEnum.CLIMB;
                        break;
                    case NPCStateTypeEnum.CONFUSED:
                        ChangeState(m_confused);
                        CurrentState = NPCStateTypeEnum.CONFUSED;
                        break;
                    case NPCStateTypeEnum.TIRED:
                        ChangeState(m_tired);
                        CurrentState = NPCStateTypeEnum.TIRED;
                        break;
                    case NPCStateTypeEnum.STARE:
                        ChangeState(m_stare);
                        CurrentState = NPCStateTypeEnum.STARE;
                        break;
                }
            }
        }
    }
}
