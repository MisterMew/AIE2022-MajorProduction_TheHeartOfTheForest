/*
 * Date Created: 26/08/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.CorruptedMushroom
{
    [RequireComponent(typeof(Corrupted_StateMachine))]
    public class Corrupted_Climb : Climb
    {
        private Corrupted_Wander m_wander;
        private Corrupted_Chase m_chase;

        protected override void Awake()
        {
            base.Awake();

            //Cache components
            m_wander = GetComponent<Corrupted_Wander>();
            m_chase = GetComponent<Corrupted_Chase>();
        }

        public override void EnterClimbTrigger()
        {
            base.EnterClimbTrigger();

            switch (m_stateMachine.CurrentState)
            {
                //If the current state is chase
                case NPCStateTypeEnum.CHASE:
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.TIRED);
                    switch (ClimbableSurface.ClimbType)
                    {
                        //If the left is higher than the right
                        case ClimbSurfaceType.LEFT_IS_HIGHER_THAN_RIGHT:

                            if (m_stateMachine.FacingLeft)
                                m_stateMachine.ChangeStateByType(NPCStateTypeEnum.TIRED);
                            else
                                m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                            break;
                        //If the right is higher than the left
                        case ClimbSurfaceType.RIGHT_IS_HIGHER_THAN_LEFT:
                            if (!m_stateMachine.FacingLeft)
                                m_stateMachine.ChangeStateByType(NPCStateTypeEnum.TIRED);
                            else
                                m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                            break;
                    }
                    break;
                default:
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                    break;
            }
        }

        protected override void ChangeToAppropriateState()
        {
            switch (m_chase.PlayerInChaseRadius())
            {
                //If the player is in the chase radius
                case true:
                    InstantTurnAround();
                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                    break;
                //If the player is NOT in the chase radius
                case false:
                    m_stateMachine.FacingLeft = m_climbingToLeft;

                    switch (m_stateMachine.LastState)
                    {
                        //If the last state was chase
                        case NPCStateTypeEnum.CHASE:
                            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CONFUSED);
                            break;
                        //Set the state to wander by default
                        default:
                            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WANDER);
                            break;
                    }
                    break;
            }
        }

        protected override void InstantTurnAround()
        {
            //If the enemy was climbing to the left, and the player is on the right...
            if (m_climbingToLeft && m_playerTransform.position.x > m_transform.position.x)
            {
                //Climb to the right
                m_climbingToLeft = false;
                Enter();
            }
            //If the enemy was climbing to the right, and the player is on the left...
            else if (!m_climbingToLeft && m_playerTransform.position.x < m_transform.position.x)
            {
                //Climb to the left
                m_climbingToLeft = true;
                Enter();
            }
        }
    }
}
