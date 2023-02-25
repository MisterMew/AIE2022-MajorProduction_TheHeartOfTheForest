/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using UnityEngine;

namespace HotF.AI
{
    public class NPC_StateMachine : StateMachine
    {
        [Tooltip("The current state of this NPC")]
        private NPCStateTypeEnum currentState;
        [Tooltip("The state this NPC was in before the current state")]
        private NPCStateTypeEnum lastState;

        [Tooltip("True of the NPC is facing to the left")]
        private bool m_facingLeft;

        //Getters and Setters
        public NPCStateTypeEnum CurrentState { get { return currentState; } set {currentState = value;} }
        public NPCStateTypeEnum LastState { get { return lastState; } set {lastState = value;} }
        public bool FacingLeft { get { return m_facingLeft; } set { m_facingLeft = value; } }

        /// <summary>
        /// Swaps the facing left variable
        /// </summary>
        public void SwapFacingDirection()
        {
            m_facingLeft = m_facingLeft ? m_facingLeft = false : m_facingLeft = true;
        }

        /// <summary>
        /// Changes the enemy state
        /// </summary>
        /// <param name="state">The state to change to</param>
        public virtual void ChangeStateByType(NPCStateTypeEnum state)
        {

        }
    }
}
