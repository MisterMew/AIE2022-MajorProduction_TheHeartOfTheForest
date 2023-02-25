/*
 * Date Created: 29.08.2022
 * Author: Unity
 * Contributors: Nghia
 *  - Nghia - Added this script to project
 */

/*
 * CHANGE LOG:
 * Nghia: changed variable names to be keyboard specific, added variables for gamepad controls
 */

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF
{
    /// <summary>
    /// A custom inspector for <see cref="RebindActionUI"/> which provides a more convenient way for
    /// picking the binding which to rebind.
    /// </summary>
    [CustomEditor(typeof(RebindActionUI))]
    [CanEditMultipleObjects]
    public class RebindActionUIEditor : UnityEditor.Editor
    {
        protected void OnEnable()
        {
            m_inputAction = serializedObject.FindProperty("inputActions"); // Added by Nghia
            m_isKeyboard = serializedObject.FindProperty("isKeyboard"); // Added by Nghia
            m_ActionProperty = serializedObject.FindProperty("m_Action"); // Added by Nghia
            m_BindingKeyboardIdProperty = serializedObject.FindProperty("m_BindingKeyboardId");
            m_BindingGamepadIdProperty = serializedObject.FindProperty("m_BindingGamepadId"); // Added by Nghia
            m_ActionLabelProperty = serializedObject.FindProperty("m_ActionLabel");
            m_BindingTextProperty = serializedObject.FindProperty("m_BindingText");
            m_RebindOverlayProperty = serializedObject.FindProperty("m_RebindOverlay");
            m_RebindTextProperty = serializedObject.FindProperty("m_RebindText");
            m_UpdateBindingUIEventProperty = serializedObject.FindProperty("m_UpdateBindingUIEvent");
            m_RebindStartEventProperty = serializedObject.FindProperty("m_RebindStartEvent");
            m_RebindStopEventProperty = serializedObject.FindProperty("m_RebindStopEvent");
            m_DisplayStringOptionsProperty = serializedObject.FindProperty("m_DisplayStringOptions");

            RefreshBindingOptions();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(m_inputAction); // Added by Nghia
            EditorGUILayout.PropertyField(m_isKeyboard); // Added by Nghia

            // Binding section.
            EditorGUILayout.LabelField(m_BindingKeyboardLabel, Styles.boldLabel);
            EditorGUILayout.LabelField(m_BindingGamepadLabel, Styles.boldLabel); // (Nghia)
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_ActionProperty);

                var newSelectedKeyboardBinding = EditorGUILayout.Popup(m_BindingKeyboardLabel, m_SelectedBindingKeyboardOption, m_BindingOptions);
                if (newSelectedKeyboardBinding != m_SelectedBindingKeyboardOption)
                {
                    var bindingId = m_BindingOptionValues[newSelectedKeyboardBinding];
                    m_BindingKeyboardIdProperty.stringValue = bindingId;
                    m_SelectedBindingKeyboardOption = newSelectedKeyboardBinding;
                }

                // (Nghia) start
                var newSelectedGamepadBinding = EditorGUILayout.Popup(m_BindingGamepadLabel, m_SelectedBindingGamepadOption, m_BindingOptions);
                if (newSelectedGamepadBinding != m_SelectedBindingGamepadOption)
                {
                    var bindingId = m_BindingOptionValues[newSelectedGamepadBinding];
                    m_BindingGamepadIdProperty.stringValue = bindingId;
                    m_SelectedBindingGamepadOption = newSelectedGamepadBinding;
                }
                // (Nghia) end

                var optionsOld = (InputBinding.DisplayStringOptions)m_DisplayStringOptionsProperty.intValue;
                var optionsNew = (InputBinding.DisplayStringOptions)EditorGUILayout.EnumFlagsField(m_DisplayOptionsLabel, optionsOld);
                if (optionsOld != optionsNew)
                    m_DisplayStringOptionsProperty.intValue = (int)optionsNew;
            }

            // UI section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_UILabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_ActionLabelProperty);
                EditorGUILayout.PropertyField(m_BindingTextProperty);
                EditorGUILayout.PropertyField(m_RebindOverlayProperty);
                EditorGUILayout.PropertyField(m_RebindTextProperty);
            }

            // Events section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(m_EventsLabel, Styles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(m_RebindStartEventProperty);
                EditorGUILayout.PropertyField(m_RebindStopEventProperty);
                EditorGUILayout.PropertyField(m_UpdateBindingUIEventProperty);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                RefreshBindingOptions();
            }
        }

        protected void RefreshBindingOptions()
        {
            var actionReference = (InputActionReference)m_ActionProperty.objectReferenceValue;
            var action = actionReference?.action;

            if (action == null)
            {
                m_BindingOptions = new GUIContent[0];
                m_BindingOptionValues = new string[0];
                m_SelectedBindingKeyboardOption = -1;
                m_SelectedBindingGamepadOption = -1; // (Nghia)
                return;
            }

            var bindings = action.bindings;
            var bindingCount = bindings.Count;

            m_BindingOptions = new GUIContent[bindingCount];
            m_BindingOptionValues = new string[bindingCount];
            m_SelectedBindingKeyboardOption = -1;
            m_SelectedBindingGamepadOption = -1; // (Nghia)

            var currentBindingKeyboardId = m_BindingKeyboardIdProperty.stringValue;
            var currentBindingGamepadId = m_BindingGamepadIdProperty.stringValue; // (Nghia)
            for (var i = 0; i < bindingCount; ++i)
            {
                var binding = bindings[i];
                var bindingId = binding.id.ToString();
                var haveBindingGroups = !string.IsNullOrEmpty(binding.groups);

                // If we don't have a binding groups (control schemes), show the device that if there are, for example,
                // there are two bindings with the display string "A", the user can see that one is for the keyboard
                // and the other for the gamepad.
                var displayOptions =
                    InputBinding.DisplayStringOptions.DontUseShortDisplayNames | InputBinding.DisplayStringOptions.IgnoreBindingOverrides;
                if (!haveBindingGroups)
                    displayOptions |= InputBinding.DisplayStringOptions.DontOmitDevice;

                // Create display string.
                var displayString = action.GetBindingDisplayString(i, displayOptions);

                // If binding is part of a composite, include the part name.
                if (binding.isPartOfComposite)
                    displayString = $"{ObjectNames.NicifyVariableName(binding.name)}: {displayString}";

                // Some composites use '/' as a separator. When used in popup, this will lead to to submenus. Prevent
                // by instead using a backlash.
                displayString = displayString.Replace('/', '\\');

                // If the binding is part of control schemes, mention them.
                if (haveBindingGroups)
                {
                    var asset = action.actionMap?.asset;
                    if (asset != null)
                    {
                        var controlSchemes = string.Join(", ",
                            binding.groups.Split(InputBinding.Separator)
                                .Select(x => asset.controlSchemes.FirstOrDefault(c => c.bindingGroup == x).name));

                        displayString = $"{displayString} ({controlSchemes})";
                    }
                }

                m_BindingOptions[i] = new GUIContent(displayString);
                m_BindingOptionValues[i] = bindingId;

                if (currentBindingKeyboardId == bindingId)
                    m_SelectedBindingKeyboardOption = i;
                if (currentBindingGamepadId == bindingId) // (Nghia)
                    m_SelectedBindingGamepadOption = i;
            }
        }

        private SerializedProperty m_inputAction; // Added by Nghia
        private SerializedProperty m_isKeyboard; // Added by Nghia
        private SerializedProperty m_ActionProperty;
        private SerializedProperty m_BindingKeyboardIdProperty;
        private SerializedProperty m_BindingGamepadIdProperty; // Added by Nghia
        private SerializedProperty m_ActionLabelProperty;
        private SerializedProperty m_BindingTextProperty;
        private SerializedProperty m_RebindOverlayProperty;
        private SerializedProperty m_RebindTextProperty;
        private SerializedProperty m_RebindStartEventProperty;
        private SerializedProperty m_RebindStopEventProperty;
        private SerializedProperty m_UpdateBindingUIEventProperty;
        private SerializedProperty m_DisplayStringOptionsProperty;

        private GUIContent m_BindingKeyboardLabel = new GUIContent("Binding Keyboard");
        private GUIContent m_BindingGamepadLabel = new GUIContent("Binding Gamepad"); // (Nghia)
        private GUIContent m_DisplayOptionsLabel = new GUIContent("Display Options");
        private GUIContent m_UILabel = new GUIContent("UI");
        private GUIContent m_EventsLabel = new GUIContent("Events");
        private GUIContent[] m_BindingOptions;
        private string[] m_BindingOptionValues;
        private int m_SelectedBindingKeyboardOption;
        private int m_SelectedBindingGamepadOption; // (Nghia)

        private static class Styles
        {
            public static GUIStyle boldLabel = new GUIStyle("MiniBoldLabel");
        }
    }
}
#endif
