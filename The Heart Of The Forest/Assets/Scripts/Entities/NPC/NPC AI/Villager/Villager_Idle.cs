/*
 * Date Created: 09/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;


namespace HotF.Villager
{
    public class Villager_Idle : Idle
    {
        public override void Enter()
        {
            base.Enter();

            //If the last state was NOT idle, play the idle animation
            if (m_stateMachine.LastState != NPCStateTypeEnum.INTERACT)
                m_animator.CrossFade("Idle", 0.1f, -1, 0);
        }
    }
}
