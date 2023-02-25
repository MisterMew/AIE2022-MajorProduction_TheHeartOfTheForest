using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateYAxis : MonoBehaviour
{
    private Transform m_rotatingObject;

     [SerializeField] private float m_rotationSpeed = 1;

     private void Start()
     {
         m_rotatingObject = GetComponent<Transform>();
     }

     // Update is called once per frame
    void Update()
    {
        Vector3 rot = m_rotatingObject.eulerAngles;
        rot.y += m_rotationSpeed;
        transform.eulerAngles = rot;
    }
}
