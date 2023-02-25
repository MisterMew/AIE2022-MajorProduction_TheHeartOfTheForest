/*
 * Date Created: 16.10.2022
 * Author: Nghia
 * Contributors: Jazmin
 */

/*
 * CHANGE LOG:
 * Jazmin: 
 * - Added OnEnable() and OnDisable() subscriptions
 * - Added SelectInput() method
 * - Added "InputActionReference guiInputAction" variable
 * - Added "InputAction guiAction" variable
 */

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace HotF.GUI
{
    /// <summary>
    /// Select a UI
    /// </summary>
    public class SelectUI : MonoBehaviour
    {
        /// <summary>
        /// A selectable UI type
        /// </summary>
        public enum SelectableUiType
        {
            BUTTON,
            SLIDER
        }

        /* Variables */
        [Tooltip("Select UI by default")]
        [SerializeField] public bool isDefaultSelection = true;
        [Tooltip("UI type")]
        [SerializeField] private SelectableUiType uiType = SelectableUiType.BUTTON;

        [SerializeField] private InputActionReference guiInputAction = null; //Added by Jazz
        [SerializeField] private InputAction guiAction = null; //Added by Jazz

        private void OnAwake() =>
            guiAction = GetComponent<PlayerInput>().actions.FindAction(guiInputAction.action.id); //Find the specified input action reference

        /// <summary>
        /// Callback function for input detection subscription
        /// </summary>
        /// <param name="obj"></param>
        private void SelectInput(InputAction.CallbackContext obj) => Select(); //Added by Jazz

        private void OnEnable() //Added by Jazz
        {
            ControlGlyphSelector.GlyphShouldChange += Select;
            guiAction.started += SelectInput;
        }
        private void OnDisable() //Added by Jazz
        {
            ControlGlyphSelector.GlyphShouldChange -= Select;
            guiAction.started -= SelectInput;
        }

        /// <summary>
        /// Select UI based on type
        /// </summary>
        public void Select()
        {
            if (!ControlGlyphSelector.CurrentSchemeIsGamepad) return;  //Added by Jazz - Validate check for gamepad 

            switch (uiType)
            {
                case SelectableUiType.BUTTON:
                    GetComponent<Button>().Select();
                    break;

                case SelectableUiType.SLIDER:
                    GetComponent<Slider>().Select();
                    break;
            }
        }
    }
}