/*
 * Date Created: 04.09.2022
 * Author: Nghia
 * Contributors: Jazmin
 */

/*
 * CHANGE LOG:
 * Jazmin: Updated GraphicsSettings variable to GraphicsManager
 */

using HotF.Graphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotF
{
    /// <summary>
    /// Main Game Controller
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("Managers")]
        [Tooltip("Game Data Manager")]
        [SerializeField] GameDataManager gameDataManager;
        [Tooltip("Menu Manager")]
        [SerializeField] private GUI.MenuManager menuManager;
        [Tooltip("Audio Manager")]
        [SerializeField] private AudioManager audioManager;
        [Tooltip("Graphics Settings")]
        [SerializeField] private GraphicsManager graphicsSettings;
        [Tooltip("Camera Manager")]
        [SerializeField] private CameraManager cameraManager;
        [Tooltip("SceneManager")]
        [SerializeField] private SceneTransition sceneTransition;
        [Tooltip("God Interactable")]
        [SerializeField] private Interactable.GodInteractable godInteractable;

        [Header("Player")]
        [Tooltip("In game player")]
        [SerializeField] private GameObject player;
        [Tooltip("Spawn Point (set this to default spawn point)")]
        [HideInInspector] public Vector3 spawnPoint;

        [Header("Asset Switchers")]
        [Tooltip("AssetSwicthers")]
        [SerializeField] private Environment.AssetSwitcher[] assetSwitchers;
        [Tooltip("Background AssetSwitcher")]
        [SerializeField] private Environment.AssetSwitcher[] bgAssetSwitchers;
        [Tooltip("Exit AssetSwitcher")]
        [SerializeField] private Environment.AssetSwitcher[] exitAssetSwitchers;
        [Tooltip("Blur AssetSwitcher")]
        [SerializeField] private Environment.AssetSwitcher blurAssetSwitchers;

        [Header("Dissolve Controlers")]
        [Tooltip("Dissolve Controlers")]
        [SerializeField] private DissovleController[] dissolveControllers;

        [Header("Intro Cutscene")]
        [Tooltip("Play Intro on new game start?")]
        [SerializeField] private bool playIntroCutscene = true;
        [Tooltip("Intro cutscene object")]
        [SerializeField] private GameObject introCutsceneObject;
        [Tooltip("New game Intro Cutscene")]
        [SerializeField] private UnityEngine.Playables.PlayableDirector introCutscene;

        [Header("New Game")]
        [SerializeField] private GameObject continueButton;

        [Header("End Game")]
        [SerializeField] private GameObject endGamePanel;

        private const string inputActionGameplayStr = "Gameplay";
        private const string inputActionUiStr = "UI";

        /// <summary>
        /// Setup GameData
        /// </summary>
        private void SetupGameData()
        {
            // Null check
            if (!gameDataManager) return;

            gameDataManager.Setup();

            // Reset all save data if the Save version is different from current version
            if (!gameDataManager.IsSaveVersionCurrent())
            {
                if (continueButton != null) continueButton.SetActive(false);
                gameDataManager.ResetAllToDefault();
            }

                gameDataManager.LoadSettings();
            gameDataManager.LoadGameState();
        }

        /// <summary>
        /// Setup Heart Fragments
        /// </summary>
        private void SetupHeartFragments()
        {
            // Setup Heart Fragments
            for (int hfIdx = 0; hfIdx < gameDataManager.heartFragmentList.Length; hfIdx++)
            {
                switch (gameDataManager.heartFragmentList[hfIdx].State)
                {
                    // TODO CALL UPDATE STATE ON HF
                    case Interactable.HeartFragmentState.UNCOLLECTED:
                        // Do nothing
                        break;
                    case Interactable.HeartFragmentState.COLLECTED:
                        // Add Heart Fragment to player inventory
                        break;
                    case Interactable.HeartFragmentState.RETURNED:
                        // Remove already collected Heart Fragments
                        gameDataManager.heartFragmentList[hfIdx].gameObject.SetActive(false);
                        break;
                }
            }
        }

        /// <summary>
        /// Setup Waypoint
        /// </summary>
        public void SetupWaypoint()
        {
            gameDataManager.LoadWaypoint();
        }

        /// <summary>
        /// Setup AssetSwitchers
        /// </summary>
        public void SetupAssetSwitchers()
        {
            // Setup blur asset switchers
            blurAssetSwitchers.Setup();
            blurAssetSwitchers.SwitchSprites();

            // Setup exit asset switchers
            for (int idx = 0; idx < exitAssetSwitchers.Length; idx++)
            {
                exitAssetSwitchers[idx].Setup();
            }

            // Switch to corrupted assets if respective HF not returned (excluding village)
            for (int idx = 0; idx < assetSwitchers.Length && idx < gameDataManager.heartFragmentList.Length; idx++)
            {
                // Setup background asset switchers
                bgAssetSwitchers[idx].Setup();
                bgAssetSwitchers[idx].SwitchMaterials();

                assetSwitchers[idx].Setup();
                dissolveControllers[idx].Setup(-1);

                // If uncollected, then close shortcuts
                if (gameDataManager.heartFragmentList[idx].State == Interactable.HeartFragmentState.UNCOLLECTED)
                    assetSwitchers[idx].SwitchGameObjects();

                // If first Heart Fragment and collected, then close exits
                else if (idx == 0 && gameDataManager.heartFragmentList[idx].State == Interactable.HeartFragmentState.COLLECTED)
                    assetSwitchers[idx].SwitchGameObjects();

                // If not returned then switch to coruppted
                if (gameDataManager.heartFragmentList[idx].State != Interactable.HeartFragmentState.RETURNED)
                {
                    UpdateWorldState(idx);
                }

                // If returned, then switch materials to purifyed
                else
                {
                    dissolveControllers[idx].Setup(1);

                    //bgAssetSwitchers[idx].SwitchMaterials();
                    // If the tutorial heart fragment is returned then switch town area
                    //if (idx == 0) bgAssetSwitchers[4].SwitchMaterials();
                }
            }
        }

        /// <summary>
        /// Setup Menus
        /// </summary>
        private void SetupMenus()
        {
            // Null check
            if (!menuManager) return;

            //menuManager.Setup();
        }

        /// <summary>
        /// Setup Audio Manager
        /// </summary>
        private void SetupAudio()
        {
            // Null check
            if (!audioManager) return;

            audioManager.Setup();
            audioManager.PlayBgm();
        }

        /// <summary>
        /// Setup GraphicsSettings
        /// </summary>
        private void SetupGraphics()
        {
            if (!graphicsSettings) return;

            graphicsSettings.Setup();
        }

        /// <summary>
        /// Setup Player
        /// </summary>
        private void SetupPlayer()
        {
            // Find player if not set
            if (!player) player = FindObjectOfType<Player.PlayerMovement>()?.gameObject;

            // Null check
            if (!player) return;

            // Spawn player at last savepoint
            player.transform.position = spawnPoint;
            player.GetComponent<Player.PlayerMap>().Setup();
        }

        /// <summary>
        /// Debug respawn player
        /// </summary>
        //public void Debug_RespawnPlayer()
        //{
        //    gameDataManager.LoadWaypoint();
        //    player.transform.position = spawnPoint;
        //}

        /// <summary>
        /// Respawn player and setup world state
        /// </summary>
        public void RespawnPlayer()
        {
            gameDataManager.SaveHeartFragments();

            SetupWaypoint();
            SetupPlayer();
            SetupHeartFragments();
            //SetupCamera();
        }

        /// <summary>
        /// Setup Camera
        /// </summary>
        public void SetupCamera()
        {
            // Null check
            if (!cameraManager) return;

            cameraManager.Setup();
            cameraManager.SetCamTarget(player.transform);
        }

        /// <summary>
        /// Setup IntroCutscene
        /// </summary>
        public void SetupIntroCutscene()
        {
            if (!introCutscene) return;

            introCutscene.Play();
        }

        /// <summary>
        /// Setup GodInteractable
        /// </summary>
        public void SetupGod()
        {
            if (!godInteractable) return;

            godInteractable.Setup();
        }

        /// <summary>
        /// Setup Main Menu Scene
        /// </summary>
        private void SetupMainMenuScene()
        {
            SetupGameData();
            SetupMenus();
            SetupAudio();
            SetupGraphics();
            //SetupPlayer();
            SwitchPlayerInputToUi();
        }

        /// <summary>
        /// Set up game scene
        /// </summary>
        private void SetupGameScene()
        {
            // Set default spawn point if not set
            if (!gameDataManager.DefaultSpawnPoint && player) gameDataManager.DefaultSpawnPoint = player.transform;
            
            // If intro cutscene is avaliable and you want to play it, then play it
            if (gameDataManager.gameData.isNewGame && 
                introCutsceneObject != null && introCutsceneObject != null && playIntroCutscene)
            {
                SetupGameData();
                SetupHeartFragments();
                SetupWaypoint();
                //SetupAssetSwitchers();
                SetupMenus();
                SetupAudio();
                SetupGraphics();
                SetupPlayer();
                SetupGod(); // move to end of cutscene
                SetupCamera(); 
                SwitchPlayerInputToGameplay();
                player.GetComponent<Player.PlayerMap>().ToggleCanUse(true);

                // Setup Intro Cutscene
                cameraManager.gameObject.SetActive(false);
                introCutsceneObject.SetActive(true);
                SetupIntroCutscene();
            }
            // Game setup without intro cutscene
            else
            {
                SetupGameData();
                SetupHeartFragments();
                SetupWaypoint();
                SetupAssetSwitchers();
                SetupMenus();
                SetupAudio();
                SetupGraphics();
                SetupPlayer();
                SetupGod();
                SetupCamera();
                SwitchPlayerInputToGameplay();
                player.GetComponent<Player.PlayerMap>().ToggleCanUse(true);
            }

            // Old setup
            //SetupGameData();
            //SetupHeartFragments();
            //SetupWaypoint();
            //SetupAssetSwitchers();
            //SetupMenus();
            //SetupAudio();
            //SetupGraphics();
            //SetupPlayer();
            //SetupCamera();
            //SwitchPlayerInputToGameplay();
        }

        /// <summary>
        /// Set up Credits Scene
        /// </summary>
        private void SetupCreditsScene()
        {
            SwitchPlayerInputToUi();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Find player if not set
            if (!player) player = FindObjectOfType<Player.PlayerMovement>()?.gameObject;


            // Setup based on current scene
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0: // Main Menu Scene
                    SetupMainMenuScene();
                    break;

                case 1: // Game Scene
                    SetupGameScene();
                    break;

                case 2: // Credits Scene
                    SetupCreditsScene();
                    break;
                default: // Test Scenes
                    SetupGameScene();
                    break;
            }

        }

        /// <summary>
        /// Update world state
        /// </summary>
        /// <param name="index">Index of Heart Fragment returned</param>
        public void UpdateWorldState(int index)
        {
            //Debug.Log("GC: UpdadeWorldState: " + index);
            // Update world based on number of HF collected
            assetSwitchers[index].SwitchMaterials();
            assetSwitchers[index].SwitchSprites();
            assetSwitchers[index].SwitchAudioClips();
            //assetSwitchers[index].SwitchGameObjects();

            // If the tutorial heart fragment is returned then switch town area
            if (index == 0)
            {
                assetSwitchers[4].SwitchMaterials();
                assetSwitchers[4].SwitchSprites();
                assetSwitchers[4].SwitchAudioClips();
                //assetSwitchers[index].SwitchGameObjects();

                //bgAssetSwitchers[4].SwitchMaterials();
            }

            // Set background to match current ground type
            //bgAssetSwitchers[index].SwitchMaterials();

            // Save game state
            gameDataManager.SaveGameState();
        }

        /// <summary>
        /// Activate/deactivate player controls
        /// </summary>
        /// <param name="state"></param>
        public void SetPlayerControlsState(bool state)
        {
            // Find player if not set (for some reason the plyer is Null at this point)
            if (!player) player = FindObjectOfType<Player.PlayerMovement>()?.gameObject;

            // Do not switch states if it is the same state
            if (player.GetComponent<Player.PlayerMovement>().enabled == state)
                return;
            
            //Debug.Log("Player Controls Set: " + state);

            player.GetComponent<Player.PlayerMovement>().enabled = state;
            player.GetComponent<Player.PlayerAbilityHandler>().enabled = state;
            player.GetComponent<Player.PlayerMap>().ToggleCanUse(state);
        }

        /// <summary>
        /// Switch input action map to Gameplay
        /// </summary>
        public void SwitchPlayerInputToGameplay() => player?.GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap(inputActionGameplayStr);

        /// <summary>
        /// Switch input action map to UI
        /// </summary>
        public void SwitchPlayerInputToUi() => player?.GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap(inputActionUiStr);

        /// <summary>
        /// Check for end of game to go to credits
        /// </summary>
        public void CheckGameEnd()
        {
            if (gameDataManager.gameData.heartFragmentsReturned >= 4)
            {
                // Ask before ending the game
                if (endGamePanel)
                {
                    SetPlayerControlsState(false);
                    endGamePanel.SetActive(true);
                }
                else
                {
                    sceneTransition.TransitionToScene(2);
                }
            }
        }
    }

}