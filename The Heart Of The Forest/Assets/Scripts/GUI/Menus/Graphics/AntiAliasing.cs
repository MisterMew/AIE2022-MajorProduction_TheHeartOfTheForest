/*
 * Date Created: 12.10.2022
 * Author: Jazmin Fazzolari
 * Contributors: Nghia
 */

/*
 * CHANGE LOG:
 * Nghia: added loading of settings data
 */

using UnityEngine;

namespace HotF.Graphics
{
    /// <summary>
    /// Handles the games Anti-Aliasing quality options and initialisation
    /// </summary>
    public class AntiAliasing : MonoBehaviour
    {
        /* Variables */
        private GraphicsManager gm;
        private Camera cam;

        /// <summary>
        /// Initialisation for the graphics AA
        /// </summary>
        public void Init()
        {
            /* Get */
            gm = GetComponent<GraphicsManager>();
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            LoadAntiAliasing();

            /* Listeners */
            gm.antiAliasingDropdown.onValueChanged.AddListener(delegate { AntiAlias(gm.antiAliasingDropdown.value); }); //Add listener to dropdown
            gm.antiAliasingDropdown.onValueChanged.AddListener(delegate { gm.settingsData.antiAliasing = gm.antiAliasingDropdown.value; }); // added by Nghia
        }

        /// <summary>
        /// Load anti aliasing from settings data (added by Nghia)
        /// </summary>
        private void LoadAntiAliasing()
        {
            AntiAlias(gm.settingsData.antiAliasing); //Set the AA quality
            gm.antiAliasingDropdown.SetValueWithoutNotify(gm.settingsData.antiAliasing); //Updated the AA dropdown
        }

        /// <summary>
        /// Handles antialiasing settings
        /// </summary>
        /// <param name="index"></param>
        private void AntiAlias(int index)
        {
            cam.allowMSAA = true; //Allow MSAA

            if (index == 1)
            {
                cam.allowMSAA = false; //Disable MSAA
                QualitySettings.antiAliasing = 0; //If none or FXAA, turn off MSAA
            }
            else if (index == 2)
                QualitySettings.antiAliasing = 2;

            else if (index == 3)
                QualitySettings.antiAliasing = 4;

            else if (index == 4)
                QualitySettings.antiAliasing = 8;

            else
                QualitySettings.antiAliasing = 0; //If none or FXAA, turn off MSAA

            gm.antiAliasingDropdown.SetValueWithoutNotify(index); //Update dropdown
        }
    }
}