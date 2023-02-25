/*
 * Date Created: 25/08/2022
 * Author: Nicholas Connell
 */

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace HotF.Enemy.CorruptedMushroom
{
    [CustomEditor(typeof(Corrupted_Attack))]
    public class Corrupted_AttackEditor : StateInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var t = target as Corrupted_Attack;

            DrawHandles(t);
        }

        void DrawHandles(Corrupted_Attack t)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(t.transform.position, Vector3.forward, t.AttackRadius);

        }
    }
}

#endif