/*
 * Date Created: 24/08/2022
 * Author: Nicholas Connell
 */

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace HotF.Enemy.CorruptedMushroom
{
    [CustomEditor(typeof(Corrupted_Chase))]
    public class CorruptedChaseEditor : StateInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI()
        {
            var t = target as Corrupted_Chase;

            DrawHandles(t);
        }

        /// <summary>
        /// Draw the handles for this inspector
        /// </summary>
        /// <param name="t">Chase state</param>
        void DrawHandles(Corrupted_Chase t)
        {
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(t.transform.position, Vector3.forward, t.ChaseRadius);
        }
    }
}

#endif
