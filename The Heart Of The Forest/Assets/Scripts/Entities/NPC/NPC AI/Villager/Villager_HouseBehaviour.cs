/*
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using UnityEngine;

public class Villager_HouseBehaviour : NPC_State
{
    //The house the villager is walking into
    private VillagerHouse m_house;

    //The target position inside the house
    private Vector3 m_inHousePos;
    //The target position outside the house (Original position before the NPC enters)
    private Vector3 m_outHousePos;
    //The current target position the NPC is moving towards (either in house or out house)
    private Vector3 m_currentTargetPos;

    //The direction to the house
    private Vector3 m_toHouseDir;
    //The direction to the original position before the npc enters
    private Vector3 m_toOriginalDir;

    //The original scale of the villager before they walk in the house
    private float m_originalScale;

    private bool m_wasInHouse = false;
    private bool m_walkingToHouse = false;

    [Tooltip("The speed of the NPC")]
    [SerializeField] private float m_speed = 1.0f;
    [Tooltip("How long the NPC will stay inside the house.")]
    [SerializeField] private float m_houseTimer = 5.0f;

    [Tooltip("The scale of the villager when walking into the house")]
    [SerializeField] private float m_inHouseScale = 0.8f;
    [Tooltip("How long it takes the villagers to resize")]
    [SerializeField] private float m_resizeTimer = 3;

    //Getters and Setters
    public VillagerHouse House { get { return m_house; } set { m_house = value; } }

    public override void Enter()
    {
        base.Enter();

        m_originalScale = m_transform.lossyScale.x;
        m_walkingToHouse = false;
        m_animator.CrossFade("Walk", 0, -1, 0);
        StartCoroutine(ShrinkVillager());
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        m_transform.position = Vector3.MoveTowards(m_transform.position, m_currentTargetPos, m_speed * Time.deltaTime);

        if (!m_walkingToHouse && m_transform.position == m_inHousePos)
        {
            StartCoroutine(InHouse());
            m_house.ToggleDoor();
            m_walkingToHouse = true;
        }
        else if (m_wasInHouse && m_transform.position == m_outHousePos)
        {
            m_wasInHouse = false;
            m_house.ToggleDoor();
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WANDER);
        }
    }

    public override void Exit()
    {
        base.Exit();

        m_house.Occupied = false;
    }

    public void EnterHouseTrigger(VillagerHouse house)
    {
        m_stateMachine.ChangeStateByType(NPCStateTypeEnum.WALK_IN_HOUSE);

        m_house = house;
        m_house.Occupied = true;
        m_house.ToggleDoor();
        m_outHousePos = m_transform.position;

        //Get the in house position, but keep the Y axis.
        m_inHousePos = house.transform.position;
        m_inHousePos.y = m_transform.position.y;

        //Calculate to vectors
        m_toHouseDir = m_inHousePos - m_outHousePos;
        m_toOriginalDir = m_outHousePos - m_inHousePos;

        m_transform.eulerAngles = CalculateLookDirection(m_toHouseDir.normalized);

        m_currentTargetPos = m_inHousePos;
    }

    IEnumerator InHouse()
    {
        yield return new WaitForSeconds(m_houseTimer);

        
        m_wasInHouse = true;
        m_transform.eulerAngles = CalculateLookDirection(m_toOriginalDir.normalized);
        m_currentTargetPos = m_outHousePos;
        m_house.ToggleDoor();

        yield return new WaitForSeconds(2.3f);
        StartCoroutine(EnlargeVillager());
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

    IEnumerator ShrinkVillager()
    {
        Vector3 originalScale = m_transform.localScale;
        Vector3 newScale = new Vector3(m_inHouseScale, m_inHouseScale, m_inHouseScale);
        float waitTime = m_resizeTimer;
        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
            m_transform.localScale = Vector3.Lerp(originalScale, newScale, elapsedTime / waitTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator EnlargeVillager()
    {
        Vector3 originalScale = m_transform.localScale;
        Vector3 newScale = Vector3.one;
        float waitTime = m_resizeTimer;
        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
            m_transform.localScale = Vector3.Lerp(originalScale, newScale, elapsedTime / waitTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}