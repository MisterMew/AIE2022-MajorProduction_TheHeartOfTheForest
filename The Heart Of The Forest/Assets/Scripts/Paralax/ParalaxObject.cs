/*
 * Date Created: 01/10/2022
 * Author: Nicholas Connell
 */

using System;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class ParalaxObject : MonoBehaviour
{
    private GameObject mainCam;

    private Transform m_transform;

    [Tooltip("-1 for objects to move faster than camera.\n" +
             "0 for objects to stay with camera.\n" +
             "1 for objects to move slower than camera.")]
    [SerializeField, Range(-1, 1)] private float paralaxValueX = 0;

    [Tooltip("-1 for objects to move faster than camera.\n" +
             "0 for objects to stay with camera.\n" +
             "1 for objects to move slower than camera.")]
    [SerializeField, Range(-1, 1)] private float paralaxValueY = 0;

    [Tooltip("If true, the Y value will be the same as the X value.")]
    [SerializeReference] private bool m_sameParalaxValues = false;

    [Tooltip("The order in layer which the sprites will be.")]
    [SerializeField] private int m_orderInLayer;

    [SerializeField, Tooltip("If we only want the paralax object to be horizontal")]
    private bool onlyHorizontal = true;

    void Awake()
    {
        mainCam = Camera.main.gameObject;
        m_transform = transform;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            //Get the distance between the objects and the camera
            float distanceX = mainCam.transform.position.x * paralaxValueX;
            float distanceY = mainCam.transform.position.y * paralaxValueY;

            if (onlyHorizontal)
                //Offset the position of this object by the distance
                m_transform.position = new Vector3(distanceX, transform.position.y, transform.position.z);
            else
            {
                if (m_sameParalaxValues)
                    m_transform.position = new Vector3(distanceX, distanceX, transform.position.z);
                else
                    m_transform.position = new Vector3(distanceX, distanceY, transform.position.z);
            }
        }
    }
#endif

    void FixedUpdate()
    {
        if (Application.isPlaying)
        {
            //Get the distance between the objects and the camera
            float distanceX = mainCam.transform.position.x * paralaxValueX;
            float distanceY = mainCam.transform.position.y * paralaxValueY;

            if (onlyHorizontal)
                //Offset the position of this object by the distance
                m_transform.position = new Vector3(distanceX, transform.position.y, transform.position.z);
            else
            {
                if (m_sameParalaxValues)
                    m_transform.position = new Vector3(distanceX, distanceX, transform.position.z);
                else
                    m_transform.position = new Vector3(distanceX, distanceY, transform.position.z);
            }
        }
    }

    public void SetOrders()
    {
        foreach (var obj in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            obj.sortingOrder = m_orderInLayer;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ParalaxObject)), CanEditMultipleObjects]
public class ParalaxObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = target as ParalaxObject;

        serializedObject.Update();

        SetOrder(t);

        serializedObject.ApplyModifiedProperties();
    }

    void SetOrder(ParalaxObject t)
    {
        if (GUILayout.Button("Set Sprite Layer Orders"))
        {
            t.SetOrders();
        }
    }
}
#endif
