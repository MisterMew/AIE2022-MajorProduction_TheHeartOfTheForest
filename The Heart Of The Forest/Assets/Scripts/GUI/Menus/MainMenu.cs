/*
 * Date Created: 07.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotF.GUI
{
    /// <summary>
    /// Handles the functionality behind the main menus navigation
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /* Variables */
        private MenuManager menuManager;
        private SceneTransition sceneTransition;

        private void Awake()
        {
            sceneTransition = FindObjectOfType<SceneTransition>(); //Finds the scene transitioner
            menuManager = FindObjectOfType<MenuManager>(); //Finds the menu Manager
        }

        /// <summary>
        /// Load the current game from the saved player prefs
        /// </summary>
        public void ContinueCurrentGame()
        {
            sceneTransition.TransitionToScene(1); //Transitions to the game scene
        }

        /// <summary>
        /// Load a new game
        /// </summary>
        public void NewGame()
        {
            menuManager.confirmationPrompt.SetActive(true); //Enable confirmation prompt
        }

        /// <summary>
        /// Confirm whether to load a new game
        /// </summary>
        /// <returns></returns>
        public void ConfirmNewGame(bool confirmed)
        {
            if (confirmed)
                sceneTransition.TransitionToScene(1);

            menuManager.confirmationPrompt.SetActive(false); //Disable confirmation prompt
        }

        /// <summary>
        /// Takes the user to the options menu
        /// </summary>
        public void OptionsMenu()
        {
            menuManager.mainMenuUI.SetActive(false);    //Disable main menu
            menuManager.optionsMenuUI.SetActive(true); //Enable options menu
        }

        /// <summary>
        /// Take useer to the credits scene
        /// </summary>
        public void GoToCredits()
        {
            sceneTransition.TransitionToScene(2); //Transitions to the credits scene
        }
    }
}