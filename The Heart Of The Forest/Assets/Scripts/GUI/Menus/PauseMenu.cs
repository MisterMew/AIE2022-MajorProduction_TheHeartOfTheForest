/*
 * Date Created: 06.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.GUI
{
    /// <summary>
    /// Handles the functinoality behind the pause menus navigation system
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        /* Variables */
        private PauseGameManager gamePauseHandler;
        private MenuManager menuManager;
        private SceneTransition sceneTransition;

        /// <summary>
        /// Called once before start
        /// </summary>
        private void Awake()
        {
            gamePauseHandler = FindObjectOfType<PauseGameManager>();
            menuManager = FindObjectOfType<MenuManager>();
            sceneTransition = FindObjectOfType<SceneTransition>();
        }

        /// <summary>
        /// Resumes the game from the pause state
        /// </summary>
        public void ResumeGame()
        {
            gamePauseHandler.PauseGame(false);
        }

        /// <summary>
        /// Takes the user to the options menu
        /// </summary>
        public void OptionsMenu()
        {
            menuManager.pauseMenuUI.SetActive(false);   //Disable pause menu
            menuManager.optionsMenuUI.SetActive(true); //Enable options menu
        }

        /// <summary>
        /// Takes the user to the statistics menu
        /// </summary>
        public void StatisticsMenu()
        {
            menuManager.pauseMenuUI.SetActive(false);      //Disable pause menu
            menuManager.statisticsMenuUI.SetActive(true); //Enable options menu
        }

        /// <summary>
        /// Returns the user to the main menu scene
        /// </summary>
        public void ReturnToMainMenu()
        {
            sceneTransition.TransitionToScene(0); //Transition to main menu
            ResumeGame();
        }

        /// <summary>
        /// Quits the application
        /// </summary>
        public void QuitGame()
        {
            menuManager.quitGamePrompt.SetActive(true); //Enable confirmation prompt
        }

        /// <summary>
        /// Confirm whether to quit game
        /// </summary>
        /// <returns></returns>
        public void ConfirmQuitGame(bool confirmed)
        {
            if (confirmed)
            {
                gamePauseHandler.PauseGame(false);
                menuManager.quitGamePrompt.SetActive(false); //Disable confirmation prompt
                menuManager.QuitGame();
            }
        }
    }
}