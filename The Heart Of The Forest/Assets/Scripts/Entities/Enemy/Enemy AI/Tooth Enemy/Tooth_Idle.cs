/*
 * Date Created: 02/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.ToothEnemy
{
    public class Tooth_Idle : Idle
    {
        public override void Enter()
        {
            base.Enter();

            //Play the idle animation
            m_animator.CrossFade("Idle", 0.3f, -1, 0);
        }
    }
}

