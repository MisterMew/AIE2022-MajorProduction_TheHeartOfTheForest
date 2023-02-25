/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.AI
{
    public class Attack : NPC_State
    {
        protected virtual void InstantiateAttackTrigger()
        {

        }

        /// <summary>
        /// When the player enters the attack trigger
        /// </summary>
        public virtual void EnterAttackTrigger()
        {

        }

        /// <summary>
        /// While the player is in the attack trigger
        /// </summary>
        public virtual void StayAttackTrigger()
        {

        }

        /// <summary>
        /// When the player exits the attack trigger
        /// </summary>
        public virtual void ExitAttackTrigger()
        {

        }
    }
}

 