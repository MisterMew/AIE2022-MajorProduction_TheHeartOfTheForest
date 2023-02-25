/*
 * 
 * Date Created: 22.08.2022
 * Author: Lewis Comstive
 *
 */

/*
 * 
 * Changelog:
 *	Lewis - Initial creation
 *  Nghia - Added OnOverheating event, setup statistics gathering
 */

using System;
using UnityEngine;
using HotF.Abilities;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF.Player
{
	[RequireComponent(typeof(PlayerInput))]
	public class PlayerAbilityHandler : MonoBehaviour
	{
		[Serializable]
		public struct AbilityHolder
		{
			public Ability Ability;
			public UnityEvent OnStarted;
			public UnityEvent OnStopped;

			[Tooltip("Tried to use ability, it went haha no")]
			public UnityEvent OnFailedToUse;
		}

		[Header("Abilities")]
		[SerializeField]
		private List<AbilityHolder> abilities = new List<AbilityHolder>();

		[Header("Input")]
		[SerializeField, Tooltip("Toggles gliding ability")]
		private InputActionReference glideToggleInput;
		[SerializeField, Tooltip("Toggles burrowing ability")]
		private InputActionReference burrowToggleInput;
		[SerializeField, Tooltip("Toggles glow ability")]
		private InputActionReference glowToggleInput;
		[SerializeField, Tooltip("Leaves burrow if pressed")]
		private InputActionReference jumpInput;

		private const int BurrowAbilityIndex = 0;
		private const int GlideAbilityIndex = 1;
		private const int GlowAbilityIndex = 2;

		public BurrowAbility BurrowAbility => abilities.Count > 0 ? (BurrowAbility)abilities[BurrowAbilityIndex].Ability : null;
		public GlideAbility GlideAbility => abilities.Count > 1 ? (GlideAbility)abilities[GlideAbilityIndex].Ability : null;
		public GlowAbility GlowAbility => abilities.Count > 2 ? (GlowAbility)abilities[GlowAbilityIndex].Ability : null;

		/// <summary>
		/// Local <see cref="PlayerInput"/> component
		/// </summary>
		private PlayerInput playerInput;

		// Cached input actions from local PlayerInput component
		private InputAction glideAction, burrowAction, glowAction, jumpAction;

		[Header("Glow ability Event")]
		[Tooltip("Event when overheating")]
		public UnityEvent OnOverheating; // added by Nghia

		/// <summary>
		/// Statistics Manager (added by Nghia)
		/// </summary>
		private PlayerStatisticsManager playerStats;

		private void Start()
		{
			// Cache local PlayerInput component
			playerInput = GetComponent<PlayerInput>();

			// Setup glow events
			((GlowAbility)abilities[2].Ability).OnOverheating = OnOverheating; // added by Nghia

			// Get input actions from input component
			if (playerInput)
			{
				// Get input actions from local PlayerInput component
				if (glowToggleInput)
				{
					glowAction = playerInput.actions.FindAction(glowToggleInput.action.id);
					glowAction.started += OnGlowPerformed;
				}
				if (glideToggleInput)
				{
					glideAction = playerInput.actions.FindAction(glideToggleInput.action.id);
					glideAction.started += OnGlidePerformed;
					glideAction.canceled += OnGlideCancelled;
				}
				if (burrowToggleInput)
				{
					burrowAction = playerInput.actions.FindAction(burrowToggleInput.action.id);
					burrowAction.started += OnBurrowToggled;
				}
				if (jumpInput)
				{
					jumpAction = playerInput.actions.FindAction(jumpInput.action.id);
					jumpAction.started += OnJumpPerformed;
				}
			}

			// Set the caster of each ability to the attached GameObject &
			//	listen to state changes for each ability
			foreach (AbilityHolder ability in abilities)
			{
				if (!ability.Ability)
					return;
				ability.Ability.Caster = gameObject;
				ability.Ability.Movement = GetComponent<PlayerMovement>();
				ability.Ability.OnReset();

				ability.Ability.ActiveStateChange += (active) => OnAbilityActiveStateChanged(ability, active);
			}

			SetupStatistics();
		}

		public void ResetState()
		{
			foreach (AbilityHolder ability in abilities)
			{
				if (ability.Ability)
					ability.Ability.OnReset();
			}
		}

		/// <summary>
		/// Setup player Statistics (added by Nghia)
		/// </summary>
		private void SetupStatistics()
		{
			// Find Statistics Manager
			playerStats = GetComponent<PlayerStatisticsManager>();

			if (!playerStats) return;

			abilities[0].OnStarted.AddListener(delegate { playerStats.IncrementBurrowCount(); });
			abilities[0].OnFailedToUse.AddListener(delegate { playerStats.IncrementBurrowFailedCount(); });
			abilities[1].OnStarted.AddListener(delegate { playerStats.IncrementGlideCount(); });
			abilities[1].OnFailedToUse.AddListener(delegate { playerStats.IncrementGlideFailedCount(); });
			abilities[2].OnStarted.AddListener(delegate { playerStats.IncrementGlowCount(); });
			abilities[2].OnFailedToUse.AddListener(delegate { playerStats.IncrementGlowFailedCount(); });
		}

		private void OnDestroy()
		{
			if (!playerInput)
				return;

			foreach (AbilityHolder ability in abilities)
			{
				if (!ability.Ability)
					continue;
				ability.Ability.OnReset();

				// Stop listening to events
				ability.Ability.ActiveStateChange -= (active) => OnAbilityActiveStateChanged(ability, active);
			}

			// Stop listening to when buttons are pressed
			glowAction.started -= OnGlowPerformed;
			jumpAction.started -= OnJumpPerformed;
			burrowAction.started -= OnBurrowToggled;
			glideAction.started -= OnGlidePerformed;
			glideAction.canceled -= OnGlideCancelled;
		}

		#region Input event handling
		private void OnGlowPerformed(InputAction.CallbackContext _)
		{
			if (!enabled || !GlowAbility)
				return;

			if (GlowAbility.OnCooldown)
				abilities[GlowAbilityIndex].OnFailedToUse?.Invoke();
			else
				GlowAbility.Activate();
		}

		private void OnGlidePerformed(InputAction.CallbackContext _)
		{
			if (!GlideAbility || !enabled)
				return;
			GlideAbility.AbilityKeyHeld = true;
			GlideAbility.Activate(true);
		}

		private void OnGlideCancelled(InputAction.CallbackContext _)
		{
			if (!GlideAbility || !enabled)
				return;
			GlideAbility.AbilityKeyHeld = false;
			GlideAbility.Activate(false);
		}

		private void OnBurrowToggled(InputAction.CallbackContext _)
		{
			if (!enabled || !BurrowAbility)
				return;

			if (!BurrowAbility.Activate())
				abilities[BurrowAbilityIndex].OnFailedToUse?.Invoke();
		}


		private void OnJumpPerformed(InputAction.CallbackContext obj)
		{
			if (enabled && BurrowAbility)
				BurrowAbility.Activate(false);
		}
		#endregion

		private void OnAbilityActiveStateChanged(AbilityHolder ability, bool active)
		{
			if (active)
				ability.OnStarted?.Invoke();
			else
				ability.OnStopped?.Invoke();
		}

		#region Ability function passthrough
		private void Update()
		{
			foreach (AbilityHolder ability in abilities)
			{
				ability.Ability?.ReduceCooldown(Time.deltaTime);
				ability.Ability?.OnUpdate();
			}
		}

		public void ChangeGroundedState(bool grounded)
		{
			foreach (AbilityHolder ability in abilities)
				ability.Ability?.OnGroundStateChanged(grounded);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			foreach (AbilityHolder ability in abilities)
				ability.Ability?.EnteredTrigger(collision);
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			foreach (AbilityHolder ability in abilities)
				ability.Ability?.ExitedTrigger(collision);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			foreach (AbilityHolder ability in abilities)
				ability.Ability?.EnteredCollision(collision);
		}

		private void OnCollisionExit2D(Collision2D collision)
		{
			foreach (AbilityHolder ability in abilities)
				ability.Ability?.ExitedCollision(collision);
		}
		#endregion

		private void OnDrawGizmos()
		{
			if (!BurrowAbility)
				return;

			float angle = BurrowAbility.MaxBurrowAngle * Mathf.Deg2Rad;
			Color handleColor = new Color(0, 0, 1, 1);
			Vector3 origin = transform.position + Vector3.down * BurrowAbility.RaycastLengthBurrow;
			Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);

#if UNITY_EDITOR
			Handles.color = handleColor;
			Handles.DrawWireArc(
				/* Position  */ origin,
				/* Normal	 */ Vector3.forward,
				/* Direction */ direction,
				/* Angle	 */ BurrowAbility.MaxBurrowAngle * 2.0f,
				/* Radius	 */ 0.2f
			);

			handleColor.a = 0.1f;
			Handles.color = handleColor;
			Handles.DrawSolidArc(
				/* Position  */ origin,
				/* Normal	 */ Vector3.forward,
				/* Direction */ direction,
				/* Angle	 */ BurrowAbility.MaxBurrowAngle * 2.0f,
				/* Radius	 */ 0.2f
			);
#endif
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(PlayerAbilityHandler))]
		private class PlayerAbilityHandlerEditor : Editor
		{
			public override void OnInspectorGUI()
			{
				EditorGUILayout.HelpBox("Expected ability order:\n - Burrow\n - Glide\n - Glow", MessageType.Warning);

				base.OnInspectorGUI();
			}
		}
#endif
	}
}