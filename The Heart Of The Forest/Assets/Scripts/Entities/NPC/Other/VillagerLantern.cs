/*
 * Date Created: 04/11/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerLantern : MonoBehaviour
{
    [Tooltip("If the lantern is dimmed")]
    private bool m_isDimmed = false;

    [SerializeField] private GameObject m_flame1;
    [SerializeField] private GameObject m_flame2;

    [SerializeField] private float m_dimTimer = 12;

    public bool IsDimmed { get{ return m_isDimmed; } set{ m_isDimmed = value; } }

    private void Start()
    {
        StartCoroutine(DimLight());
    }

    /// <summary>
    /// Restarts the lantern dim timer
    /// </summary>
    public void RestartLantern()
    {
        m_isDimmed = false;
        m_flame1.SetActive(true);
        m_flame2.SetActive(true);

        StartCoroutine(DimLight());
    }

    /// <summary>
    /// Dims the lantern lights after a time
    /// </summary>
    IEnumerator DimLight()
    {
        yield return new WaitForSeconds(m_dimTimer);

        //Deactivate the wisp flames
        m_flame1.SetActive(false);
        m_flame2.SetActive(false);

        //Set the lantern to dimmed
        m_isDimmed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Villager") && m_isDimmed)
        {
            if (!other.GetComponentInParent<Villager_LanternBehaviour>().CanReLight) return;

            other.GetComponentInParent<Villager_LanternBehaviour>().RelightLantern(this);
        }
    }
}
