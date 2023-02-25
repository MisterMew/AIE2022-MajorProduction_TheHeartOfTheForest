/*
 * Date Created: 21.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using HotF.GUI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace HotF
{
    /// <summary>
    /// Sets the mouse cursors appearance and manages cursor lock states during various states of gameplay
    /// </summary>
    public class MouseCursorManager : MonoBehaviour
    {

        /* Variables */
        public Texture2D cursorNormal;
        public Texture2D cursorHover;

        private InputActions inputAction;
        private PauseGameManager pauseGameManager;
        private MenuManager menuManager;

        private void Awake() => ChangeCursor(cursorNormal);

        /// <summary>
        /// On Start
        /// </summary>
        private void Start()
        {
            pauseGameManager = FindObjectOfType<PauseGameManager>(); //Get pause game manager
            menuManager = FindObjectOfType<MenuManager>(); //Get Menu manager

            inputAction = new InputActions();

            // Subscribe to click events
            inputAction.UI.Click.started += ctx => ClickStarted();
            inputAction.UI.Click.performed += ctx => ClickEnded();
        }

        private void Update() => HandleLockstate();

        private void OnEnable()
        {
            // Null check (added by Nghia)
            if (inputAction == null) inputAction = new InputActions();

            inputAction.Enable();
        }

        private void OnDisable() => inputAction.Disable();

        /// <summary>
        /// Changes the cursors texture
        /// </summary>
        /// <param name="cursorTex"></param>
        private void ChangeCursor(Texture2D cursorTex) => Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);

        private void ClickStarted() => ChangeCursor(cursorHover);
        private void ClickEnded() => ChangeCursor(cursorNormal);

        /// <summary>
        /// Set the cursors lock state
        /// </summary>
        /// <param name="lockMode">Optional: set a lockstate </param>
        public void HandleLockstate()
        {
            if (pauseGameManager.gamePaused || SceneManager.GetActiveScene().buildIndex == 0 || menuManager.gameCompletionPrompt.activeSelf) //Game is paused OR main menu is active
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true; //Cursor visible
                return;
            }
            else //!gamePaused
            {
                #if UNITY_EDITOR
                return;
                #endif

                Cursor.visible = false; //Cursor invisible

                if (!CheckMouseMovement())                     //If mouse is not moving
                    Cursor.lockState = CursorLockMode.Locked; //Lock

                if (CheckMouseMovement())                    //If mouse is moving
                    Cursor.lockState = CursorLockMode.None; //Unlock
            }
        }
      
        /// <summary>
        /// Checks if the mouse has movement from locked pos
        /// </summary>
        /// <returns></returns>
        public bool CheckMouseMovement()
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            if (mouseDelta.x != 0 || mouseDelta.y != 0)
                return true;

            return false;
        }
    }
}