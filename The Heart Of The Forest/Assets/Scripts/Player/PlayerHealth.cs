/*
 * Date Created: 23.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

/*
 * CHANGE LOG:
 * Nghia: Added OnHurt Event,OnDeath Event, Player isAlive check
 * 
 * 13.09.2022 - Lewis
 *  - Added enemy collision detection
 *  - Added knockback w/ enemy hit
 *  - Clamped currentlives in AddLives()
 *  - Added public getters to access current & max lives externally
 *  - Added custom inspector to show readonly lives remaining
 *
 * 15.09.2022 - Nick
 *  - Modified knockback code
 *  
 */

using System;
using UnityEngine;
using HotF.Interactable;
using UnityEngine.Events;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF.Player
{
	public enum PlayerDamageType : byte
	{
		/// <summary>
		/// Used when player's death is not by means otherwise specified
		/// </summary>
		Generic = 0,

		/// <summary>
		/// Death by tooth enemy, or other crushing force
		/// </summary>
		Crushed,

		/// <summary>
		/// Death by poison, generally poison puddle
		/// </summary>
		Poison,

		/// <summary>
		/// Death by projectile, such as from spore-shooting enemy
		/// </summary>
		Projectile,

		/// <summary>
		/// Death by being hit from an enemy's attack
		/// </summary>
		EnemyAttack,

		/// <summary>
		/// Death by overheating the glow ability
		/// </summary>
		Overheat
	}

	/// <summary>
	/// Handles the player health system
	/// </summary>
	public class PlayerHealth : MonoBehaviour
	{
		/* Variables */
		[Header("Health Parameters")]
		[Tooltip("Maximum number of lives the player can have.")]
		[SerializeField, Range(0F, 10F)] private int maxLives = 0;
		[SerializeField] private int currentLives = 0;
		private bool isInvulnerable = false;
		[Tooltip("Players immunity period in seconds.")]
		[SerializeField, Range(0F, 10F)] private float invulnerableCooldown = 0F;

		[SerializeField, Tooltip("Tag to check for enemy collisions")]
		private string enemyTag = "Enemy";

		[SerializeField, Tooltip("Amount of force to apply as knockback when hit")]
		private float knockbackAmount = 100.0f;
		[SerializeField, Tooltip("The height of the knockback when hit")]
		[Range(0, 1)] private float knockbackHeight = .3f;

		[Tooltip("On player hurt event")]
		[SerializeField] UnityEvent OnHurt;

		[Header("Events")]

		[Tooltip("Called whenever player spawns")]
		[SerializeField] private UnityEvent OnSpawn;

		[Tooltip("Called whenever player dies")]
		[SerializeField] private UnityEvent OnDeath;

		[field: SerializeField] public bool DropHeartFragOnDeath { get; private set; } = true;

		[HideInInspector]
		[SerializeField]
		private UnityEvent[] onDiedEvents;

		[SerializeField] public static event Action OnLivesChanged;
		[SerializeField] public static event Action OnPlayerHurt;

		/// <summary>
		/// Is the player alive
		/// </summary>
		public bool isAlive { get; private set; } = true;

		/// <summary>
		/// Maximum amount of lives the player can have
		/// </summary>
		public int MaxLives => maxLives;

		/// <summary>
		/// Current amount of lives the player has
		/// </summary>
		public int CurrentLives => currentLives;

		/// <summary>
		/// Player cannot lose a life while this is active
		/// </summary>
		public bool Invulnerable => isInvulnerable;

		private void OnEnable() => OnSpawn?.Invoke();

		private void Start() => RestoreLives();

		/// <summary>
		/// Handles the players death sequence
		/// </summary>
		private async void OnPlayerDeath(PlayerDamageType damageType)
		{
			if (currentLives > 0 || !isAlive)
				return;

			// Do Something //
			/* Display game over screen
                * Reset player position
                * Restore level to default
                */

			isAlive = false;
			OnDeath?.Invoke();
			onDiedEvents[(int)damageType]?.Invoke();

			gameObject.SetActive(false);

			if (damageType != PlayerDamageType.Poison && DropHeartFragOnDeath)
			{
				// Drop singular heart fragments
				HeartFragInteractable heartFragment = GetComponentInChildren<HeartFragInteractable>(true);
				heartFragment?.UpdateState(HeartFragmentState.UNCOLLECTED);
			}

			await Task.Delay(1000);

			GameController gameController = FindObjectOfType<GameController>();
			if (gameController)
				gameController.RespawnPlayer();

			RestoreLives();

			isInvulnerable = false;
			currentLives = maxLives;
			gameObject.SetActive(true);
			isAlive = true;
		}

		/// <summary>
		/// Restores all lives to their maximum
		/// </summary>
		public void RestoreLives()
		{
			currentLives = maxLives;
			OnLivesChanged?.Invoke();
			isAlive = true;
		}

		/// <summary>
		/// Regenerate lives
		/// </summary>
		/// <param name="livesToAdd">The amount of lives to increase by.</param>
		public void AddLives(int livesToAdd)
		{
			currentLives = Mathf.Clamp(currentLives + livesToAdd, 0, maxLives);
			OnLivesChanged?.Invoke();
		}

		/// <summary>
		/// Deducts lives
		/// </summary>
		/// <param name="livesToDeduct">The amount of lives to decrease by.</param>
		public async void RemoveLives(int livesToDeduct, PlayerDamageType damageType = PlayerDamageType.Generic)
		{
			if (isInvulnerable || currentLives <= 0 || livesToDeduct <= 0) { return; } //Validate if lives can be deducted

			isInvulnerable = true; //Set to invulnerable
			currentLives = Mathf.Clamp(currentLives - livesToDeduct, 0, maxLives); //Deduct lives

			if (currentLives > 0)
			{
				try
				{
					OnHurt.Invoke();
					OnPlayerHurt.Invoke();
					OnLivesChanged?.Invoke();
				}
				catch (Exception e) { Debug.LogException(e); }

				await Task.Delay((int)(invulnerableCooldown * 1000));
				isInvulnerable = false;
			}
			else
				OnPlayerDeath(damageType);
		}

		/// <summary>
		/// Checks for collision against an enemy, using <see cref="enemyTag"/>.
		/// If enemy, applies <see cref="knockbackAmount"/> to this player.
		/// </summary>
		/// <param name="collision"></param>
		private void OnCollisionEnter2D(Collision2D collision)
		{
            if (!collision.gameObject.CompareTag("Enemy"))
                return;

            // Reduce life count by 1
            RemoveLives(1, PlayerDamageType.EnemyAttack);

            // Apply knockback
            if (knockbackAmount > 0.0f && TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 collisionDir;

                //Check direction
                if (transform.position.x < collision.transform.position.x)
                    collisionDir = new Vector2(-1, knockbackHeight);
                else
                    collisionDir = new Vector2(1, knockbackHeight);

                //Add force to player
                rb.AddForce(collisionDir * knockbackAmount, ForceMode2D.Impulse);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag(enemyTag))
                return;

            // Reduce life count by 1
            RemoveLives(1, PlayerDamageType.EnemyAttack);

            // Apply knockback
            if (knockbackAmount > 0.0f && TryGetComponent(out Rigidbody2D rb))
            {
                Vector2 collisionDir;

                //Check direction
                if (transform.position.x < col.transform.position.x)
                    collisionDir = new Vector2(-1, knockbackHeight);
                else
                    collisionDir = new Vector2(1, knockbackHeight);

                //Add force to player
                rb.AddForce(collisionDir * knockbackAmount, ForceMode2D.Impulse);
            }
        }


#if UNITY_EDITOR
		[CustomEditor(typeof(PlayerHealth))]
		public class PlayerHealthEditor : Editor
		{
			private PlayerHealth playerHealth;
			private PlayerDamageType damageType = PlayerDamageType.Generic;

			private void OnEnable() => playerHealth = (PlayerHealth)target;

			public override void OnInspectorGUI()
			{
				PlayerHealth playerHealth = (PlayerHealth)target;
				base.OnInspectorGUI();

				EditorGUILayout.Space();

				string[] playerDeathTypes = Enum.GetNames(typeof(PlayerDamageType));
				if (playerHealth.onDiedEvents.Length != playerDeathTypes.Length)
					playerHealth.onDiedEvents = new UnityEvent[playerDeathTypes.Length];

				SerializedProperty deathEvents = serializedObject.FindProperty($"onDiedEvents");
				for(int i = 0; i < playerDeathTypes.Length; i++)
					EditorGUILayout.PropertyField(deathEvents.GetArrayElementAtIndex(i), new GUIContent($"OnDeath{playerDeathTypes[i]}"));
				serializedObject.ApplyModifiedProperties();

				EditorGUILayout.Space();

				EditorGUILayout.LabelField("State", EditorStyles.boldLabel);
				EditorGUILayout.LabelField($"Lives: {playerHealth.CurrentLives}/{playerHealth.MaxLives}");
				EditorGUILayout.Toggle("Invulnerable", playerHealth.Invulnerable);

				EditorGUILayout.Space();
				if (Application.isPlaying)
				{
					if (GUILayout.Button("Add Life")) playerHealth.AddLives(1);
					if (GUILayout.Button("Restore All Lives")) playerHealth.RestoreLives();

					EditorGUILayout.Space();

					damageType = (PlayerDamageType)EditorGUILayout.EnumPopup("Damage Type", damageType);
					if (GUILayout.Button("Remove Life")) playerHealth.RemoveLives(1, damageType);
					if (GUILayout.Button("KILL!")) playerHealth.RemoveLives(9999, damageType);
				}
			}
		}
#endif

	}
}