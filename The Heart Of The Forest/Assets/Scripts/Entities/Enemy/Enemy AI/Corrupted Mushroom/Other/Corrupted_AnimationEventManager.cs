/*
 * Date Created: 29/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotF.AI;

namespace HotF.Enemy.CorruptedMushroom
{
    public class Corrupted_AnimationEventManager : MonoBehaviour
    {
        private Corrupted_StateMachine m_stateMachine;

        private void Awake()
        {
            m_stateMachine = GetComponentInParent<Corrupted_StateMachine>();
        }

        public void ChangeState(NPCStateTypeEnum state)
        {
            m_stateMachine.ChangeStateByType(state);
        }
    }
}
