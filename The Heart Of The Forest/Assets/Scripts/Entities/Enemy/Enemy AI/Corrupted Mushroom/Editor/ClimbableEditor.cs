/*
 * Date Created: 29/08/2022
 * Author: Nicholas Connell
 */

using HotF.AI;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;

namespace HotF.AI
{
    [CustomEditor(typeof(ClimbableSurface))]
    public class ClimbableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var t = target as ClimbableSurface;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Inspector Tools");

            serializedObject.Update();

            ResetPoints(t);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var t = target as ClimbableSurface;

            EditorGUI.BeginChangeCheck();
            Vector3 leftPos = Handles.DoPositionHandle(t.m_leftPoint, Quaternion.identity);
            Vector3 rightPos = Handles.DoPositionHandle(t.m_rightPoint, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move points");
                t.m_leftPoint = leftPos;
                t.m_rightPoint = rightPos;
            }

            DrawHandles(t);
        }

        void DrawHandles(ClimbableSurface t)
        {
            Handles.color = Color.yellow;

            //Draw lines 
            Handles.DrawLine(t.m_leftPoint, t.m_leftPoint + Vector3.left);
            Handles.DrawLine(t.m_rightPoint, t.m_rightPoint + Vector3.right);

            Handles.color = Color.white;
            Vector3 leftMidPoint = new Vector3(t.transform.position.x, t.m_leftPoint.y, t.m_leftPoint.z);
            Vector3 rightMidPoint = new Vector3(t.transform.position.x, t.m_rightPoint.y, t.m_rightPoint.z);

            //Guiding lines for left point
            Handles.DrawLine(t.transform.position, leftMidPoint);
            Handles.DrawLine(leftMidPoint, t.m_leftPoint);

            //Guiding lines for right point
            Handles.DrawLine(t.transform.position, rightMidPoint);
            Handles.DrawLine(rightMidPoint, t.m_rightPoint);

            //Draw text
            Handles.Label(t.m_leftPoint, "LEFT POINT");
            Handles.Label(t.m_rightPoint, "RIGHT POINT");

        }

        void ResetPoints(ClimbableSurface t)
        {
            Vector3 lPoint = t.transform.position;
            lPoint.x -= 1;
            Vector3 rPoint = t.transform.position;
            rPoint.x += 1;

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Reset Climb Points"))
            {
                Debug.Log("Reset Climb Points");
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Reset Climb Points");
                t.m_leftPoint = lPoint;
                t.m_rightPoint = rPoint;
            }
        }
    }
}

