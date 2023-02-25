/*
 * 
 * Date Created: 06.09.2022
 * Author: Lewis Comstive
 *
 */

/*
 * CHANGE LOG:
 * Nghia: added call to TriggerInteractedEvent in Interact(), added OnTriggerExit2D() to give player back control, added OnTriggerEnter2D()
 */

using TMPro;
using System;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF.Dialogue
{
	public class Dialogue : Interactable.Interactable
	{
		public enum ConversationBehaviour
		{
			[Tooltip("Stops conversations after final one is finished")]
			None,

			[Tooltip("Loops through conversations in order")]
			Looping,

			[Tooltip("Goes through all conversations in a random order")]
			Random
		}

		[Serializable]
		public struct Conversation
		{
			public string[] Lines;
		}

		[SerializeField, Tooltip("Prompt to show when continuing conversation")]
		private string continuePrompt = "Continue";

		[SerializeField, Tooltip("Text to show conversation")]
		private TMP_Text text;

		[SerializeField, Tooltip("Automatically start conversation when in front of trigger")]
		private bool autoConverse = false;

		[SerializeField, Tooltip("All potential conversations")]
		private Conversation[] conversations;

		[SerializeField, Tooltip("Conversation choosing behaviour")]
		private ConversationBehaviour conversationBehaviour = ConversationBehaviour.None;

		[Header("Preprocessing")]
		[SerializeField]
		private bool overridePostProcessing = false;

		[SerializeField]
		public GlyphInfo.FormattingOptions OverrideFormattingOptions;

		[Header("Events")]
		[SerializeField] private UnityEvent conversationStarted;
		[SerializeField] private UnityEvent conversationEnded;
		[SerializeField] private UnityEvent OnEnter; // added by Nghia

		/// <summary>
		/// When set to false, the conversation is not being shown
		/// </summary>
		private bool conversationActive = false;

		/// <summary>
		/// Index of conversation last activated.
		/// This is used for multiple conversations, depending
		/// </summary>
		[HideInInspector]
		public int conversationIndex = 0;

		/// <summary>
		/// Tracks which line was last shown in a conversation.
		/// Reset to 0 when conversation is inactive.
		/// </summary>
		private int lineIndex = -1;

		public Conversation CurrentConversation => conversations.Length > conversationIndex ? conversations[conversationIndex] : new Conversation();

		public override bool CanInteract => conversationIndex >= 0 && conversationIndex < conversations.Length;

		/// <summary>
		/// Game controller
		/// </summary>
		private GameController gameController;

		protected override void Start()
		{
			base.Start();
			ShowConversation(false);
			gameController = FindObjectOfType<GameController>();
		}

		private void OnEnable()  => ControlGlyphSelector.GlyphShouldChange += OnGlyphShouldChange;
		private void OnDisable() => ControlGlyphSelector.GlyphShouldChange -= OnGlyphShouldChange;

		/// <summary>
		/// Trigger event on enter
		/// </summary>
		/// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
			if (collision.tag == "Player")
			{
				if (autoConverse) Interact(FindObjectOfType<Player.PlayerHealth>().gameObject); // added by Nghia
			}
		}

        /// <summary>
        /// Give player back control if they leave the collider
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerExit2D(Collider2D collision)
        {
			if (collision.tag == "Player")
			{
				gameController?.SetPlayerControlsState(true); // added by Nghia
			}
		}

        public override void OnPlayerEnteredTrigger(GameObject interactor)
		{
			// Check if player can interact
			if (!CanInteract)
				return;

			if (conversationActive)
				hud.OpenMessagePanel(continuePrompt);
			else
				hud.OpenMessagePanel(hudMessage);
		}

		public override void OnPlayerExitedTrigger(GameObject interactor)
		{
			base.OnPlayerExitedTrigger(interactor);
			ShowConversation(false);
		}

		public override void Interact(GameObject interactor)
		{
			if (!CanInteract)
				return; // No valid conversation to show

			// Continue conversation
			if (conversationActive)
				NextLine();

			// Start conversation
			else
				ShowConversation(true);

			// Update HUD text
			OnPlayerEnteredTrigger(interactor);
		}

		private void ShowConversation(bool show)
		{
			// Reset variables
			lineIndex = -1;
			text.text = string.Empty;

			if (show)
			{
				conversationStarted?.Invoke();

				// Show first line
				NextLine();
			}
			else
				conversationEnded?.Invoke();

			conversationActive = show;
		}

		private void NextLine()
		{
			lineIndex++;

			// Check if conversation finished
			if (lineIndex >= CurrentConversation.Lines.Length)
			{
				ShowConversation(false);
				SelectNextConversation();
			}
			else
			{
				// Update conversation text
				text.text = ControlGlyphSelector.ReplaceText(CurrentConversation.Lines[lineIndex], OverrideFormattingOptions, !overridePostProcessing);
				TriggerInteractedEvent();
			}

		}

		private void SelectNextConversation()
		{
			// Choose random conversation
			if(conversationBehaviour == ConversationBehaviour.Random)
			{
				conversationIndex = UnityEngine.Random.Range(0, conversations.Length);
				return;
			}

			// Choose next conversation in sequence
			conversationIndex++;

			// Loop conversation
			if (conversationIndex >= conversations.Length &&
				conversationBehaviour == ConversationBehaviour.Looping)
				conversationIndex = 0;

			if (conversationIndex >= conversations.Length &&
				conversationBehaviour == ConversationBehaviour.None)
				hud.CloseMessagePanel();
		}

		private void OnGlyphShouldChange()
		{
			if (conversationActive && lineIndex < CurrentConversation.Lines.Length)
				text.text = ControlGlyphSelector.ReplaceText(CurrentConversation.Lines[lineIndex], OverrideFormattingOptions, !overridePostProcessing);
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(Dialogue))]
		private class DialogueEditor : Editor
		{
			private Dialogue _target;

			private void OnEnable() => _target = (Dialogue)target;

			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();

				if (Application.isPlaying && _target.overridePostProcessing && GUILayout.Button("Refresh Pre Processing"))
					_target.OnGlyphShouldChange();
			}
		}
#endif
	}
}