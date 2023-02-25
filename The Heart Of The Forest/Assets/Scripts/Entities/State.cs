/*
 * Date Created: 22/08/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEngine.Events;

namespace HotF.AI
{
    public class State : MonoBehaviour
    {
        public const int PLAYER_LAYER = 6;
        public const int ENEMY_LAYER = 7;
        public const int ENEMY_PROJECTILE_LAYER = 8;
        public const int BURROWABLE_LAYER = 9;
        public const int ENEMY_NON_PLAYER_LAYER = 11;

        [Tooltip("When we enter the state, before any update is performed")]
        [HideInInspector] public UnityEvent OnEnterState;
        [Tooltip("When we exit the state")]
        [HideInInspector] public UnityEvent OnExitState;

        protected virtual void Awake() { }

        protected virtual void Start() { }

        /// <summary>
        /// Enters the state
        /// </summary>
        public virtual void Enter()
        {
            //Invoke unity event
            OnEnterState.Invoke();
        }

        /// <summary>
        /// Updates the logic of the state (updates every frame)
        /// </summary>
        public virtual void UpdateLogic() { }

        /// <summary>
        /// Exits the state
        /// </summary>
        public virtual void Exit()
        {
            //Invoke unity event
            OnExitState.Invoke();
        }
    }
}
