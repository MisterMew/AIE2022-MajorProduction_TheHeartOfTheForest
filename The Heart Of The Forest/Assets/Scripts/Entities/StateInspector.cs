/*
 * Date Created: 22/08/2022
 * Author: Nicholas Connell
 */

using HotF.AI;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(State), true)]
public class StateInspector : Editor
{
    private SerializedProperty EnterEvent;
    private SerializedProperty ExitEvent;

    private bool showEvents = false;

    private void OnEnable()
    {
        EnterEvent = serializedObject.FindProperty("OnEnterState");
        ExitEvent = serializedObject.FindProperty("OnExitState");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.Space();
        showEvents = EditorGUILayout.BeginFoldoutHeaderGroup(showEvents, "Events");
        if (showEvents)
        {
            EditorGUILayout.PropertyField(EnterEvent);
            EditorGUILayout.PropertyField(ExitEvent);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif