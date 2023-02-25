/*
 * Date Created: 06.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.GUI
{
    /// <summary>
    /// Stores all the games menus and prompts
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        /* Variables */
        [Header("Main Menus")]
        public GameObject mainMenuUI = null;
        public GameObject confirmationPrompt = null;
        public GameObject creditsMenuUI = null;

        [Header("In Game Menus")]
        public GameObject hudOverlayUI = null;
        public GameObject gameCompletionPrompt = null;
        public GameObject mapUI = null;
        public GameObject pauseMenuUI = null;
        public GameObject quitGamePrompt = null;
        public GameObject optionsMenuUI = null;
        public GameObject statisticsMenuUI = null;

        [Header("Options Menus")]
        public GameObject optionsAudioMenuUI = null;
        public GameObject optionsGraphicsMenuUI = null;
        public GameObject optionsGameplayMenuUI = null;
        public GameObject optionsControlsMenuUI = null;

        /// <summary>
        /// Quits the application
        /// </summary>
        public void QuitGame() => Application.Quit();
    }
}