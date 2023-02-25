/*
 * Date Created: 05/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.SporeEnemy
{
    public class Spore_Idle : Idle
    {
        public override void Enter()
        {
            base.Enter();

            if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                m_animator.CrossFade("Idle", 0.3f, -1, 0);
            }
        }
    }
}
