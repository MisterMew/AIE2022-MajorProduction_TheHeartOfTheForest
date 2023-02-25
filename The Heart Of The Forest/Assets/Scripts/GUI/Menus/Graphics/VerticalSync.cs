/*
 * Date Created: 24.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

/*
 * CHANGE LOG:
 * Nghia: added loading of settings data
 */

using UnityEngine;

namespace HotF.Graphics
{
    /// <summary>
    /// Handles the games vertical sync options and initialisation
    /// </summary>
    public class VerticalSync : MonoBehaviour
    {
        /* Variables */
        private GraphicsManager gm;

        /// <summary>
        /// Initialisation for the graphics quality
        /// </summary>
        public void Init()
        {
            // Get
            gm = GetComponent<GraphicsManager>();
            
            LoadVsync();

            // Listeners
            gm.vSyncToggle.onValueChanged.AddListener(delegate { VertSync(gm.vSyncToggle.isOn); } ); //Add listener
            gm.vSyncToggle.onValueChanged.AddListener(delegate { gm.settingsData.vSync = gm.vSyncToggle.isOn; } ); // added by Nghia
        }

        /// <summary>
        /// Load vSync from settings data (added by Nghia)
        /// </summary>
        private void LoadVsync()
        {
            VertSync(gm.settingsData.vSync);
            gm.vSyncToggle.SetIsOnWithoutNotify(gm.settingsData.vSync);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isToggled">the toggle option in the graphics menu</param>
        /// <returns></returns>
        private bool VertSync(bool isToggled)
        {
            if (isToggled)
                QualitySettings.vSyncCount = 1;
            else
                QualitySettings.vSyncCount = 0; //No vsync

            gm.vSyncToggle.SetIsOnWithoutNotify(isToggled);
            //settings.vSyncValue = isToggled;

            return false; //temp
        }
    }
}
