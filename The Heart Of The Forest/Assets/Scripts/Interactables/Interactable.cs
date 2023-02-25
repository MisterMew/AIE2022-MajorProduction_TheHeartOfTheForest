using HotF.Hud;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Interactable
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
		[field: SerializeField]
		[field: Tooltip("Displayed message when in range of interactable object.")]
		public string hudMessage { get; set; }

		[Tooltip("Executed when interaction occurs")]
		public UnityEvent OnInteracted;

		protected HUDManager hud;

		protected virtual void Start() => hud = FindObjectOfType<HUDManager>();

		public virtual bool CanInteract { get; protected set; } = true;

		public virtual void OnPlayerEnteredTrigger(GameObject interactor)
		{
			if(!hud) hud = FindObjectOfType<HUDManager>();
			if (CanInteract) hud?.OpenMessagePanel(hudMessage);
		}

		public virtual void OnPlayerExitedTrigger(GameObject interactor)
		{
			if(!hud) hud = FindObjectOfType<HUDManager>();
			if (CanInteract) hud?.CloseMessagePanel();
		}

		public abstract void Interact(GameObject interaction);

		protected void TriggerInteractedEvent() => OnInteracted?.Invoke();
	}
}