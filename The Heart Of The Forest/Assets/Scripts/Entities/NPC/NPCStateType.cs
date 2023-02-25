/*
 * Date Created: 25/08/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF.AI
{
    /// <summary>
    /// The state of the enemy
    /// </summary>
    public enum NPCStateTypeEnum
    {
        IDLE, WANDER, CHASE, ATTACK, POST_ATTACK, CLIMB, CONFUSED, 
        INTERACT, TIRED, CONVERSATE, STARE, WALK_IN_HOUSE, CONSTANT, LANTERN
    }

    public class NPCStateType : MonoBehaviour
    {
        public NPCStateTypeEnum m_stateType;
    }
}

