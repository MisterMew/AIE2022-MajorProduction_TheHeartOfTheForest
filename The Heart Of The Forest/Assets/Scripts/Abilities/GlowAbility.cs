/*
 * 
 * Date Created: 29.08.2022
 * Author: Lewis Comstive
 *
 */

/*
 * 
 * Changelog:
 *	Lewis - Initial creation
 *  Nghia - Added event for OnOverheating
 */

using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace HotF.Abilities
{
	/// <summary>
	/// Creates a light, scales a child gameobject when activated
	/// </summary>
	[CreateAssetMenu(fileName = "Glow", menuName = "HotF/Abilities/Glow")]
	public class GlowAbility : ToggleableAbility
	{
		[Tooltip("Radius the ability can reach")]
		public float Radius = 5.0f;

		[Tooltip("Offset where light is spawned, relative to caster")]
		public Vector3 SpawnOffset = new Vector3(0, 0, -1);

		[Tooltip("Curve for radius size of visual when activating/deactivating ability")]
		public AnimationCurve ActivateCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

		[Tooltip("Prefab to spawn while ability is active")]
		public GameObject GlowPrefab;

		[Header("Overheating")]

		[Tooltip("Maximum time the player can glow, in seconds, before overheating")]
		public float MaxGlowTime = 10.0f;

		[Tooltip("Scales rate that glow time increases when ability is active")]
		public float GlowTimeMultiplier = 1.0f;

		[Tooltip("Scales rate that glow time regenerates when not in use")]
		public float RegenerationMultiplier = 1.0f;

		[Tooltip("Scales rate of glow time regeneration while waiting for cooldown to finish. During this time the ability cannot be activated")]
		public float OverheatCooldownMultiplier = 3.5f;

		/// <summary>
		/// Current time spent with ability active.
		/// 
		/// Once this value reaches <see cref="MaxGlowTime"/>, deactivates this ability and 
		/// cannot be activated again until this value reaches 0.
		/// 
		/// When ability is active, this value increases by <see cref="GlowTimeMultiplier"/> each second.
		/// When ability is not active, this value decreases by <see cref="RegenerationMultiplier"/> each second.
		/// When ability is overheated, this value decreases by <see cref="OverheatCooldownMultiplier"/> each second.
		/// 
		/// Range is [0 - MaxGlowTime]
		/// </summary>
		public float GlowTime { get; private set; } = 0;

		/// <summary>
		/// True when <see cref="GlowTime"/> exceeds <see cref="MaxGlowTime"/>.
		/// Resets to false when <see cref="GlowTime"/> reaches 0 again.
		/// </summary>
		public bool OnCooldown { get; private set; } = false;

		private Light lighting;
		private Transform glowTransform;

		/// <summary>
		/// Event when overheating (added by Nghia)
		/// </summary>
		public UnityEvent OnOverheating;

		public override void OnReset()
		{
			GlowTime = 0;
			CanToggle = true;
			IsActive = false;
			OnCooldown = false;

			if (glowTransform)
				Destroy(glowTransform.gameObject);

			lighting = null;
			glowTransform = null;
		}

		public override void OnUpdate()
		{
			// Update glow time depending if ability is active or not
			float glowTimeMultiplier = OnCooldown ? OverheatCooldownMultiplier : (IsActive ? -GlowTimeMultiplier : RegenerationMultiplier);
			GlowTime = Mathf.Clamp(GlowTime - Time.deltaTime * glowTimeMultiplier, 0, MaxGlowTime);

			// Check for overheating
			if(GlowTime >= MaxGlowTime && IsActive)
			{
				// Overheating occurred
				Activate(); // Toggle ability OFF
				OnOverheating.Invoke(); // Added by Nghia
				OnCooldown = true;
			}

			// Check if overheat completed
			if (!CanToggle && OnCooldown && GlowTime == 0)
			{
				// Overheat has completed, reset cooldown state & allow ability to be activated
				CanToggle = true;
				OnCooldown = false;
			}
		}

		protected override bool OnActiveStateChanged()
		{
			DoGlow();
			return true;
		}

		private async void DoGlow()
		{
			bool isActive = IsActive;

			// Don't let the ability state change while activating/de-activating
			CanToggle = false;

			// Activating ability, create child object
			if (!isActive)
				CreateGlowGameObject();

			float totalTime = 0;
			float desiredTime = ActivateCurve.keys[ActivateCurve.keys.Length - 1].time;
			while(totalTime < desiredTime)
			{
				// Calculate radius based on total time passed
				float time = !isActive ? totalTime : (desiredTime - totalTime);
				float radius = Radius * ActivateCurve.Evaluate(time);

				// Update values
				glowTransform.localScale = Vector3.one * radius;

				if(lighting)
					lighting.range = radius;

				// Wait and loop
				float deltaTime = Time.deltaTime;
				await Task.Delay((int)(deltaTime * 1000));
				totalTime += deltaTime;

				// See if playmode has been exited
				if (!Application.isPlaying)
				{
					OnReset();
					return;
				}
			}

			// De-activating ability, destroy child object
			if (isActive)
				Destroy(glowTransform.gameObject);

			// Now the ability state can change without issue
			CanToggle = !OnCooldown;
		}

		/// <summary>
		/// Spawns an object as child of caster with relevant components
		/// </summary>
		private void CreateGlowGameObject()
		{
			// Create visuals object
			GameObject gameObject = Instantiate(GlowPrefab, Caster.transform);
			glowTransform = gameObject.transform;
			glowTransform.localPosition = SpawnOffset;

			// Try to get light component at root of spawned object
			lighting = gameObject.GetComponentInChildren<Light>();
		}
	}
}
