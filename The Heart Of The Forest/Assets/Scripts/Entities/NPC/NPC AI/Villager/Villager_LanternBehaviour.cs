/*
 * Date Created: 04/11/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

public class Villager_LanternBehaviour : NPC_State
{
    private VillagerLantern m_lantern;

    private Vector3 m_toLanternDirection;
    private Vector3 m_originalDir;

    [Tooltip("If this villager can re-light a lantern.")]
    [SerializeField] private bool m_canRelight = false;

    public bool CanReLight {get { return m_canRelight; } }

    public override void Enter()
    {
        base.Enter();

        m_animator.CrossFade("Lantern", 0.1f, -1, 0);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //If the animator time is greater than 1 AND the current animation being played is Lantern...
        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 &&
            m_animator.GetCurrentAnimatorStateInfo(0).IsName("Lantern"))
        {
            m_transform.eulerAngles = m_originalDir;

            //Change the state to wander
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WANDER);
        }
    }

    /// <summary>
    /// Makes the villager re-light the lantern
    /// </summary>
    /// <param name="lantern">The current lantern the villager is tampering with</param>
    public void RelightLantern(VillagerLantern lantern)
    {
        m_originalDir = m_transform.eulerAngles;

        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.LANTERN);

        //Set the lantern
        m_lantern = lantern;
        m_lantern.IsDimmed = false;
        m_lantern.RestartLantern();

        m_toLanternDirection = m_lantern.transform.position - m_transform.position;

        m_transform.eulerAngles = CalculateLookDirection(m_toLanternDirection.normalized);
    }

    /// <summary>
    /// Calculates the look direction
    /// </summary>
    Vector3 CalculateLookDirection(Vector3 dir)
    {
        //Calculate look rotation
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        //Set the x and z values to 0, and add 90 to the Y rotation
        Vector3 newRot = rot.eulerAngles;
        newRot.x = 0;
        newRot.y += 90;
        newRot.z = 0;
        return newRot;
    }
}
