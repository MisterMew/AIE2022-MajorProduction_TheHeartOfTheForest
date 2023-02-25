/*
 * Date Created: 02/10/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VillagerHouse : MonoBehaviour
{
    private bool m_open = false;
    private Transform m_doorTransform;

    [Tooltip("If this house is occupied")]
    private bool m_occupied = false;

    [Tooltip("The door gameobject (origin)")]
    [SerializeField] private GameObject m_door;

    [Tooltip("The time it takes to open/close the door")]
    [SerializeField] private float m_doorTimer = .5f;

    //Getters and Setters
    public bool Occupied { get { return m_occupied; } set { m_occupied = value; } }

    private void Awake()
    {
        m_doorTransform = m_door.transform;
    }

    public void ToggleDoor()
    {
        StartCoroutine(ToggleDoorEnumerator());
    }

    IEnumerator ToggleDoorEnumerator()
    {
        float elapsedTime = 0;
        float waitTime = m_doorTimer;

        Vector3 fromRot = m_doorTransform.localEulerAngles;
        Vector3 toRot;

        //If the door is open...
        if (m_open)
        {
            toRot = new Vector3(0, 0, 0);
            m_open = false;
        }
        //Otherwise, if the door is closed
        else
        {
            toRot = new Vector3(0, 90, 0);
            m_open = true;
        }

        while (elapsedTime < waitTime)
        {
            m_doorTransform.localEulerAngles = Vector3.Slerp(fromRot, toRot, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If a villager enters the trigger, and the house is no occupied
        if (other.CompareTag("Villager") && !m_occupied)
        {
            other.GetComponentInParent<Villager_HouseBehaviour>().EnterHouseTrigger(this);
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(VillagerHouse))]
public class VillagerHouseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = target as VillagerHouse;

        serializedObject.Update();

        EditorGUILayout.Space();

        ToggleDoor(t);

        serializedObject.ApplyModifiedProperties();
    }

    void ToggleDoor(VillagerHouse t)
    {
        if (GUILayout.Button("Open/Close Door"))
        {
            t.ToggleDoor();
        }
    }
}
#endif
