/*
 * Date Created: 09/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotF.AI;

namespace HotF.Villager
{
    public class Villager_StateMachine : NPC_StateMachine
    {
        private Villager_Idle m_idle;
        private Villager_Wander m_wander;
        private Villager_Climb m_climb;
        private Villager_Interact m_interact;
        private Villager_HouseBehaviour m_house;
        private Villager_ConstantAnimation m_constantAnim;
        private Villager_LanternBehaviour m_lantern;

        protected override void Awake()
        {
            //Cache components
            m_idle = GetComponent<Villager_Idle>();
            m_wander = GetComponent<Villager_Wander>();
            m_climb = GetComponent<Villager_Climb>();
            m_interact = GetComponent<Villager_Interact>();
            m_house = GetComponent<Villager_HouseBehaviour>();
            m_constantAnim = GetComponent<Villager_ConstantAnimation>();
            m_lantern = GetComponent<Villager_LanternBehaviour>();


            //Set state machine for states
            m_idle.SetStateMachine(this);
            m_wander.SetStateMachine(this);
            m_climb.SetStateMachine(this);
            m_interact.SetStateMachine(this);
            m_house.SetStateMachine(this);
            m_constantAnim.SetStateMachine(this);
            m_lantern.SetStateMachine(this);
        }

        protected override State GetInitialState()
        {
            if (m_constantAnim.StartWithConstant)
            {
                CurrentState = NPCStateTypeEnum.CONSTANT;
                return m_constantAnim;
            }
            else
            {
                CurrentState = NPCStateTypeEnum.IDLE;
                return m_idle;
            }
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
                case NPCStateTypeEnum.WANDER:
                    CurrentState = NPCStateTypeEnum.WANDER;
                    ChangeState(m_wander);
                    break;
                case NPCStateTypeEnum.CLIMB:
                    CurrentState = NPCStateTypeEnum.CLIMB;
                    ChangeState(m_climb);
                    break;
                case NPCStateTypeEnum.INTERACT:
                    CurrentState = NPCStateTypeEnum.INTERACT;
                    ChangeState(m_interact);
                    break;
                case NPCStateTypeEnum.WALK_IN_HOUSE:
                    CurrentState = NPCStateTypeEnum.WALK_IN_HOUSE;
                    ChangeState(m_house);
                    break;
                case NPCStateTypeEnum.CONSTANT:
                    CurrentState = NPCStateTypeEnum.CONSTANT;
                    ChangeState(m_constantAnim);
                    break;
                case NPCStateTypeEnum.LANTERN:
                    CurrentState = NPCStateTypeEnum.LANTERN;
                    ChangeState(m_lantern);
                    break;
            }
        }

        public void ChangeToInteractState() => ChangeStateByType(NPCStateTypeEnum.INTERACT);

        public void ChangeToLastState()
        {
            if (LastState != NPCStateTypeEnum.INTERACT) ChangeStateByType(LastState);
            else ChangeStateByType(NPCStateTypeEnum.IDLE);
        }
    }
}
