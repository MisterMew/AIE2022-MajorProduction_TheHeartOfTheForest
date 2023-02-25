/*
 * Date Created: 13.09.2022
 * Author: Lewis Comstive
 * Contributors: -
 */

/*
 * CHANGE LOG:
 * Nghia: Save functionality setup in code instead of in inspector, added OnExited event
 */

using HotF.Player;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF.Interactable
{
	/// <summary>
	/// Interactable for saving player spawn point & restoring health
	/// </summary>
	[SelectionBase]
	public class WaypointInteractable : Interactable
	{
		[SerializeField, Tooltip("Time, in seconds, to restore lost hearts to player")]
		private float _heartRestoreTime = 1.0f;

		[Tooltip("Event fired for each singular heart restored")]
		public UnityEvent OnLifeAdded;

		[Tooltip("When set as waypoint. Called at start of scene")]
		public UnityEvent OnWaypointActivated;

		[Tooltip("When unset as waypoint. Called at start of scene")]
		public UnityEvent OnWaypointDeactivated;

		[Tooltip("When player interacts to set as waypoint")]
		public UnityEvent OnWaypointNewlySet;

		[Tooltip("When player enters waypoint")]
		public UnityEvent OnEntered;

		[Tooltip("When player exits waypoint")]
		public UnityEvent OnExited;

		/// <summary>
		/// Cache of this scene's GameDataManager
		/// </summary>
		private GameDataManager gameDataManager;

		protected override void Start()
		{
			base.Start();

			CheckGameDataManager();

			if (IsCurrentWaypoint())
				OnWaypointActivated?.Invoke();
			else
				OnWaypointDeactivated?.Invoke();
		}

		/// <summary>
		/// Save game state (added by Nghia)
		/// Placed in seperate funciton to prevent it being called when adding as a delegade or anonymous lambda
		/// </summary>
		private void SaveGameState()
		{
			gameDataManager.SaveGameState(transform);
		}

		/// <summary>
		/// When the player presses the interact button on this waypoint.
		/// 
		/// Restores <see cref="PlayerHealth.CurrentLives"/> up to <see cref="PlayerHealth.MaxLives"/>,
		/// and sets <see cref="GameData.wayPoint"/> to this object.
		/// </summary>
		/// <param name="interaction"></param>
		public async override void Interact(GameObject interaction)
		{
			if (!interaction.TryGetComponent(out PlayerHealth playerHealth))
				return;

			float totalTime = 0;
			float timeDelta = _heartRestoreTime / (playerHealth.MaxLives - playerHealth.CurrentLives);
			int timeDeltaMS = Mathf.RoundToInt(timeDelta * 1000.0f); // Convert seconds to milliseconds
			while (totalTime < _heartRestoreTime &&
					playerHealth.CurrentLives < playerHealth.MaxLives)
			{
				playerHealth.AddLives(1);
				OnLifeAdded?.Invoke();
				await Task.Delay(timeDeltaMS);
				totalTime += timeDelta;
			}

			if(!IsCurrentWaypoint())
				SetAsWaypoint();
			else
				TriggerInteractedEvent();
			// hud.CloseMessagePanel();

			// Add save functionality to waypoint (added by Nghia)
			gameDataManager.SaveGameState(transform);
		}

		/// <summary>
		/// Sets <see cref="GameData.wayPoint"/> to this waypoint
		/// </summary>
		private void SetAsWaypoint()
		{
			// Check for & cache GameDataManager
			if (!CheckGameDataManager() || IsCurrentWaypoint())
				return;

			DeactivateCurrentWaypoint();

			// Set & save waypoint
			gameDataManager.SetWaypoint(transform);
			gameDataManager.SaveWaypoint();

			OnWaypointActivated?.Invoke();
			OnWaypointNewlySet?.Invoke();

			// Debug.Log($"Set waypoint to {transform.position}");
		}

		private void DeactivateCurrentWaypoint()
		{
			// Check all waypoints in scene and deactivate current one
			WaypointInteractable[] waypoints = FindObjectsOfType<WaypointInteractable>();
			foreach (WaypointInteractable waypoint in waypoints)
			{
				if (!waypoint.IsCurrentWaypoint())
					continue;

				waypoint.OnWaypointDeactivated?.Invoke();
				break;
			}
		}

		/// <summary>
		/// Checks if <see cref="gameDataManager"/> exists & it's <see cref="GameData.wayPoint"/> is this waypoint
		/// </summary>
		private bool IsCurrentWaypoint()
		{
			// Check for & cache GameDataManager
			if (!CheckGameDataManager())
				return false;

			// Check if distance to saved waypoint is less than 1 unit
			return Vector3.Distance(transform.position, gameDataManager.gameData.wayPoint) < 1.0f;
		}

		/// <summary>
		/// Checks the scene for a <see cref="GameDataManager"/> and caches it in <see cref="gameDataManager"/>
		/// </summary>
		/// <returns></returns>
		private bool CheckGameDataManager()
		{
			// Check if already cached
			if (gameDataManager)
				return true;
			
			// Find in scene
			gameDataManager = FindObjectOfType<GameDataManager>();

			// Check if found
			if (!gameDataManager)
			{
				Debug.LogWarning("No GameDataManager found in scene");
				return false;
			}
			return true;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.tag == "Player")
			{
				OnEntered.Invoke();
				//collision.GetComponent<Player.PlayerMap>().ToggleCanUse(true);
			}
		}

            private void OnTriggerExit2D(Collider2D collision)
        {
			if (collision.tag == "Player")
			{
				OnExited.Invoke();
				//collision.GetComponent<Player.PlayerMap>().ToggleCanUse(false);
			}
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(WaypointInteractable))]
		public class WaypointInteractableEditor : Editor
		{
			private WaypointInteractable waypoint;

			private void OnEnable() => waypoint = (WaypointInteractable)target;

			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();

				EditorGUILayout.Space();
				if (GUILayout.Button("Set as current waypoint"))
					waypoint.SetAsWaypoint();

				EditorGUILayout.Toggle("Current waypoint", waypoint.IsCurrentWaypoint());
		}
		}
#endif
	}
}