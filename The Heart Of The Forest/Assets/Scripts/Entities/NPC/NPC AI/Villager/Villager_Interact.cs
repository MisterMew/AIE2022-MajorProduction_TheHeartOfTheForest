/*
 * Date Created: 12/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.ShaderGraph.Internal;
#endif

namespace HotF.Villager
{
    public class Villager_Interact : NPC_State
    {
        private float elapsedTime;
        private float waitTime;

        [Tooltip("The last rotation of the NPC")]
        private Quaternion m_lastRotation;

        [Tooltip("The time it takes to rotate")]
        [SerializeField] private float m_rotationTime;

        [Tooltip("The transform of the villager to rotate")]
        [SerializeField] private Transform m_villagerMeshTransform;

        protected override void Awake()
        {
            base.Awake();

            //Get the last rotation of the NPC
            m_lastRotation = m_villagerMeshTransform.rotation;
        }

        public override void Enter()
        {
            base.Enter();

            //Start the rotation to the player
            StartCoroutine(RotateToPlayer());

            //Play the talking animation
            //if (m_stateMachine.LastState != NPCStateTypeEnum.IDLE)
            m_animator.CrossFade("Talking", 0.1f, -1, 0);
        }

        public override void Exit()
        {
            base.Exit();

            //Set the rotation to what it was before interacting
            //m_villagerMeshTransform.localRotation = m_lastRotation;
            StartCoroutine(RotateToOriginalRotation());
        }

        /// <summary>
        /// Rotates the NPC to the player
        /// </summary>
        IEnumerator RotateToPlayer()
        {
            elapsedTime = 0;
            waitTime = m_rotationTime;

            //Get the initial rotation of the villager
            Quaternion initialRot = m_villagerMeshTransform.rotation;

            //Get the facing direction to the player
            Vector3 toPlayerRotation = m_playerTransform.position - m_transform.position;
            Quaternion newRot = Quaternion.LookRotation(toPlayerRotation, Vector3.up);
            //Set the x and z values to 0
            newRot.x = 0;
            newRot.z = 0;

            while (elapsedTime < waitTime)
            {
                //Add to elapsed time
                elapsedTime += Time.deltaTime;

                //Lerp to the new rotation
                m_villagerMeshTransform.rotation = Quaternion.Slerp(initialRot, newRot, elapsedTime / waitTime);

                yield return null;
            }
        }

        IEnumerator RotateToOriginalRotation()
        {
            elapsedTime = 0;
            waitTime = m_rotationTime;

            //Get the initial rotation
            Quaternion initialRot = m_villagerMeshTransform.rotation;

            while (elapsedTime < waitTime)
            {
                //Add to elapsed time
                elapsedTime += Time.deltaTime;

                //Lerp to the original rotation
                m_villagerMeshTransform.rotation = Quaternion.Lerp(initialRot, m_lastRotation, elapsedTime / waitTime);

                yield return null;
            }

        }
    }
}
