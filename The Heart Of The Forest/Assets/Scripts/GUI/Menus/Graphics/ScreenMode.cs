/*
 * Date Created: 24.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: Nghia Do
 * 
 * CHANGE LOG:
 * Nghia: added loading of settings data
 */

using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace HotF.Graphics
{
    /// <summary>
    /// Handles the games screen mode options, functionality, and population
    /// </summary>
    public class ScreenMode : MonoBehaviour
    {
        /* Variables */
        private GraphicsManager gm;

        [Header("Screen Settings")]
        [HideInInspector] public FullScreenMode screenMode;
        private List<string> screenModeOptions = new List<string>() { "Fullscreen", "Windowed", "Borderless" };


        /// <summary>
        /// Initialisation for the screen mode
        /// </summary>
        public void Init()
        {
            // Get
            gm = GetComponent<GraphicsManager>();

            // Initialisation
            PopulateScreenmodesDropdown();
            LoadScreenMode();
            InitScreenMode();

            // Listeners
            gm.screenmodeDropdown.onValueChanged.AddListener(delegate { ScreenOptions(gm.screenmodeDropdown.options[gm.screenmodeDropdown.value].text); });
            gm.screenmodeDropdown.onValueChanged.AddListener(delegate { gm.settingsData.screenMode = gm.screenmodeDropdown.options[gm.screenmodeDropdown.value].text; }); // added by Nghia
        }

        /// <summary>
        /// Load screen mode from saved settings (added by Nghia)
        /// </summary>
        private void LoadScreenMode()
        {
            if (gm.settingsData.screenMode == "Fullscreen")
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

            else if (gm.settingsData.screenMode == "Windowed")
                Screen.fullScreenMode = FullScreenMode.Windowed;

            else
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        /// <summary>
        /// Populates the screenmode dropdown with available screen modes
        /// </summary>
        private void PopulateScreenmodesDropdown()
        {
            gm.screenmodeDropdown.options.Clear();

            List<string> options = screenModeOptions;

            foreach (var option in options)
                gm.screenmodeDropdown.options.Add(new TMP_Dropdown.OptionData() { text = option });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        private void ScreenOptions(string mode)
        {
            if (mode == "Fullscreen")
                screenMode = FullScreenMode.ExclusiveFullScreen;

            else if (mode == "Windowed")
                screenMode = FullScreenMode.Windowed;

            else
                screenMode = FullScreenMode.FullScreenWindow;

            gm.settingsData.screenMode = mode; // added by Nghia
            Screen.fullScreenMode = screenMode; //Set the screen mode
        }


        /// <summary>
        /// Initialise the screen mode and automatically set
        /// </summary>
        private void InitScreenMode()
        {
            if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
            {
                gm.screenmodeDropdown.value = 0;
                screenMode = FullScreenMode.ExclusiveFullScreen;
            }

            else if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                gm.screenmodeDropdown.value = 1;
                screenMode = FullScreenMode.Windowed;
            }

            else
            {
                gm.screenmodeDropdown.value = 2;
                screenMode = FullScreenMode.FullScreenWindow;
            }

            gm.screenmodeDropdown.RefreshShownValue(); //refresh the current option display
        }
    }
}