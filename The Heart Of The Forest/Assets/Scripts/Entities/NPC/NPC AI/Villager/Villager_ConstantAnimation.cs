/*
 * Date Created: 04/11/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

public class Villager_ConstantAnimation : NPC_State
{
    [SerializeField] private bool m_startWithConstant;

    [Tooltip("The name of the animation to play.")]
    [SerializeField] private string m_animName;

    public bool StartWithConstant { get{ return m_startWithConstant; } }

    public override void Enter()
    {
        base.Enter();

        m_animator.CrossFade(m_animName, 0, -1, 0);
    }
}
