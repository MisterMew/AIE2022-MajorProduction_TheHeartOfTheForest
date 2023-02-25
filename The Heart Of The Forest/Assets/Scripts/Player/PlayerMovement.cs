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
 *	Nghia - Added IsWalkingOnGround(), ToggleFootstepsAudio() to play footsteps audio
 *
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using HotF.Environment.Platform;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor; // For handles
#endif

namespace HotF.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(CapsuleCollider2D))]
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Movement")]
		[SerializeField, Tooltip("Force to apply to movement")]
		private float moveSpeed = 12.5f;

		[SerializeField, Tooltip("Multiplier for calculated force when airborne"), Range(0, 1)]
		private float airSpeedMultiplier = 0.75f;

		[SerializeField, Tooltip("How fast to begin movement")]
		private float acceleration = 13;

		[SerializeField, Tooltip("How fast to begin movement")]
		private float deceleration = 16;

		[SerializeField, Tooltip("How much forces to apply against the player when releasing movement input")]
		private float friction = 0.22f;

		[SerializeField, Tooltip("How much forces to apply against the player when releasing movement input in the air")]
		private float airFriction = 0.22f;

		[SerializeField, Tooltip("Maximum velocity of player's rigidbody, on x & y axes")]
		private Vector2 maxVelocity = new Vector2(10, 10);

		[Header("Jumping")]
		[SerializeField, Tooltip("Force to apply when jumping")]
		private float jumpForce = 10.0f;

		[SerializeField, Tooltip("Maximum amount of jumps the player has")]
		private int maxJumps = 2;

		[SerializeField, Tooltip("Extra force to apply when falling, to prevent feeling 'floaty'")]
		private float fallMultiplier = 2.5f;

		[SerializeField, Tooltip("Extra force to apply when mid-jump but not holding the jump button, to lower the overall jump height")]
		private float lowJumpMultiplier = 2f;

		[SerializeField, Tooltip("Maximum amount of time when airborne to still allow the first jump, in seconds")]
		private float coyoteTimeframe = 0.1f;

		[SerializeField, Tooltip("Maximum amount of time to apply jump force while holding jump button. In seconds")]
		private float maxJumpTime = 1.0f;

		[SerializeField, Tooltip("How much force the first jump has")]
		private float firstJumpMultiplier = 1.0f;

		[SerializeField, Tooltip("Minimum time, in seconds, between jump inputs are registered")]
		private float minTimeBetweenJumpInputs = 0.1f;

		[Header("Obstacle Checks")]

		[SerializeField, Tooltip("Radius to check for obstacles at player's feet and head")]
		private float obstacleCheckRadius = 0.25f;

		[SerializeField]
		private LayerMask groundLayers;

		[Header("Input")]
		[SerializeField] private InputActionReference moveInputAction;
		[SerializeField] private InputActionReference jumpInputAction;

		[Header("Animations")]
		[SerializeField]
		private Animator animator;

		[SerializeField, Tooltip("How fast to flip between facing left & right")]
		private float flipSpeed = 2.0f;

		[SerializeField, Tooltip("Time between triggering jump animation and applying upwards force")]
		private float jumpForceDelay = 0.2f;

		[Header("Audio")]
		[SerializeField, Tooltip("Looping footsteps audio source")]
		private AudioSource footstepsAudio; // Added by Nghia

		#region Unity Events
		[Serializable]
		public struct InspectorEvents
		{
			public UnityEvent OnJumped;
			public UnityEvent OnAirJumped;
			public UnityEvent OnLanded;
			public UnityEvent<bool> OnGroundStateChanged;
		}
		[Header("Events")]
		[SerializeField] private InspectorEvents events = new InspectorEvents();
		[SerializeField] public static event Action OnCancelBounce;
		#endregion

		[Header("Other")]
		[SerializeField, Tooltip("The object to set parent to when standing on platforms")]
		public Transform rootObject;

		/// <summary>
		/// True if the jump button was pressed this frame
		/// </summary>
		public bool IsJumpPressed { get; private set; }

		/// <summary>
		/// True if the jump button is currently held down
		/// </summary>
		public bool IsJumpHeld => jumpAction.ReadValue<float>() > 0.1f;

		/// <summary>
		/// State of character being on a surface suitable to jump off
		/// </summary>
		public bool IsGrounded { get; private set; }

		/// <summary>
		/// True if the player can jump
		/// </summary>
		[field: SerializeField]
		public bool CanJump { get; set; } = true;

		/// <summary>
		/// State of the player's direction
		/// </summary>
		public bool FacingRight { get; private set; } = true;

		/// <summary>
		/// The collider that caused <see cref="IsGrounded"/> to be true
		/// </summary>
		public Collider2D GroundCollider { get; private set; } = null;

		/// <summary>
		/// The animator attached to this player
		/// </summary>
		public Animator Animator => animator;

		/// <summary>
		/// Rigidbody attached to this player
		/// </summary>
		private Rigidbody2D _rigidbody;

		/// <summary>
		/// Collider attached to the player
		/// </summary>
		private CapsuleCollider2D _collider;

		/// <summary>
		/// Counts times player has jumped.
		/// </summary>
		private int usedJumps = 0;

		/// <summary>
		/// Time away from ground, in seconds
		/// </summary>
		private float timeAwayFromGround = 0;

		/// <summary>
		/// Holds the current horizontal movement input value
		/// </summary>
		private float moveInput;

		/// <summary>
		/// Countdown that starts when the player jumps.
		/// When it reaches zero, the player can jump again.
		/// This is implemented to stop multiple inputs in a very short span of time.
		/// </summary>
		private float lastJumpTimer = 0.0f;

		private PlayerInput playerInput;
		private InputAction jumpAction = null, moveAction;

		/// <summary>
		/// Caches the player's position along the global Z axis during startup.
		/// Used to restrict the player to always be at this value on the Z axis, as this is a 2D game.
		/// </summary>
		private float playerZ = 0.0f;

		/// <summary>
		/// Caches the local Y rotation of the transform attached to <see cref="animator"/> during startup.
		/// This is used for turning that transform towards movement direction in 2D (left or right)
		/// </summary>
		private float originalYRotation = 0.0f;

		/// <summary>
		/// Coroutine for initiating jump
		/// </summary>
		private Coroutine jumpRoutine = null;

		/// <summary>
		/// When larger than 0, clamps rigidbody's velocity to <see cref="maxVelocity"/>.
		/// Should be set to 0 when hitting a bouncepad, or inside glide area.
		/// Multiplier for velocity clamping, expected range from [0.0-1.0],
		///		where 1.0 is not letting velocity go above <see cref="maxVelocity"/>.
		/// </summary>
		[field: SerializeField] // <---- DEBUGGING ---->
		public float ClampVelocityAmount { get; set; } = 1.0f;

		[SerializeField, Tooltip("How quickly the velocity gets clamped after jumping on a bounce mushroom")]
		private float clampVelocityRestoreSpeed = 0.2f;

		private void Start()
		{
			// Cache local rigidbody component
			_rigidbody = GetComponent<Rigidbody2D>();

			// Get local collider and cache initial values
			_collider = GetComponent<CapsuleCollider2D>();

			// Cache player input actions for attached PlayerInput
			playerInput = GetComponent<PlayerInput>();
			moveAction = playerInput.actions.FindAction(moveInputAction.action.id);
			jumpAction = playerInput.actions.FindAction(jumpInputAction.action.id);
			
			// Listen to jump input
			jumpAction.started += OnJumpPressed;

			// Set the initial grounded state
			CheckGrounded();

			playerZ = transform.position.z;
			originalYRotation = animator ? animator.transform.localEulerAngles.y : 0.0f;

			// Set root object to self if unassigned
			if (!rootObject)
				rootObject = transform;
		}

		private void OnEnable()
		{
			if (jumpAction != null)
				// Listen to jump input
				jumpAction.started += OnJumpPressed;
		}

		/// <summary>
		/// Player movement is typically disabled when interacting. So set velocity to zero.
		/// </summary>
		private void OnDisable()
		{
			jumpAction.started -= OnJumpPressed;
			_rigidbody.velocity = Vector3.zero;

			if(animator)
			{
				animator.ResetTrigger("Jump");
				animator.SetBool("Falling", false);
				animator.SetBool("Grounded", true);
				animator.SetFloat("Horizontal Input", 0.0f);
			}
		}

		// Callback when jump input is pressed
		private void OnJumpPressed(InputAction.CallbackContext _) => IsJumpPressed = true;

		private void Update()
		{
			// Update move input from input action
			moveInput = moveAction.ReadValue<float>();

			// Update direction that the player is facing
			if (FacingRight && moveInput <= -0.1f)
				FacingRight = false;
			else if (!FacingRight && moveInput >= 0.1f)
				FacingRight = true;

			// Update animation values
			if (animator)
			{
				animator.SetFloat("Horizontal Input", Mathf.Abs(moveInput));

				// Rotate to face direction of movement
				float desiredAngle = ((FacingRight ? 0 : 180.0f) + originalYRotation) % 360.0f;
				animator.transform.localEulerAngles = Vector3.Lerp(animator.transform.localEulerAngles, Vector3.up * desiredAngle, Time.deltaTime * flipSpeed);
			}

			// Increase time from ground
			if (!IsGrounded)
				timeAwayFromGround += Time.deltaTime;

			// Enforce player's location on Z axis
			Vector3 playerPos = transform.position;
			playerPos.z = playerZ;
			transform.position = playerPos;

			ClampVelocityAmount = Mathf.Clamp(ClampVelocityAmount + Time.deltaTime * clampVelocityRestoreSpeed, 0.0f, 1.0f);
		}

		private void FixedUpdate()
		{
			// Updates IsGrounded
			CheckGrounded();

			// If on ground, apply artificial force against player for deceleration
			if (IsGrounded)
				ApplyDrag(friction);
			else
				ApplyDrag(airFriction);

			Run();

			lastJumpTimer = Mathf.Clamp(lastJumpTimer - Time.deltaTime, 0, minTimeBetweenJumpInputs);

			// Check if can jump
			if (IsJumpPressed &&
				CanJump &&
				usedJumps < maxJumps &&
				lastJumpTimer <= 0 &&
				jumpRoutine == null)
				jumpRoutine = StartCoroutine(StartJump());

			// Only valid for one frame
			IsJumpPressed = false;

			// Apply additional downwards force to reduce 'floaty' feel
			if (_rigidbody.velocity.y < 0) // If falling
				_rigidbody.velocity += Physics2D.gravity * _rigidbody.gravityScale * (fallMultiplier - 1) * Time.fixedDeltaTime;
			else if (_rigidbody.velocity.y > 0 && (!IsJumpHeld || timeAwayFromGround >= maxJumpTime) && !IsGrounded) // Reduce overall jump height as jump key is not pressed
				_rigidbody.velocity += Physics2D.gravity * _rigidbody.gravityScale * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;

			// Clamp velocity on both axes
			if (_rigidbody.velocity.y > maxVelocity.y)
				_rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, new Vector2(_rigidbody.velocity.x, maxVelocity.y), ClampVelocityAmount);
			if (Mathf.Abs(_rigidbody.velocity.x) > maxVelocity.x)
				_rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, new Vector2(maxVelocity.x * Mathf.Sign(_rigidbody.velocity.x), _rigidbody.velocity.y), ClampVelocityAmount);

			if(animator)
				animator.SetBool("Falling", _rigidbody.velocity.y < 0);

			// Coyote time, allows the initial jump from ground to be delayed,
			//	use up that initial jump if away from ground longer than coyoteTimeframe
			if (timeAwayFromGround > coyoteTimeframe && usedJumps == 0)
				usedJumps++;
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.tag.Equals("BouncePad"))
				ClampVelocityAmount = 0.0f;
			else if(IsGrounded)
				OnCancelBounce?.Invoke();
		}

		/// <summary>
		/// Checks if the player is currently on a surface they can jump off of
		/// </summary>
		private void CheckGrounded()
		{
			if (!enabled)
				return;

			bool wasGrounded = IsGrounded;
			bool groundColliderChanged = false;
			IsGrounded = false;

			// Raycast from center of player downwards, half their height
			Vector2 origin = (Vector2)transform.position + Vector2.down * (_collider.size.y / 2.0f) + _collider.offset;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, obstacleCheckRadius, groundLayers);

			// Make sure a collider other than this player was found
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject &&
					!colliders[i].isTrigger)
				{
					IsGrounded = true;
					if (GroundCollider != colliders[i])
						groundColliderChanged = true;

					GroundCollider = colliders[i];
					break;
				}
			}

			// Invoke event for landing on ground
			if (!wasGrounded && IsGrounded && _rigidbody.velocity.y <= 0)
			{
				events.OnLanded?.Invoke();
				events.OnGroundStateChanged?.Invoke(true);

				ClampVelocityAmount = 1.0f;
			}

			// Player left ground
			if (wasGrounded && !IsGrounded)
			{
				GroundCollider = null;
				events.OnGroundStateChanged?.Invoke(false);

				// Reset parent transform
				rootObject.parent = null;
			}

			// Reset time away from ground
			if (IsGrounded &&
				lastJumpTimer < minTimeBetweenJumpInputs)
			{
				usedJumps = 0;
				timeAwayFromGround = 0;
			}

			// If player landed on moving platform
			if (groundColliderChanged && GroundCollider.GetComponentInParent<MovingPlatform>())
				// Set transform parent to ground so player
				// "sticks" to platform
				rootObject.parent = GroundCollider.transform;
			else if (groundColliderChanged)
				rootObject.parent = null; // Reset parent, no PlatformTransit on ground collider

			// Update animation parameter
			if (animator)
				animator.SetBool("Grounded", IsGrounded);
		}

		/// <summary>
		/// Moves the player horizontally
		/// </summary>
		private void Run()
		{
			float targetSpeed = moveInput * moveSpeed;
			float speedDifference = targetSpeed - _rigidbody.velocity.x;

			// Calculate acceleration
			float accelerationRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
			if (!IsGrounded)
				accelerationRate *= airSpeedMultiplier;

			if (Mathf.Abs(_rigidbody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f)
				accelerationRate = 0; // Prevent deceleration when above target speed, preserves momentum

			float movement = Mathf.Abs(speedDifference) * accelerationRate * Mathf.Sign(speedDifference);

			// Apply movement force to rigidbody
			_rigidbody.AddForce(new Vector2(movement, 0));

			ToggleFootstepsAudio(IsWalkingOnGround(_rigidbody.velocity.sqrMagnitude));
		}

		/// <summary>
		/// Applies artificial force against player (e.g. for deceleration)
		/// </summary>
		/// <param name="amount">Multiplier of force to apply</param>
		private void ApplyDrag(float amount)
		{
			Vector2 force = amount * _rigidbody.velocity.normalized;
			force.x = Mathf.Min(Mathf.Abs(_rigidbody.velocity.x), Mathf.Abs(force.x)) * Mathf.Sign(_rigidbody.velocity.x);
			force.y = Mathf.Min(Mathf.Abs(_rigidbody.velocity.y), Mathf.Abs(force.y)) * Mathf.Sign(_rigidbody.velocity.y);

			// Applies drag force against movement direction
			_rigidbody.AddForce(-force, ForceMode2D.Impulse);
		}

		private IEnumerator StartJump()
		{
			// Calculate jump force
			float forceToApply = jumpForce;
			if (usedJumps == 1)
				forceToApply *= firstJumpMultiplier;

			// Update animator
			if (animator)
				animator.SetTrigger("Jump");

			if(IsGrounded)
				yield return new WaitForSeconds(jumpForceDelay);

			usedJumps++;

			if (usedJumps <= 1)
				events.OnJumped?.Invoke();
			else
				events.OnAirJumped?.Invoke();

			IsGrounded = false;
			events.OnGroundStateChanged?.Invoke(false);
			lastJumpTimer = minTimeBetweenJumpInputs;
			timeAwayFromGround = coyoteTimeframe;

			// Reset vertical velocity if falling
			if (_rigidbody.velocity.y < 0)
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

			// Apply jump force
			_rigidbody.AddForce(Vector2.up * forceToApply, ForceMode2D.Impulse);

			jumpRoutine = null;
		}

		/// <summary>
		/// Check if player is walking and on ground (By Nghia)
		/// </summary>
		/// <param name="speed">Movement speed</param>
		private bool IsWalkingOnGround(float speed)
		{
			// If moving on ground, then play audio
			if (IsGrounded && speed > 0.1f)
				return true;

			return false;
		}

		/// <summary>
		/// Play/Stop footstep audio (by Nghia)
		/// </summary>
		/// <param name="isPlay">Play state</param>
		private void ToggleFootstepsAudio(bool isPlay)
		{
			if (isPlay && !footstepsAudio.isPlaying)
				footstepsAudio.Play();
			else if (!isPlay && footstepsAudio.isPlaying)
				footstepsAudio.Pause();
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			if (!_collider)
				_collider = GetComponent<CapsuleCollider2D>();

			// Draw obstacle check at player feet
			Vector3 origin = transform.position + Vector3.down * (_collider.size.y / 2.0f) + (Vector3)_collider.offset;
			Handles.color = (IsGrounded || !Application.isPlaying) ? Color.green : Color.red;
			Handles.DrawWireDisc(origin, Vector3.forward, obstacleCheckRadius);
		}
#endif
	}
}
