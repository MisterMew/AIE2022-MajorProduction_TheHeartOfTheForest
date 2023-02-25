/*
 * 
 * Date Created: 22.08.2022
 * Author: Lewis Comstive
 *
 */

using UnityEngine;
using HotF.Player;
using System.Collections.Generic;

namespace HotF.Abilities
{
	/// <summary>
	/// Base class for entity ability.
	/// Can be activated to produce effects, change values and the like.
	/// </summary>
	[System.Serializable]
	public abstract class Ability : ScriptableObject
	{
		[SerializeField, Tooltip("Minimum time between activations, in seconds")]
		private float cooldown = 1.0f;

		/// <summary>
		/// Amount of time remaining before ability can be activated, in seconds
		/// </summary>
		private float cooldownRemaining = 0;

		/// <summary>
		/// Stores the grounded state of the caster. Set in <see cref="OnGroundStateChanged(GameObject, bool)"/>
		/// </summary>
		protected bool IsGrounded => Movement.IsGrounded;

		/// <summary>
		/// Reflects current state
		/// </summary>
		private bool isActive = false;

		/// <summary>
		/// Reflects current state
		/// </summary>
		public bool IsActive
		{
			get => isActive;
			protected set
			{
				if (isActive == value)
					return; // No update needed

				isActive = value;
				
				// Trigger event
				ActiveStateChange?.Invoke(isActive);
			}
		}

		/// <summary>
		/// Entity that cast the ability
		/// </summary>
		public GameObject Caster { get; set; } = null;

		/// <summary>
		/// Entity movement script, can be used to disable movement and check grounded state
		/// </summary>
		public PlayerMovement Movement { get; set; } = null;

		/// <summary>
		/// Triggers that the entity is currently inside of.
		/// 
		/// Modified when Unity's trigger related message functions are called.
		/// </summary>
		protected List<Collider2D> SurroundingTriggers { get; private set; } = new List<Collider2D>();

		public delegate void OnActiveStateChange(bool active);
		/// <summary>
		/// Called whenever the active state (<see cref="IsActive"/>) of this ability changes
		/// </summary>
		public event OnActiveStateChange ActiveStateChange;

		/// <summary>
		/// Reduces time until ability can be activated again, in seconds
		/// </summary>
		public void ReduceCooldown(float time) => cooldownRemaining = Mathf.Clamp(cooldownRemaining - time, 0, cooldown);

		/// <summary>
		/// Toggles activation state of the ability
		/// </summary>
		/// <returns>True if activated successfully. Otherwise false</returns>
		public bool Activate() => Activate(!IsActive); // Toggle between active & inactive

		/// <summary>
		/// Activates the ability
		/// </summary>
		/// <returns>True if activated successfully. Otherwise false</returns>
		public bool Activate(bool activate)
		{
			if (activate == IsActive || cooldownRemaining > 0)
				return false; // Failed to swap state

			bool result = OnActivated(activate);
			if (result)
			{
				// Update state
				IsActive = activate;
				cooldownRemaining = cooldown;
			}

			return result;
		}

		/// <summary>
		/// Called when resetting the ability state.
		/// This can be used to clear any cached variables, for instance.
		/// </summary>
		public virtual void OnReset()
		{
			isActive = false;
			SurroundingTriggers.Clear();
		}

		/// <summary>
		/// When <see cref="Movement"/>'s grounded state changes, this function is called
		/// </summary>
		public virtual void OnGroundStateChanged(bool grounded) { }

		/// <summary>
		/// Called when caster has entered a trigger
		/// </summary>
		public virtual void EnteredTrigger(Collider2D collider) => SurroundingTriggers.Add(collider);

		/// <summary>
		/// Called when caster has exited a trigger
		/// </summary>
		public virtual void ExitedTrigger(Collider2D collider) => SurroundingTriggers.Remove(collider);

		/// <summary>
		/// Called when caster has begun colliding with another physics object
		/// </summary>
		public virtual void EnteredCollision(Collision2D collision) { }

		/// <summary>
		/// Called when caster has stopped colliding with another physics object
		/// </summary>
		public virtual void ExitedCollision(Collision2D collision) { }
		
		/// <summary>
		/// Called once per frame
		/// </summary>
		public virtual void OnUpdate() { }

		/// <summary>
		/// Function to be overriden in inherited classes. Called when ability activation state changes.
		/// </summary>
		protected abstract bool OnActivated(bool activate);
	}
}