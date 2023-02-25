/*
 * Date Created: 26/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotF.AI;

namespace HotF.Enemy.CorruptedMushroom
{
    public class AttackArea : MonoBehaviour
    {
        private Collider2D m_collider;
        private Attack m_attack;

        //Getters and Setters
        public Collider2D Collider { get { return m_collider; } }

        public void SetCollider(Collider2D collider) {m_collider = collider;}
        public void SetAttack(Attack attack) {m_attack = attack;}
        public void SetColliderActive(bool val) => m_collider.enabled = val;

        private void Awake()
        {
            gameObject.tag = "Enemy";
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //If the collided object is the player...
            if (col.CompareTag("Player"))
            {
                m_attack.EnterAttackTrigger();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            //If the player stays in the attack trigger...
            if (other.CompareTag("Player"))
            {
                m_attack.StayAttackTrigger();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //If the player leaves the trigger...
            if (other.CompareTag("Player"))
            {
                m_attack.ExitAttackTrigger();
            }
        }
    }
}
