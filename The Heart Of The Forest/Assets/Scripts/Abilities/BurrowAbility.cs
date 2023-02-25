/*
 * 
 * Date Created: 22.08.2022
 * Author: Lewis Comstive
 *
 */

using UnityEngine;
using HotF.Player;
using System.Threading.Tasks;

namespace HotF.Abilities
{
	/// <summary>
	/// Enables the caster to dig in to the ground below them
	/// </summary>
	[CreateAssetMenu(fileName = "Burrow", menuName = "HotF/Abilities/Burrow")]
	public class BurrowAbility : ToggleableAbility
	{
		[Tooltip("Curve for lerping to & from, when matching rotation of ground beneath caster")]
		public AnimationCurve RotateTimeCurve;

		[Range(0, 180), Tooltip("Maximum angle of a platform the player can use for burrowing")]
		public int MaxBurrowAngle = 20;

		[Tooltip("Multiplier for how much to burrow down, relative to the entity's height")]
		public float BurrowHeight = 1.0f;

		[Tooltip("Length of raycast to check for ground when burrowing")]
		public float RaycastLengthBurrow = 1.0f;

		[Tooltip("Length of raycast to check for ground when unburrowing")]
		public float RaycastLengthUnburrow = 1.0f;

		[Tooltip("Velocity to apply away from the ground when unburrowing")]
		public float UnBurrowForce = 10.0f;

		[Tooltip("Velocity to apply away from the ground when unburrowing from an autoburrow")]
		public float UnAutoBurrowForce = 10.0f;

		[Tooltip("Time to wait before moving transform in to burrow position, in seconds. Used so animation can play")]
		public float BurrowDelay = 0.5f;

		[Tooltip("Layers that the caster can burrow in to. Used for raycasting and checking for valid ground")]
		public LayerMask BurrowableLayers;

		[Tooltip("Layer for forcing burrow ability to activate when contact is made")]
		public string AutoBurrowTag = "AutoBurrow";

		protected override bool CanToggle => IsActive ? CanUnburrow() : CanBurrow(FindGround());

		/// <summary>
		/// Cached collider of the caster
		/// </summary>
		private Collider2D casterCollider;

		/// <summary>
		/// Cached rigidbody of the caster
		/// </summary>
		private Rigidbody2D casterRB;

		/// <summary>
		/// Height of <see cref="casterCollider"/>
		/// </summary>
		private Vector2 colliderSize = Vector2.zero;

		/// <summary>
		/// Store the last autoburrow collider, to prevent autoburrowing as
		/// the caster leaves. This means they can also stand on top after burrowing.
		/// </summary>
		private Collider2D lastAutoBurrowCollider = null;

		/// <summary>
		/// When true, entity is currently burrowing or unburrowing, and has not finished yet
		/// </summary>
		private bool isChangingState = false;

		/// <summary>
		/// Filter used when raycasting and checking for valid, burrow-able ground
		/// </summary>
		private ContactFilter2D raycastFilter = new ContactFilter2D()
		{
			useLayerMask = true,
			useTriggers = false
		};

		public override void OnReset()
		{
			base.OnReset();
			IsActive = false;

			// Get caster's collider & make sure it's valid
			if (!Caster.TryGetComponent(out casterCollider))
			{
				Debug.LogWarning($"No Collider2D was present on '{Caster.name}', and is required for using the burrow ability");
				return;
			}

			// Calculate collider size
			if (casterCollider is CapsuleCollider2D)
				colliderSize = ((CapsuleCollider2D)casterCollider).size;
			else
				colliderSize = (Vector2)casterCollider.bounds.size;
			colliderSize.x /= 2.0f;

			// Get attached rigidbody
			casterRB = Caster.GetComponent<Rigidbody2D>();

			// Update filter layermask
			raycastFilter.SetLayerMask(BurrowableLayers);
		}

		protected override bool OnActiveStateChanged()
		{
			// Don't begin burrow if not grounded
			if (!IsGrounded && !IsActive)
			{
				Debug.Log("Cannot burrow, not grounded");
				return false;
			}

			if (!IsActive)
			{
				// Find valid ground & burrow into it
				RaycastHit2D groundHit = FindGround();
				if (!CanBurrow(groundHit))
					return false;

				Burrow(groundHit);
			}
			else
			{
				// Already burrow, return to surface
				if (!CanUnburrow())
					return false;

				Unburrow();
			}
			return true;
		}

		public override void EnteredCollision(Collision2D collision)
		{
			base.EnteredCollision(collision);

			// Check if should force (auto)burrow
			if (collision.gameObject.CompareTag(AutoBurrowTag))
			{
				// Raycast towards collision surface
				RaycastHit2D hit = FindGround((collision.transform.position - Caster.transform.position).normalized);

				// Do autoburrow
				Burrow(hit, true);
			}
		}

		public override void OnGroundStateChanged(bool grounded)
		{
			base.OnGroundStateChanged(grounded);

			if (!grounded)
				lastAutoBurrowCollider = null; // Clear last used autoburrow collider
		}

		/// <summary>
		/// Lowers <see cref="Ability.Caster"/> into <paramref name="groundHit"/>'s <see cref="RaycastHit2D.collider"/>.
		/// <see cref="Ability.Caster"/>'s collider, movement & rigidbody are disabled, and it's transform parent is set to the ground collider.
		/// </summary>
		private async void Burrow(RaycastHit2D groundHit, bool autoBurrow = false)
		{
			if (!groundHit.collider || autoBurrow && lastAutoBurrowCollider == groundHit.collider)
				return;
			isChangingState = true;

			// Disable player movement script
			if (Caster.TryGetComponent(out PlayerMovement playerMovement))
				playerMovement.enabled = false;

			if (autoBurrow)
			{
				// Force burrow, bypass animation
				IsActive = true;
				lastAutoBurrowCollider = groundHit.collider;

				Caster.transform.position = groundHit.point + groundHit.normal * colliderSize.y * BurrowHeight;
			}
			else if(playerMovement && playerMovement.Animator)
			{
				// Play animation
				playerMovement.Animator.SetTrigger("Burrow");
				playerMovement.Animator.ResetTrigger("UnBurrow");

				// Wait for animation to play
				await Task.Delay((int)(BurrowDelay * 1000));
			}

			// Disable rigidbody simulation and attached collider
			casterRB.simulated = false;
			casterCollider.enabled = false;

			// Set parent as ground burrowing in to
			Transform groundTransform = groundHit.collider.transform;
			Caster.transform.parent = groundTransform;

			// Rotation to match ground
			Quaternion groundRotation = Quaternion.LookRotation(Vector3.forward, groundHit.normal);

			// Lerp towards ground rotation
			float totalTime = 0;
			float desiredTime = RotateTimeCurve.keys[RotateTimeCurve.keys.Length - 1].time;
			while (totalTime < desiredTime && !autoBurrow)
			{
				Caster.transform.rotation = Quaternion.Slerp(Caster.transform.rotation, groundRotation, RotateTimeCurve.Evaluate(totalTime));

				float deltaTime = Time.deltaTime;
				await Task.Delay((int)(deltaTime * 1000));
				totalTime += deltaTime;
			}
			Caster.transform.rotation = groundRotation;
			isChangingState = false;
		}

		private async void Unburrow()
		{
			isChangingState = true;

			if(Caster.TryGetComponent(out PlayerMovement playerMovement))
				playerMovement.Animator?.SetTrigger("UnBurrow");

			Vector3 burrowedUp = Caster.transform.up;

			// Lerp from ground rotation to no rotation
			float totalTime = 0;
			float desiredTime = RotateTimeCurve.keys[RotateTimeCurve.keys.Length - 1].time;
			while (totalTime < desiredTime)
			{
				Caster.transform.rotation = Quaternion.Slerp(Caster.transform.rotation, Quaternion.identity, RotateTimeCurve.Evaluate(totalTime));

				float deltaTime = Time.deltaTime;
				await Task.Delay((int)(deltaTime * 1000));
				totalTime += deltaTime;
			}
			Caster.transform.rotation = Quaternion.identity;

			// Restore original parent & scale
			if (!playerMovement)
				Caster.transform.parent = null;
			else
				Caster.transform.parent = playerMovement.rootObject;

			Caster.transform.localScale = Vector3.one;

			// Restore rigidbody simulation and attached collider
			casterCollider.enabled = true;
			casterRB.simulated = true;

			if (lastAutoBurrowCollider == null) // Normal unburrow
				casterRB.AddForce(Caster.transform.up * UnBurrowForce, ForceMode2D.Impulse);
			else // Unburrow from autoburrow
			{
				casterRB.AddForce(burrowedUp * UnAutoBurrowForce, ForceMode2D.Impulse);
				Caster.transform.position += burrowedUp * colliderSize.y * BurrowHeight;
			}

			// Enable player movement script
			if (playerMovement)
				playerMovement.enabled = true;

			isChangingState = false;
		}

		/// <summary>
		/// Sends 3 rays from player to check for valid ground. One ray from the center of player,
		/// the other two at the horizontal extents of their collider.
		/// </summary>
		/// <returns>Hit ground from center raycast</returns>
		private RaycastHit2D FindGround(Vector3 direction)
		{
			Vector3 origin = Caster.transform.position;
			RaycastHit2D[] hits = new RaycastHit2D[3];

			Debug.DrawLine(origin + Vector3.left, origin + Vector3.right, Color.yellow, 0.5f);

			// Check left and right of player, to ensure all of collider is on ground.
			// Trying to prevent player from half sticking out of burrowed ground.
			for (int i = -1; i <= 1; i++)
			{
				Vector2 rayOrigin = origin + Vector3.left * i * colliderSize.x;
				Debug.DrawRay(rayOrigin, direction * RaycastLengthBurrow, Color.yellow, 0.5f);

				RaycastHit2D[] results = new RaycastHit2D[1];
				int resultCount = Physics2D.Raycast(rayOrigin, direction, raycastFilter, results, RaycastLengthBurrow);
				if (resultCount == 0)
					return new RaycastHit2D();
				hits[i + 1] = results[0];
			}

			return hits[1]; // Middle raycast
		}

		/// <summary>
		/// Sends 3 rays from player to check for valid ground. One ray from the center of player,
		/// the other two at the horizontal extents of their collider.
		/// </summary>
		/// <returns>Hit ground from center raycast</returns>
		private RaycastHit2D FindGround() => FindGround(-Caster.transform.up);


		/// <summary>
		/// Checks below the player for valid burrowable ground.
		/// </summary>
		private bool CanBurrow(RaycastHit2D groundHit) =>
			!isChangingState &&
			// Check if ground was found,
			//  and angle is valid
			groundHit.collider != null &&
					Vector3.Angle(Vector3.up, groundHit.normal) <= MaxBurrowAngle;

		/// <summary>
		/// Checks above player if any objects are preventing them from unburrowing
		/// </summary>
		/// <returns>True if player can unburrow without any obstructions</returns>
		private bool CanUnburrow()
		{
			// Check if currently burrowing
			if(isChangingState) return false;

			Vector3 origin = Caster.transform.position + Caster.transform.up * colliderSize.y * BurrowHeight;

			Debug.DrawLine(origin + Vector3.left, origin + Vector3.right, Color.yellow, 0.5f);

			for (int i = -1; i <= 1; i++)
			{
				Vector3 rayOrigin = origin + Vector3.left * i * colliderSize.x;

				RaycastHit2D[] results = new RaycastHit2D[1];
				int resultCount = Physics2D.Raycast(rayOrigin, Caster.transform.up, raycastFilter, results, RaycastLengthUnburrow);
				Debug.DrawRay(rayOrigin, Caster.transform.up * RaycastLengthUnburrow, Color.yellow, 0.5f);
				if (resultCount > 0)
					return false;
			}
			return true;
		}
	}
}