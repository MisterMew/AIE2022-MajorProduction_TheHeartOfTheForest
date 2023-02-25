/*
 * Date Created: 10/09/2022
 * Author: Nicholas Connell
 */

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HotF.AI
{
    [CustomEditor(typeof(Wander), true)]
    public class WanderEditor : StateInspector
    {
        [Tooltip("Locks the points in place when moving the NPC around")]
        private bool lockPoints;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var t = target as Wander;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Inspector Tools");

            serializedObject.Update();

            //ToggleLockPoints();

            ResetPoints(t);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var t = target as Wander;

            EditorGUI.BeginChangeCheck();
            Vector3 leftPos = Handles.PositionHandle(t.LeftPoint, Quaternion.identity);
            Vector3 rightPos = Handles.PositionHandle(t.RightPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move Left Handle");
                t.LeftPoint = leftPos;

                Undo.RecordObject(target, "Move Right Handle");
                t.RightPoint = rightPos;
            }

            DrawHandles(t);
        }

        void DrawHandles(Wander t)
        {
            Handles.color = Color.white;
            //Draw a line from the object to the handle
            Handles.DrawLine(t.transform.position, t.LeftPoint);
            Handles.DrawLine(t.transform.position, t.RightPoint);

            Handles.color = Color.green;
            Handles.DrawLine(t.LeftPoint, t.LeftPoint + new Vector3(0, 2, 0));
            Handles.DrawLine(t.RightPoint, t.RightPoint + new Vector3(0, 2, 0));

            //Label the handles
            Handles.Label(t.LeftPoint, t.name + ": Left point");
            Handles.Label(t.RightPoint, t.name + ": Right point");

            //Clamp the points to the centre of the enemy
            if (t.LeftPoint.x > t.transform.position.x)
            {
                Vector3 point = t.LeftPoint;
                point.x = t.transform.position.x;
                t.LeftPoint = point;
            }
            if (t.RightPoint.x < t.transform.position.x)
            {
                Vector3 point = t.RightPoint;
                point.x = t.transform.position.x;
                t.RightPoint = point;
            }

            //Clamp the z position of the handles
            Vector3 lPoint = t.LeftPoint;
            lPoint.y = t.transform.position.y;
            lPoint.z = t.transform.position.z;
            t.LeftPoint = lPoint;
            //Clamp the y position of the handles
            Vector3 rPoint = t.RightPoint;
            rPoint.y = t.transform.position.y;
            rPoint.z = t.transform.position.z;
            t.RightPoint = rPoint;
        }

        void ResetPoints(Wander t)
        {
            //Get and store left point
            Vector3 lPoint = t.transform.position;
            lPoint.x -= 3;
            //Get and store right point
            Vector3 rPoint = t.transform.position;
            rPoint.x += 3;

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Reset Wander Points"))
            {
                Debug.Log("Reset Points");
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Reset Points");
                t.LeftPoint = lPoint;
                t.RightPoint = rPoint;
            }
        }

        void ToggleLockPoints()
        {
            lockPoints = EditorGUILayout.Toggle("Lock Points to NPC", lockPoints);
        }
    }
}

#endif
