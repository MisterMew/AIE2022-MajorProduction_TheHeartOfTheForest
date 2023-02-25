/*
 * Date Created: 01/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.Enemy.CorruptedMushroom;
using Unity.Mathematics;
using UnityEngine;

namespace HotF.AI
{
    public class NPC_State : State
    {
        protected NPC_StateMachine m_stateMachine;

        [Tooltip("The animator being used by this NPC")]
        protected Animator m_animator;
        [Tooltip("The transform of this NPC")]
        protected Transform m_transform;
        [Tooltip("The rigid body of this NPC")]
        protected Rigidbody2D m_rigidBody;

        [Tooltip("The player object")]
        protected GameObject m_player;
        [Tooltip("The transform of the player object")]
        protected Transform m_playerTransform;


        public void SetStateMachine(NPC_StateMachine stateMachine) { m_stateMachine = stateMachine; }


        protected override void Awake()
        {
            //Cache components
            m_transform = transform;
            m_animator = m_transform.GetComponentInChildren<Animator>();
            m_rigidBody = GetComponent<Rigidbody2D>();

            //Cache player components
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_playerTransform = m_player.transform;
        }
    }
}
