using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

namespace HotF.Villager
{
    public class Villager_Climb : Climb
    {
        protected override void ChangeToAppropriateState()
        {
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WANDER);
        }
    }
}
