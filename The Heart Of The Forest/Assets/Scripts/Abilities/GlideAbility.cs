/*
 * 
 * Date Created: 22.08.2022
 * Author: Lewis Comstive
 *
 */

using UnityEngine;
using HotF.Environment;

namespace HotF.Abilities
{
	/// <summary>
	/// Reduces gravity applied to <see cref="Ability.Caster"/> while active
	/// </summary>
	[CreateAssetMenu(fileName = "Glide", menuName = "HotF/Abilities/Glide")]
	public class GlideAbility : Ability
	{
		[Tooltip("Gravity applied while ability is active")]
		public float GravityMultiplier = 0.75f;

		/// <summary>
		/// Current active area that allows gliding
		/// </summary>
		private GlideArea glideArea = null;

		/// <summary>
		/// Cached rigidbody of <see cref="Ability.Caster"/>
		/// </summary>
		private Rigidbody2D casterRigidbody = null;

		/// <summary>
		/// Trigger on <see cref="glideArea"/>
		/// </summary>
		private Collider2D glideAreaTrigger = null;

		/// <summary>
		/// State of related input, used to keep this ability active between glide areas
		/// </summary>
		[HideInInspector]
		public bool AbilityKeyHeld = false;

		public override void OnReset()
		{
			base.OnReset();
			glideArea = null;
			casterRigidbody = null;
			glideAreaTrigger = null;
		}

		protected override bool OnActivated(bool activate)
		{
			// Don't activate if not in a glide area
			if (!IsActive && !glideAreaTrigger)
			{
				if(casterRigidbody)
					casterRigidbody.gravityScale = IsActive ? GravityMultiplier : 1.0f;
				return false;
			}

			SetGliding();
			return true;
		}

		/// <summary>
		/// Sets gliding properties based on <see cref="ToggleableAbility.IsActive"/>'s values
		/// </summary>
		private void SetGliding()
		{
			if (!casterRigidbody)
				casterRigidbody = Caster.GetComponent<Rigidbody2D>();

			casterRigidbody.gravityScale = IsActive ? 1.0f : GravityMultiplier;

			// Disable or restore player jumping
			if (Caster.TryGetComponent(out Player.PlayerMovement movement))
			{
				movement.CanJump = IsActive;
				movement.ClampVelocityAmount = !IsActive ? 0.0f : 1.0f;
			}
		}

		public override void EnteredTrigger(Collider2D collider)
		{
			base.EnteredTrigger(collider);

			// Check if glide area is already active, or this trigger is not a glide area
			if (glideAreaTrigger ||
				!collider.TryGetComponent(out glideArea))
				return;

			// Cache the trigger
			glideAreaTrigger = collider;

			// Remove player's ability to jump, in favour of enabling gliding
			if (Caster.TryGetComponent(out Player.PlayerMovement movement))
			{
				movement.CanJump = false;
				movement.ClampVelocityAmount = 0.0f;
			}

			if (AbilityKeyHeld && !IsActive)
				Activate(true);
		}

		public override void ExitedTrigger(Collider2D collider)
		{
			base.ExitedTrigger(collider);

			// Check if this trigger is the glide area trigger
			if (glideAreaTrigger != collider)
				return;

			// Check if another glide area trigger is active.
			// Could be transitioning between two glide areas
			for(int i = 0; i < SurroundingTriggers.Count; i++)
			{
				if (SurroundingTriggers[i] &&
					SurroundingTriggers[i].TryGetComponent(out glideArea))
				{
					glideAreaTrigger = SurroundingTriggers[i];
					return;
				}
			}

			glideArea = null;
			glideAreaTrigger = null;

			// Re-enable player's ability to jump
			if (Caster.TryGetComponent(out Player.PlayerMovement movement))
			{
				movement.CanJump = true;
				movement.ClampVelocityAmount = 1.0f;
			}
		}

		public override void OnGroundStateChanged(bool grounded)
		{
			if (!grounded || !IsActive)
				return;

			IsActive = false;

			if (glideAreaTrigger)
			{
				casterRigidbody.gravityScale = IsActive ? GravityMultiplier : 1.0f;
			}
			else
			{
				// Stop gliding
				glideArea = null;
				glideAreaTrigger = null;
				SetGliding();
			}
		}

		public override void OnUpdate()
		{
			if (!IsActive || !glideAreaTrigger)
				return;

			Vector3 desiredVelocity = glideArea.transform.up * glideArea.maxVelocity;
			casterRigidbody.velocity = Vector3.Lerp(casterRigidbody.velocity, desiredVelocity, glideArea.maxVelocity * Time.deltaTime);
		}
	}
}