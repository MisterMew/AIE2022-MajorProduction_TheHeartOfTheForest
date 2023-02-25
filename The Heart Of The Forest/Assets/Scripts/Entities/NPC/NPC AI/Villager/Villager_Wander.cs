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
    public class Villager_Wander : Wander
    {
        public override void HandlePointCollision()
        {
            base.HandlePointCollision();

            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.IDLE);
        }
    }
}
