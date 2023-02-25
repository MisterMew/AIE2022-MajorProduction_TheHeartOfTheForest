/*
 * Date Created: 25/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.CorruptedMushroom
{
    /// <summary>
    /// Controls the chase area for the enemy Chase state
    /// </summary>
    public class ChaseArea : MonoBehaviour
    {
        private Collider2D m_collider;
        private Chase m_chase;

        //Getters and Setters
        public Collider2D Collider { get { return m_collider; } }

        public void SetCollider(Collider2D col) {m_collider = col;}
        public void SetChase(Chase chase) {m_chase = chase;}
        public void SetColliderActive(bool val) => m_collider.enabled = val;

        private void OnTriggerEnter2D(Collider2D col)
        {
            //if the collided object is the player...
            if (col.CompareTag("Player"))
            {
                m_chase.EnterChaseTrigger();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //If the player leaves the trigger...
            if (other.CompareTag("Player"))
            {
                m_chase.ExitChaseTrigger();
            }
        }
    }
}
