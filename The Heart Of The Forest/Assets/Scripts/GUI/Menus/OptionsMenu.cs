/*
 * Date Created: 10.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.GUI
{
    /// <summary>
    /// Handles the functinoality for bavigating the options menu
    /// </summary>
    public class OptionsMenu : MonoBehaviour
    {
        /* Variables */
        private MenuManager menuManager;
        private GameObject latestMenuCache = null; //Caches the last accessed/current menu


        private void Awake() => menuManager = FindObjectOfType<MenuManager>(); //Finds the menu Manager
        

        /// <summary>
        /// Returns back to the main menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            menuManager.optionsMenuUI.SetActive(false);
            menuManager.mainMenuUI.SetActive(true);
        }

        /// <summary>
        /// Returns the user back to the options menu from any menu
        /// </summary>
        public void ReturnToOptions() 
        {
            latestMenuCache.gameObject.SetActive(false);
            menuManager.optionsMenuUI.SetActive(true);
        }

        /// <summary>
        /// Opens the audio settings
        /// </summary>
        public void AudioSettingsMenu() 
        { 
            menuManager.optionsMenuUI.SetActive(false);
            menuManager.optionsAudioMenuUI.SetActive(true);
            latestMenuCache = menuManager.optionsAudioMenuUI; //Caches the Audio menu
        }

        /// <summary>
        /// Opens the graphics settings
        /// </summary>
        public void GraphicsSettingsMenu()
        {
            menuManager.optionsMenuUI.SetActive(false);
            menuManager.optionsGraphicsMenuUI.SetActive(true);
            latestMenuCache = menuManager.optionsGraphicsMenuUI; //Caches the Graphics menu
        }

        /// <summary>
        /// Opens the gameplay settings menu
        /// </summary>
        public void GameplaySettingsMenu()
        {
            menuManager.optionsMenuUI.SetActive(false);
            menuManager.optionsGameplayMenuUI.SetActive(true);
            latestMenuCache = menuManager.optionsGameplayMenuUI; //Caches the Gameplay menu
        }

        /// <summary>
        /// Opens the controls settings menu
        /// </summary>
        public void ControlsSettingsMenu()
        {
            menuManager.optionsMenuUI.SetActive(false);
            menuManager.optionsControlsMenuUI.SetActive(true);
            latestMenuCache = menuManager.optionsControlsMenuUI; //Caches the Controls menu
        }
    }
}