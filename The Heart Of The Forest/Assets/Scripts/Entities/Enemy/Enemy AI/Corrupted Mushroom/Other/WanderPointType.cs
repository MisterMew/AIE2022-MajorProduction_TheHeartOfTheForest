/*
 * Date Created: 23/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using HotF.Enemy.CorruptedMushroom;
using UnityEngine;

namespace HotF.AI
{
    public enum WanderPointTypeEnum
    {
        LEFT_POINT, RIGHT_POINT
    }

    public class WanderPointType : MonoBehaviour
    {
        private GameObject m_NPC;
        private Wander m_wander;

        public WanderPointTypeEnum m_pointType;

        /// <summary>
        /// Sets the gameobject and the transform of the enemy
        /// </summary>
        /// <param name="NPC">The enemy object</param>
        public void SetNPC(GameObject NPC)
        {
            m_NPC = NPC;
            m_wander = NPC.GetComponent<Wander>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //If the collided object is the assigned enemy...
            if (col.gameObject == m_NPC)
            {
                //Call the handle point collision function
                m_wander.HandlePointCollision();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject == m_NPC)
            {
                //Call the handle exit wander point function
                m_wander.HandleExitWanderPoint();
            }
        }
    }
}
