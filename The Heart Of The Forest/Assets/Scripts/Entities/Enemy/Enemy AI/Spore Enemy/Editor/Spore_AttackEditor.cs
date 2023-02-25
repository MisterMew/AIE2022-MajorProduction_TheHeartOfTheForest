/*
 * Date Created: 05/09/2022
 * Author: Nicholas Connell
 */

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HotF.Enemy.SporeEnemy
{
    [CustomEditor(typeof(Spore_Attack))]
    public class Spore_AttackEditor : StateInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var t = target as Spore_Attack;

            DrawHandles(t);
        }

        void DrawHandles(Spore_Attack t)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(t.transform.position, Vector3.forward, t.AttackRadius);

        }
    }
}

#endif
