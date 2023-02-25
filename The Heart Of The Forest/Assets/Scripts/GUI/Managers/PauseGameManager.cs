/*
 * Date Created: 06.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

/*
 * CHANGE LOG:
 * Nghia: keeps time paused if map was open
 */

using Hotf.Player.Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF.GUI
{
    /// <summary>
    /// Handles the games pause state
    /// </summary>
    public class PauseGameManager : MonoBehaviour
    {
        /* Variables */
        private MenuManager menuManager;
        private PlayerInteractions playerInteractions;
        private Player.PlayerMap playerMap; // added by Nghia

        [SerializeField] private InputActionReference pauseInput = null;
        public bool gamePaused = false;


        private void Awake()
        {
            menuManager = FindObjectOfType<MenuManager>();
            playerInteractions = FindObjectOfType<PlayerInteractions>();
            playerMap = FindObjectOfType<Player.PlayerMap>(); // added by Nghia
        }

        private void OnEnable() => pauseInput.action.started += PauseInput; //Subsribe to pause input detection
        private void OnDisable() => pauseInput.action.started -= PauseInput; //Unsubscribe to pause input detection

        private void PauseInput(InputAction.CallbackContext obj) => HandlePauseState();

        /// <summary>
        /// Called when the application is no longer in focus
        /// </summary>
        /// <param name="appInFocus"></param>
#if !UNITY_EDITOR
        private void OnApplicationFocus(bool appInFocus)
        {
            if (!appInFocus)
            {
                if (gamePaused) return;
                else PauseGame(true);
            }
        }
#endif

        /// <summary>
        /// Validates what the games current pause state is, and decides what to do
        /// </summary>
        public void HandlePauseState()
        {
            if (OptionsMenuOpen()) return;

            if (gamePaused) //If game is paused
                PauseGame(false); //unpause the game  

            else if (!gamePaused) //If game is not paused
                PauseGame(true); //Pause the game
        }

        /// <summary>
        /// Check if options menu is open
        /// </summary>
        /// <returns></returns>
        private bool OptionsMenuOpen()
        {
            if (menuManager.optionsMenuUI.activeSelf == true ||
                menuManager.optionsGraphicsMenuUI.activeSelf == true ||
                menuManager.optionsControlsMenuUI.activeSelf == true ||
                menuManager.optionsAudioMenuUI.activeSelf == true ||
                menuManager.optionsGameplayMenuUI.activeSelf == true)
                return true;

            else
                return false;
        }

        /// <summary>
        /// Controls whether the game will pause or unpause
        /// </summary>
        /// <param name="pauseGame">true to pause the game, false to resume the game.</param>
        public void PauseGame(bool pauseGame)
        {
            menuManager.hudOverlayUI.SetActive(!pauseGame);  //Handles game overlay
            menuManager.pauseMenuUI.SetActive(pauseGame); //Handles pause menu

            // If map is on when unpausing, keep time frozen (added by Nghia)
            if (!playerMap.IsOn)
                Time.timeScale = pauseGame ? 0 : 1;           //Handles time scale
            
            playerInteractions.enabled = !pauseGame;

            gamePaused = pauseGame;
        }
    }
}