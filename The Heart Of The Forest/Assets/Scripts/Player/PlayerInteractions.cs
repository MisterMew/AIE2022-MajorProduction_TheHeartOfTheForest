/*
 * Date Created: 22.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

/*
 * CHANGE LOG:
 * Nghia: Changed OnDestroy to OnDisable
 *  
 */

using UnityEngine;
using HotF.Interactable;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Hotf.Player.Interactions
{
	/// <summary>
	/// Handles the players interactions with items and entities
	/// </summary>
	public class PlayerInteractions : MonoBehaviour
	{
		[Header("Key Binds")]
		[SerializeField] private InputActionReference interactionKeyBind;

		/// <summary>
		/// All interactables that the player can currently interact with.
		/// The top of the stack, being the most recent encountered, is selected.
		/// </summary>
		private List<Interactable> interactables = new List<Interactable>();

		/// <summary>
		/// Currently selected interactable object the player is near.
		/// If multiple are nearby, the most recently entered trigger is the selected.
		/// </summary>
		[field: SerializeField]
		public Interactable SelectedInteractable { get; private set; } = null;

		private void OnEnable() => interactionKeyBind.action.started += OnKeyPressed; // Listen for keypress event (subscribe)

		private void OnDisable() => interactionKeyBind.action.started -= OnKeyPressed; // Unsubscribe

		/* Methods */
		private void OnTriggerEnter2D(Collider2D other)
		{
			// Validate other is an interactable object
			if (!other.TryGetComponent(out Interactable interactable))
				return;
			interactables.Add(interactable);

			// Set as interactable
			UpdateInteractable();
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.TryGetComponent(out Interactable interactable))
				return;

			// If this interactable is the selected, inform it of deselection
			if (interactable == SelectedInteractable)
			{
				interactable.OnPlayerExitedTrigger(gameObject);
				SelectedInteractable = null;
			}
			
			// Remove from list
			interactables.Remove(interactable);

			// If this interactable is the selected, change selected to next most recent
			UpdateInteractable();
		}

		private void UpdateInteractable()
		{
			if (interactables.Count > 0 && interactables[interactables.Count - 1] == SelectedInteractable)
				return; // Already correct interactable, don't need to continue

			// Inform currently selected that it has been deselected
			SelectedInteractable?.OnPlayerExitedTrigger(gameObject);

			// Update selected
			for (int i = interactables.Count - 1; i >= 0; i--)
			{
				// Make sure interactable can be interacted with
				if (interactables[i] && interactables[i].CanInteract)
				{
					SelectedInteractable = interactables.Count > 0 ? interactables[interactables.Count - 1] : null;
					break;
				}
			}

			// Inform newly selected that is has been selected
			SelectedInteractable?.OnPlayerEnteredTrigger(gameObject);
		}

		/// <summary>
		/// If <see cref="interactable"/> exists, calls <see cref="Interactable.Interact(GameObject)"/>
		/// </summary>
		private void OnKeyPressed(InputAction.CallbackContext _)
		{
			for (int i = interactables.Count - 1; i >= 0; i--)
				interactables[i].Interact(gameObject);
		}
	}
}