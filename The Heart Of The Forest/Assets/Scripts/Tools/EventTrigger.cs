/*
 * Date Created: 26.08.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace HotF.Tools
{
    /// <summary>
    /// Generic 2D trigger envents
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class EventTrigger : MonoBehaviour
    {
        [Tooltip("Event on enter")]
        public UnityEvent OnEnterEvent;
        [Tooltip("Event on exit")]
        public UnityEvent OnExitEvent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player") OnEnterEvent.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player") OnExitEvent.Invoke();
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for EventTrigger
    /// </summary>
    [CustomEditor(typeof(EventTrigger))]
    [CanEditMultipleObjects]
    public class EventTriggerEditor : Editor
    {
        /// <summary>
        /// This data
        /// </summary>
        public EventTrigger data;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable() => data = (EventTrigger)target;

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(data);

            // Invoke events
            if (GUILayout.Button("Invoke Enter")) data.OnEnterEvent.Invoke();
            if (GUILayout.Button("Invoke Exit")) data.OnExitEvent.Invoke();

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}