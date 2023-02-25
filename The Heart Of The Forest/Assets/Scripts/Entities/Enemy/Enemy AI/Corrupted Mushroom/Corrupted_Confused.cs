/*
 * Date Created: 29/08/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Enemy.CorruptedMushroom
{
    public class Corrupted_Confused : NPC_State
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Enter()
        {
            base.Enter();

            m_animator.CrossFade("Confused", .2f, -1, 0);
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
