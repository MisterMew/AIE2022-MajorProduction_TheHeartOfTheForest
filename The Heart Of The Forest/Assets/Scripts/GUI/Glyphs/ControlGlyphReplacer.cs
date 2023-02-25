/* 
 * Author: Lewis Comstive
 * 
 * CHANGE LOG:
 * 11/10/2022
 *	- Initial Document
 *
 */
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF
{
	[RequireComponent(typeof(TMP_Text))]
	public class ControlGlyphReplacer : MonoBehaviour
	{

		[Tooltip("When true, uses OverrideFormattingOptions instead of default formatting options")]
		public bool OverrideFormatting = true;

		public GlyphInfo.FormattingOptions OverrideFormattingOptions;

		private TMP_Text UIText;
		private string m_TextBeforeProcessing = string.Empty;

		private void Start()
		{
			UIText = GetComponent<TMP_Text>();
			m_TextBeforeProcessing = UIText.text;
			Execute();
		}

		private void OnEnable() => ControlGlyphSelector.GlyphShouldChange += OnGlyphShouldChange;
		private void OnDisable() => ControlGlyphSelector.GlyphShouldChange -= OnGlyphShouldChange;

		private void OnGlyphShouldChange() => Execute();

		/// <summary>
		/// Updates <see cref="UIText"/> value with <paramref name="text"/>, replacing matching text with glyph from <see cref="ControlGlyphSelector"/>
		/// </summary>
		public void Execute() => Execute(m_TextBeforeProcessing);

		/// <summary>
		/// Updates <see cref="UIText"/> value with <paramref name="text"/>, replacing matching text with glyph from <see cref="ControlGlyphSelector"/>
		/// </summary>
		public void Execute(string text)
		{
			if(UIText)
				UIText.text = ControlGlyphSelector.ReplaceText(text, OverrideFormattingOptions, !OverrideFormatting);
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(ControlGlyphReplacer))]
	public class ControlGlyphReplacerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			EditorGUILayout.HelpBox("Replaces text with glyph, such as:\n\n" +
									" Press [jump] to jump\n\n" +
									" Tap <\"keyboard_e\"> or <\"gamepad_x\"> to interact"
									, MessageType.Info);

			base.OnInspectorGUI();

			EditorGUILayout.Space();

			if (Application.isPlaying && GUILayout.Button("Refresh"))
				((ControlGlyphReplacer)target).Execute();
		}
	}
#endif
}