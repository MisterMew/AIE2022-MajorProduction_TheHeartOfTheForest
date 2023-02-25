/*
 * Date Created: 24.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

/*
 * CHANGE LOG:
 * Nghia: added loading of settings data
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HotF.Graphics
{
    /// <summary>
    /// Handles the games graphical quality options and initialisation
    /// </summary>
    public class GraphicsQuality : MonoBehaviour
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

            // Initialise
            PopulateQualityDropdown();
            LoadGraphicsQuality(); // added by Nghia

            // Listeners
            gm.qualityDropdown.onValueChanged.AddListener(delegate { SetQualityDropdown(gm.qualityDropdown.value); });
            gm.qualityDropdown.onValueChanged.AddListener(delegate { gm.settingsData.graphicsQuality = gm.qualityDropdown.value; }); // added by Nghia

        }

        /// <summary>
        /// Load graphics from settings data (added by Nghia)
        /// </summary>
        private void LoadGraphicsQuality()
        {
            SetQualityDropdown(gm.settingsData.graphicsQuality);
            gm.qualityDropdown.SetValueWithoutNotify(gm.settingsData.graphicsQuality);
        }

        /// <summary>
        /// Set the current quality level
        /// </summary>
        /// <param name="index"></param>
        private void SetQualityDropdown(int index)
        {
            QualitySettings.SetQualityLevel(index, false);
        }

        /// <summary>
        /// Dynamically populates the quality dropdown to the current URP quality level settings
        /// </summary>
        private void PopulateQualityDropdown()
        {
            List<string> qualityLevels = new List<string>(QualitySettings.names); //Convert quality settings array into list

            foreach (var level in qualityLevels)
                gm.qualityDropdown.options.Add(new TMP_Dropdown.OptionData() { text = level }); //Add each name to dropdown
        }
    }
}