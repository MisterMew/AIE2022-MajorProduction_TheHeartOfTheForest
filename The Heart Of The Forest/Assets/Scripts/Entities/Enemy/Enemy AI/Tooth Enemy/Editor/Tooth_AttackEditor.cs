/*
 * Date Created: 02/09/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEditor;

namespace HotF.Enemy.ToothEnemy
{
    [CustomEditor(typeof(Tooth_Attack))]
    public class Tooth_AttackEditor : StateInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var t = target as Tooth_Attack;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tools");

            serializedObject.Update();

            ResetAttackTrigger(t);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var t = target as Tooth_Attack;

            EditorGUI.BeginChangeCheck();
            Vector3 pos = Handles.PositionHandle(t.AttackTriggerPosition, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move Point");
                t.AttackTriggerPosition = pos;
            }

            DrawHandles(t);
        }

        void DrawHandles(Tooth_Attack t)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireCube(t.AttackTriggerPosition, t.AttackTriggerSize);

            //Clamp the x and z position of the handle
            Vector3 point = t.AttackTriggerPosition;
            point.x = t.transform.position.x;
            point.z = t.transform.position.z;
            t.AttackTriggerPosition = point;
        }

        void ResetAttackTrigger(Tooth_Attack t)
        {
            Vector3 point = t.transform.position;
            point.y -= 3;

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Reset Attack Trigger Position"))
            {
                Debug.Log("Reset Attack Trigger Position");
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(t, "Reset trigger point");
                t.AttackTriggerPosition = point;
            }
        }
    }
}
