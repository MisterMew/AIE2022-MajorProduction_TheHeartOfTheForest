/*
 * Date Created: 22.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

/*
 * 
 * Changelog
 *  22.08.2022 - Jazmin
 *   - Initial implementation
 *  13.08.2022 - Lewis
 *   - Link with heart fragment storage
 *   - Destroy self when picked up
 *  13.09.2022 - Nghia
 *   - Added HeartFragmentState
 *  20.09.2022 - Lewis
 *   - Save HeartFragmentState
 *  25.09.2022 - Nghia
 *   - Added audio functionality
 *  25.10.2022
 *   - Moved TriggerInteractedEvent() From UpdateState to interact()
 */

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF.Interactable
{
	/// <summary>
	/// State of the Heart Fragment
	/// </summary>
	public enum HeartFragmentState
	{
		UNCOLLECTED = 0,
		COLLECTED = 1,
		RETURNED = 2
	}

	/// <summary>
	/// 
	/// </summary>
	public class HeartFragInteractable : Interactable
	{		
		private GameDataManager gameDataManager;

		[SerializeField, Tooltip("How high the fragment can float from it's initial position")]
		private float floatHeight = 0.2f;

		[SerializeField, Tooltip("How fast the fragment moves between floating positions")]
		private float floatSpeed = 1.0f;

		[SerializeField]
		private string msgAlreadyHoldingFragment = "You can only muster the strength to hold one fragment";

		[SerializeField, Tooltip("Object to spawn when this heart is collected, spawned in place of it")]
		private GameObject collectedSpawnFX = null;

		[SerializeField, Tooltip("Time to destroy collectedSpawnFX after it spawns")]
		private float collectedSpawnFXLifetime = 5.0f;

		/// <summary>
		/// State the Heart Fragment is in.
		/// Has this heart fragment been collected and returned
		/// </summary>
		[field: SerializeField]
		public HeartFragmentState State
		{
			get;
			set;
		} = HeartFragmentState.UNCOLLECTED;

		/// <summary>
		/// Point to float around
		/// </summary>
		private Vector3 floatCenter = Vector3.zero;

		private Transform uncollectedParent = null;

		/// <summary>
		/// Audio Source
		/// </summary>
		private AudioSource audioSource = null;

		private static bool playerHoldingFragment = false;

		public override bool CanInteract => State == HeartFragmentState.UNCOLLECTED;

		protected override void Start()
		{
			base.Start();
			playerHoldingFragment = false;
			floatCenter = transform.position;
			audioSource = FindObjectOfType<AudioManager>().hfcSfx;
		}

		private GameDataManager GetGameDataManager()
		{
			if (gameDataManager)
				return gameDataManager;
			return (gameDataManager = FindObjectOfType<GameDataManager>());
		}

		private void Update()
		{
			if (State == HeartFragmentState.UNCOLLECTED)
				transform.position = floatCenter + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;
		}

		public override void OnPlayerEnteredTrigger(GameObject interactor)
		{
			if (!playerHoldingFragment)
				hud?.OpenMessagePanel(hudMessage);
			else
				hud?.OpenMessagePanel(msgAlreadyHoldingFragment);
		}

		public override void OnPlayerExitedTrigger(GameObject interactor) => hud?.CloseMessagePanel();

		public override void Interact(GameObject interaction)
		{
			if (!CanInteract || playerHoldingFragment)
				return;

			// Update state
			UpdateState(HeartFragmentState.COLLECTED);

			uncollectedParent = transform.parent;
			transform.parent = interaction.transform;
			transform.localPosition = Vector3.zero;

			audioSource.Play(); // added by Nghia

			// Hide HUD and message
			hud?.CloseMessagePanel();

			// Spawn FX, and destroy it after period of time
			if(collectedSpawnFX)
				Destroy(Instantiate(collectedSpawnFX, transform.position, Quaternion.identity), collectedSpawnFXLifetime);

			// Removed to fix bug
			//GetGameDataManager()?.SaveHeartFragments();

			TriggerInteractedEvent();
		}

		public void UpdateState(HeartFragmentState state)
		{
			if (State == state)
				return; // No change
			State = state;

			// Show or hide this gameobject depending on state
			gameObject.SetActive(State == HeartFragmentState.UNCOLLECTED);

			// Set interaction state based on collection state
			CanInteract = State == HeartFragmentState.UNCOLLECTED;

			if (State == HeartFragmentState.COLLECTED)
				playerHoldingFragment = true;
			else
				playerHoldingFragment = false;

			// Set float center if not collected
			if (State == HeartFragmentState.UNCOLLECTED)
			{
				transform.parent = uncollectedParent;
				floatCenter = transform.position;
			}

			// Set parent to root of scene once returned
			if (State == HeartFragmentState.RETURNED)
				transform.parent = null;

			// TO DO NGHIA- do this in asset switcher
			//if(State != HeartFragmentState.UNCOLLECTED)
				//TriggerInteractedEvent();
		}

		/// <summary>
		/// Publicly call TriggerInteractedEvent from HeartFragmentInteractable
		/// </summary>
		public void StartInteractedEvent() => TriggerInteractedEvent();
	}
}