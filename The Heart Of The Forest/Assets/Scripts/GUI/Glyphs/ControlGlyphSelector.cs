using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF
{
	public class ControlGlyphSelector : MonoBehaviour
	{
		[SerializeField]
		private PlayerInput m_Input;

		[SerializeField, Tooltip("Name of the control scheme as it appears in input settings")]
		private string m_GamepadControlScheme = "Gamepad";

		[Header("Glyphs")]
		[SerializeField] private GlyphInfo m_KeyboardGlyph;
		[SerializeField] private GlyphInfo m_GamepadGlyph;

		public static bool CurrentSchemeIsGamepad { get; private set; } = false;

		/// <summary>
		/// Instance of this script in game scene
		/// </summary>
		private static ControlGlyphSelector s_Instance = null;

		private void Awake()
		{
			if (s_Instance)
			{
				enabled = false;
				return;
			}
			s_Instance = this;
		}

		private void Start() => _UpdateGlyphsToMatchKeybinds();

		private void OnDestroy()
		{
			if (s_Instance == this)
				s_Instance = null;
		}

		/*
		 *
		 * Controls are checked for changes manually in FixedUpdate.
		 * This is due to a bug with PlayerInput.onControlsChanged not being called
		 *	after a scene change.
		 * This is acknowledged & fixed by Unity, but only in later versions... the 2020.3 experience
		 *
		 */
		private void FixedUpdate()
		{
			bool isGamepad = m_Input.currentControlScheme == m_GamepadControlScheme;
			if (CurrentSchemeIsGamepad != isGamepad)
			{
				CurrentSchemeIsGamepad = isGamepad;
				GlyphShouldChange?.Invoke();
			}
		}

		private void ConvertPathToGlyphName(ref string binding)
		{
			binding = binding.ToLower();

			// Make Ctrl, Shift, Meta & Alt glyphs independent of side on keyboard
			if (binding.Contains("ctrl") || binding.Contains("shift") || binding.Contains("alt") || binding.Contains("meta"))
				binding = binding.Replace("left", "").Replace("right", "");

			// Check for which device to set binding for, and replace appropriately to match glyph sprite name
			if (binding.Contains("<keyboard>/"))
				binding = binding.Replace("<keyboard>/", "<\"keyboard_") + "\">";
			else if (binding.Contains("<gamepad>/"))
				binding = binding.Replace("<gamepad>/", "<\"gamepad_") + "\">";

			// Replace all forward slashes with underscores, for compatability with sprite naming conventions
			binding = binding.Replace('/', '_');
		}

		private void _UpdateGlyphsToMatchKeybinds()
		{
			Dictionary<string, string> keyboardBindings = new Dictionary<string, string>();
			Dictionary<string, string> gamepadBindings = new Dictionary<string, string>();

			foreach (InputActionMap map in m_Input.actions.actionMaps)
			{
				foreach (InputBinding binding in map.bindings)
				{
					string action = $"[{binding.action.ToLower()}]";
					string path = string.IsNullOrEmpty(binding.overridePath) ? binding.path : binding.overridePath;

					// Debug.Log($"Binding '{binding.action}' set to '{path}'");
					ConvertPathToGlyphName(ref path);

					if (path.Contains("axis") || string.IsNullOrEmpty(path))
						continue; // '1DAxis' or '2DAxis' - these don't have a direct mapping for glyphs

					var glyphBindings = path.Contains("keyboard") ? keyboardBindings : gamepadBindings;

					if (action.Equals("[move]"))
						action = "[move_horizontal]";

					if (glyphBindings.ContainsKey(action))
					{
						if(action.Equals("[move_horizontal]"))
							glyphBindings[action] += $"/ {path} ";
						else
							glyphBindings[action] += $" or {path}";
					}
					else
						glyphBindings.Add(action, path);
				}
			}

			foreach(var pair in keyboardBindings)
			{
				for (int i = 0; i < m_KeyboardGlyph.ReplacementPairs.Length; i++)
				{
					if (m_KeyboardGlyph.ReplacementPairs[i].Key.Equals(pair.Key))
					{
						m_KeyboardGlyph.ReplacementPairs[i].Value = pair.Value;
						break;
					}
				}
			}
			foreach(var pair in gamepadBindings)
			{
				for (int i = 0; i < m_GamepadGlyph.ReplacementPairs.Length; i++)
				{
					if (m_GamepadGlyph.ReplacementPairs[i].Key.Equals(pair.Key))
					{
						m_GamepadGlyph.ReplacementPairs[i].Value = pair.Value;
						break;
					}
				}
			}

			GlyphShouldChange?.Invoke();
		}

		/// <summary>
		/// Scans <paramref name="input"/>, replacing relevant text with their associated glyph
		/// </summary>
		/// <param name="input">Text to check for glyphs</param>
		/// <param name="formatting">Additional formatting options</param>
		/// <param name="shouldOverrideFormatting">When true, ignores <paramref name="formatting"/> for formatting and uses default options</param>
		/// <returns></returns>
		public static string ReplaceText(string input) =>
			(CurrentSchemeIsGamepad ? s_Instance.m_GamepadGlyph : s_Instance.m_KeyboardGlyph).Format(input, new GlyphInfo.FormattingOptions(), true);

		/// <summary>
		/// Scans <paramref name="input"/>, replacing relevant text with their associated glyph
		/// </summary>
		/// <param name="input">Text to check for glyphs</param>
		/// <param name="formatting">Additional formatting options</param>
		/// <param name="shouldOverrideFormatting">When true, ignores <paramref name="formatting"/> for formatting and uses default options</param>
		/// <returns></returns>
		public static string ReplaceText(string input, GlyphInfo.FormattingOptions formatting, bool useDefaultFormatting = true) =>
			(CurrentSchemeIsGamepad ? s_Instance.m_GamepadGlyph : s_Instance.m_KeyboardGlyph).Format(input, formatting, useDefaultFormatting);

		public static void UpdateGlyphsToMatchKeybinds() => s_Instance?._UpdateGlyphsToMatchKeybinds();

		public delegate void OnGlyphShouldChange();
		public static event OnGlyphShouldChange GlyphShouldChange;
	}
}