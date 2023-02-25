/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF.AI
{
    public class Chase : NPC_State
    {
        protected virtual void InstantiateChaseTrigger()
        {

        }

        public virtual bool PlayerInChaseRadius()
        {
            return false;
        }

        /// <summary>
        /// When the player enters the chase trigger
        /// </summary>
        public virtual void EnterChaseTrigger()
        {

        }

        /// <summary>
        /// When the player exits the chase trigger
        /// </summary>
        public virtual void ExitChaseTrigger()
        {

        }
    }
}

