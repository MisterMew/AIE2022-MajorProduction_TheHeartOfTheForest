/*
 * Date Created: 20/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotF.AI;

namespace HotF.Enemy.CorruptedMushroom
{
    public class Corrupted_Tired : NPC_State
    {
        private Corrupted_Chase m_chase;
        private Corrupted_Climb m_climb;
        private Corrupted_Wander m_wander;

        [Tooltip("How long the enemy will be tired for, before chasing again")]
        [SerializeField, Range(.1f, 5)] private float m_tiredTimer = 2f;

        protected override void Awake()
        {
            base.Awake();

            //Cache components
            m_chase = GetComponent<Corrupted_Chase>();
            m_climb = GetComponent<Corrupted_Climb>();
            m_wander = GetComponent<Corrupted_Wander>();
        }

        public override void Enter()
        {
            base.Enter();

            //Play tired animation
            m_animator.CrossFade("Tired", .1f, -1, 0);

            //Start the tired timer coroutine
            StartCoroutine(TiredTimer());
        }

        public override void UpdateLogic()
        {
            
        }

        IEnumerator TiredTimer()
        {
            yield return new WaitForSeconds(m_tiredTimer);
            ChangeToAppropriateState();
        }

        /// <summary>
        /// Changes to the correct state after the enemy has caught its breath
        /// </summary>
        void ChangeToAppropriateState()
        {
            switch (m_climb.ReadyToClimb)
            {
                //If the enemy is in the climb trigger
                case true:
                    switch (m_chase.PlayerInChaseRadius())
                    {
                        //If the player is in the chase radius
                        case true:
                            switch (m_climb.ClimbableSurface.ClimbType)
                            {
                                //If left is higher than right
                                case ClimbSurfaceType.LEFT_IS_HIGHER_THAN_RIGHT:
                                    if (m_playerTransform.position.x > m_transform.position.x)
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                                    else
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                                    break;
                                //If right is higher than left
                                case ClimbSurfaceType.RIGHT_IS_HIGHER_THAN_LEFT:
                                    if (m_playerTransform.position.x < m_transform.position.x)
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                                    else
                                        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                                    break;
                            }
                            break;
                        //If the player is NOT is in the chase radius
                        case false:
                            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CLIMB);
                            break;
                    }
                    break;
                //If the enemy is NOT in the climb trigger
                case false:
                    switch (m_chase.PlayerInChaseRadius())
                    {
                        //If the player is in the chase radius
                        case true:
                            switch (m_wander.HitWanderPoint)
                            {
                                //If the enemy just hit a wander point
                                case true:
                                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.STARE);
                                    break;
                                case false:
                                    m_stateMachine.ChangeStateByType(NPCStateTypeEnum.CHASE);
                                    break;
                            }
                            break;
                        //If the player is NOT in the chase radius
                        case false:
                            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.IDLE);
                            break;
                    }
                    break;
            }
        }
    }
}
