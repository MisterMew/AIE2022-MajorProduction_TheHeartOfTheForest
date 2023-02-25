/*
 * Date Created: 17.10.2022
 * Author: Nghia Do
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace HotF.Interactable
{
    /// <summary>
    /// Map Toggle System (old Map system)
    /// </summary>
    public class MapInteractable : MonoBehaviour
    {
        [Header("Map")]
        [Tooltip("Map action reference")]
        [SerializeField] private InputActionReference mapToggleInput;
        [Tooltip("Event on map turned on")]
        [SerializeField] public UnityEvent OnMapToggledOn;
        [Tooltip("Event on map turned off")]
        [SerializeField] public UnityEvent OnMapToggledOff;

        /// <summary>
        /// Map Action
        /// </summary>
        private InputAction mapAction;

        /// <summary>
        /// Map state
        /// </summary>
        bool mapIsOn = false;

        private void Start()
        {
            
        }

        public void Setup()
        {
            mapAction = FindObjectOfType<PlayerInput>().actions.FindAction(mapToggleInput.action.id);
            OnMapToggledOn.AddListener(delegate { mapIsOn = true; });
            OnMapToggledOff.AddListener(delegate { mapIsOn = false; });
        }

        /// <summary>
        /// Toggle map on/off
        /// </summary>
        /// <param name="_"></param>
        private void ToggleMap(InputAction.CallbackContext _)
        {
            if (!mapIsOn) OnMapToggledOn.Invoke();
            else OnMapToggledOff.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // When entered, allow player to toggle map
            if (collision.tag == "Player")
            {
                mapAction.started += ToggleMap;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            // When exited, no longer allow player to toggle map
            if (collision.tag == "Player")
            {
                mapAction.started -= ToggleMap;
                OnMapToggledOff.Invoke();
            }
        }
    }
}