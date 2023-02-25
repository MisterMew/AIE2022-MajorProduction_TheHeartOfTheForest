/*
 * Date Created: 19.10.2022
 * Author: Nghia Do
 * Contributors: 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF.Player
{
    /// <summary>
    /// Player map
    /// </summary>
    public class PlayerMap : MonoBehaviour
    {
        [Header("Map")]
        [Tooltip("Map action reference")]
        [SerializeField] private InputActionReference mapToggleInput;
        [Tooltip("Game Controller")]
        [SerializeField] private GameController gameController;
        [Tooltip("Map UI")]
        [SerializeField] private GameObject mapUI;

        [Header("Map Icon Prefabs")]
        [Tooltip("Player image Prefab")]
        [SerializeField] private GameObject playerIconPrefab;
        [Tooltip("Heart Fragment image Prefab")]
        [SerializeField] private GameObject hfIconPrefab;
        [Tooltip("Waypoint image Prefab")]
        [SerializeField] private GameObject waypointIconPrefab;

        [Header("Dimentions")]
        [Tooltip("Center offset")]
        [SerializeField] private Vector3 centerOffset;
        [Tooltip("Non-Zero Half-Extents \n[x = Half-Width] [y = Half-Height] [z = Half-Depth]")]
        [SerializeField] private Vector3 worldHalfExtents;
        
        [Header("Objects to track")]
        [Tooltip("Player")]
        [SerializeField] private PlayerHealth player;
        [Tooltip("Heart Fragment list")]
        [SerializeField] private Interactable.HeartFragInteractable[] heartFragments;
        [Tooltip("Waypoint list")]
        [SerializeField] private Interactable.WaypointInteractable[] waypoints;

        [Header("Settings")]
        [Tooltip("Can use map at any time")]
        [SerializeField] private bool canAlwaysUse = false;

        /// <summary>
        /// Pause Game Manager
        /// </summary>
        private GUI.PauseGameManager pauseGameManager;

        /// <summary>
        /// Map Half Extents
        /// [x = Half-Width] [y = Half-Height] [z = Half-Depth]
        /// </summary>
        private Vector3 mapHalfExtents;

        /// <summary>
        /// Map Action
        /// </summary>
        private InputAction mapAction;

        // Map Icons 
        private GameObject playerIcon = null;
        private List<GameObject> hfIcons = new List<GameObject>();
        private List<GameObject> waypointIcons = new List<GameObject>();

        /// <summary>
        /// Is the map currently on
        /// </summary>
        private bool isOn = false;

        /// <summary>
        /// Can the map currently be used
        /// </summary>
        public bool CanUse { get; private set; }

        /// <summary>
        /// Is currently map on
        /// </summary>
        public bool IsOn 
        {
            get { return isOn; }
            set { isOn = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        /// <summary>
        /// Set up player map
        /// </summary>
        public void Setup()
        {
            isOn = false;
            CanUse = false;
            mapAction = GetComponent<PlayerInput>().actions.FindAction(mapToggleInput.action.id);

            // Set Screen dimentions
            SetHalfExtents();

            // Find what has not been set
            if (!gameController) gameController = FindObjectOfType<GameController>();
            if (!player) player = GetComponent<PlayerHealth>();
            if (heartFragments.Length == 0 && hfIconPrefab) heartFragments = FindObjectsOfType<Interactable.HeartFragInteractable>();
            if (waypoints.Length == 0 && waypointIconPrefab) waypoints = FindObjectsOfType<Interactable.WaypointInteractable>();
            pauseGameManager = FindObjectOfType<GUI.PauseGameManager>();

            // Turn map off at start
            ToggleMapUIState(false);

            // Init icons for each waypoint if not already
            if (waypointIcons.Count == 0)
            {
                for (int hfIdx = 0; hfIdx < waypoints.Length; hfIdx++)
                {
                    waypointIcons.Add(Instantiate(waypointIconPrefab, mapUI.transform));
                }
            }

            // Init icons for each Heart Fragment if not already
            if (hfIcons.Count == 0)
            {
                for (int hfIdx = 0; hfIdx < heartFragments.Length; hfIdx++)
                {
                    hfIcons.Add(Instantiate(hfIconPrefab, mapUI.transform));
                }
            }

            // Init icon for player if it doesn't already exsist
            if (playerIconPrefab && !playerIcon) playerIcon = Instantiate(playerIconPrefab, mapUI.transform);
        }

        /// <summary>
        /// Destroy instanciated icons
        /// </summary>
        private void OnDestroy()
        {
            Destroy(playerIcon);

            for (int hfIdx = 0; hfIdx < hfIcons.Count; hfIdx++)
            {
                Destroy(hfIcons[hfIdx]);
            }

            for (int wpIdx = 0; wpIdx < waypointIcons.Count; wpIdx++)
            {
                Destroy(waypointIcons[wpIdx]);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        /// <summary>
        /// Set active state of Map UI
        /// </summary>
        /// <param name="state">State to set to</param>
        public void ToggleMapUIState(bool state) => mapUI.SetActive(state);

        /// <summary>
        /// Set halfExtents based on screen dimentions
        /// </summary>
        private void SetHalfExtents()
        {
            mapHalfExtents.x = Screen.width / 2;
            mapHalfExtents.y = Screen.height / 2;
            mapHalfExtents.z = 0;
        }

        /// <summary>
        /// Toggle usable state of map
        /// </summary>
        /// <param name="useable"></param>
        public void ToggleCanUse(bool useable)
        {
            // if can always use or is already in correct state, then don't toggle
            if (canAlwaysUse || CanUse == useable) return;

            CanUse = useable;
            if (CanUse) mapAction.started += ToggleMap;
            else mapAction.started -= ToggleMap;
        }

        /// <summary>
        /// Toggle map using input
        /// </summary>
        /// <param name="_"></param>
        private void ToggleMap(InputAction.CallbackContext _)
        {
            // Can't use map while game is paused
            if (pauseGameManager.gamePaused) return;

            ToggleMapState(!isOn);

            // Exit if map is off
            if (!isOn) return;

            SetHalfExtents();
            RefreshIcons();
        }

        /// <summary>
        /// Toggle on/off state of map
        /// </summary>
        /// <param name="state">Map state</param>
        private void ToggleMapState(bool state)
        {
            isOn = state;

            ToggleMapUIState(isOn);
            Time.timeScale = isOn ? 0 : 1; // Pause if map is on
        }

        /// <summary>
        /// Convert game transform space to map UI transfrom space
        /// </summary>
        /// <param name="possition">Possition in world space</param>
        /// <returns>Possition in screen screen space</returns>
        private Vector3 ConvertToMapSpace(Vector3 possition)
        {
            // mapHalfExtents * possition / worldHalfExtents
            return centerOffset + new Vector3(  mapHalfExtents.x * possition.x / worldHalfExtents.x,
                                                mapHalfExtents.y * possition.y / worldHalfExtents.y,
                                                mapHalfExtents.z * possition.z / worldHalfExtents.z );
        }

        /// <summary>
        /// Refresh icon positions on map
        /// </summary>
        public void RefreshIcons()
        {
            // Refresh Player position on map
            playerIcon.transform.position = mapUI.transform.position + ConvertToMapSpace(player.transform.position);

            // Refresh Heart Fragment positions on map, if they're in the level
            for (int hfIdx = 0; hfIdx < hfIcons.Count; hfIdx++)
            {
                if (heartFragments[hfIdx].gameObject.activeSelf)
                    hfIcons[hfIdx].transform.position = mapUI.transform.position + ConvertToMapSpace(heartFragments[hfIdx].transform.position);
                else hfIcons[hfIdx].SetActive(false);
            }

            // Refresh Waypoint positions on map
            for (int wpIdx = 0; wpIdx < waypointIcons.Count; wpIdx++)
            {
                waypointIcons[wpIdx].transform.position = mapUI.transform.position + ConvertToMapSpace(waypoints[wpIdx].transform.position);
            }
        }
    }
}