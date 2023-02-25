/*
 * Date Created: 06.09.2022
 * Author: Jazmin Fazzolari
 * Desc: Pulled from previous projects to help with event systems
 */

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine.Events;
using UnityEditorInternal;

/// <summary>
/// Overrides the Unity Event drawer to allow for re-arranging instead of adding and removing
/// </summary>
[CustomPropertyDrawer(typeof(UnityEventBase), true)]
public class BaseCustomUnityEventDrawer : UnityEventDrawer {
    protected override void SetupReorderableList(ReorderableList list) {
        base.SetupReorderableList(list);

        list.draggable = true;
    }
}

#endif