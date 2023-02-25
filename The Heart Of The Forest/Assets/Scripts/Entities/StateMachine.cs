/*
 * Date Created: 22/08/2022
 * Author: Nicholas Connell
 */

using UnityEngine;

namespace HotF.AI
{
    public class StateMachine : MonoBehaviour
    {
        [Tooltip("The current state in use")]
        protected State m_currentState;
        [Tooltip("The last state this NPC was using before the current state")]
        protected State m_lastState;

        protected virtual void Awake() { }

        protected virtual void Start()
        {
            //Set the initial state
            m_currentState = GetInitialState();

            if (m_currentState != null)
            {
                //Enter the initial state
                m_currentState.Enter();
            }
        }

        private void Update()
        {
            //If the current state isn't null...
            if (m_currentState != null)
            {
                //Update the logic of the current state
                m_currentState.UpdateLogic();
            }
        }

        /// <summary>
        /// Switches between states
        /// </summary>
        public void ChangeState(State newState)
        {
            //Save the current state
            m_lastState = m_currentState;

            //Exit the current state
            if (m_currentState)
                m_currentState.Exit();

            //Set the new state
            m_currentState = newState;
            //Enter the new state
            m_currentState.Enter();
        }

        /// <summary>
        /// Gets the initial state for the state machine
        /// </summary>
        /// <returns>Initial state</returns>
        protected virtual State GetInitialState()
        {
            return null;
        }
    }
}