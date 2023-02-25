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
using System;
using System.Collections.Generic;

namespace HotF.Graphics
{
    /// <summary>
    /// Handles the games screen resolution options, initialisation, and dropdown population
    /// </summary>
    public class ScreenResolution : MonoBehaviour
    {
        /* Variables */
        private GraphicsManager gm;
        private ScreenMode screenMode;

        private List<Resolution> cachedResolutions = new List<Resolution>();
        private int resMinWidth = 600;
        private int resMinHeight = 600;


        /// <summary>
        /// Initialisation for the screen resolutions 
        /// </summary>
        public void Init()
        {
            // Get
            gm = GetComponent<GraphicsManager>();
            screenMode = GetComponent<ScreenMode>();

            // 
            Resolution[] resolutions = Screen.resolutions; //Get ALL available screen resolutions for the users monitor
            Array.Reverse(resolutions); //Reverse arrangment
            cachedResolutions = new List<Resolution>(resolutions.Length); //Same size array

            // Initialisation
            PopulateResolutionsDropdown(resolutions);
            //if (!LoadResolution()) InitResolution(cachedResolutions);
            LoadResolution();

            // Listeners
            gm.resolutionsDropdown.onValueChanged.AddListener(delegate { Screen.SetResolution(cachedResolutions[gm.resolutionsDropdown.value].width, cachedResolutions[gm.resolutionsDropdown.value].height, screenMode.screenMode); });
            gm.resolutionsDropdown.onValueChanged.AddListener(delegate { gm.settingsData.screenWidth = cachedResolutions[gm.resolutionsDropdown.value].width; }); // added by Nghia
            gm.resolutionsDropdown.onValueChanged.AddListener(delegate { gm.settingsData.screenHeight = cachedResolutions[gm.resolutionsDropdown.value].height; }); // added by Nghia
        }

        /// <summary>
        /// Load resolution from settings data (added by Nghia)
        /// </summary>
        /// <returns>Was load sucessful</returns>
        private bool LoadResolution()
        {
            // Find saved resolution
            for (int i = 0; i < cachedResolutions.Count; i++)
            {
                // Find saved resolution within resolutions list
                if (gm.settingsData.screenWidth == cachedResolutions[i].width && gm.settingsData.screenHeight == cachedResolutions[i].height)
                {
                    Debug.Log($"Setting resolution to {gm.settingsData.screenWidth}x{gm.settingsData.screenHeight}");
					Screen.SetResolution(gm.settingsData.screenWidth, gm.settingsData.screenHeight, screenMode.screenMode);
                    gm.resolutionsDropdown.value = i; //Set the current value
                    gm.resolutionsDropdown.RefreshShownValue(); //Refresh the current option displayed

                    return true;
                }
            }

            // Works exactly 50% of the time
            //if (Screen.width < 500 || Screen.height < 500)
            //Screen.SetResolution(500, 500, FullScreenMode.Windowed);

            // If no match was found then set to default
            Debug.Log($"Setting resolution to default of {gm.settingsData.screenWidth}x{gm.settingsData.screenHeight}");
            Screen.SetResolution(gm.settingsData.screenWidth, gm.settingsData.screenHeight, screenMode.screenMode);

            return false;
        }

        /// <summary>
        /// Finds resolutions compatible with the users current monitor
        /// </summary>
        /// <param name="resolution"></param>
        private void PopulateResolutionsDropdown(Resolution[] resolution)
        {
            int count = 0;

            Resolution previousResolution = new Resolution();
            //Display resolutions at current screen refresh rate
            for (int i = 0; i < resolution.Length; i++) //For all current resolutions
            {
                if (previousResolution.width != resolution[i].width &&
                    previousResolution.height != resolution[i].height) //If it's not the same resolution as previously cached
                {
                    previousResolution = resolution[i];
                    cachedResolutions.Add(resolution[i]); //Add to resolutions list
                    count++;
                }
            }

            //Add dropdown to each drop down (upon change)
            for (int i = 0; i < count; i++)
                gm.resolutionsDropdown.options.Add(new TMP_Dropdown.OptionData(ResolutionToString(cachedResolutions[i])));
        }


        /// <summary>
        /// Determines the current screen resolution and automatically sets it
        /// </summary>
        /// <param name="res"></param>
        private void InitResolution(Resolution[] res)
        {
            for (int i = 0; i < res.Length; i++) //Loop through all available resolutions
            {
                if (Screen.width == res[i].width && Screen.height == res[i].height)//Compare the screen res with the current res
                {
                    Screen.SetResolution(res[i].width, res[i].height, screenMode.screenMode);
                    gm.resolutionsDropdown.value = i; //Set the current value
                    gm.resolutionsDropdown.RefreshShownValue(); //Refresh the current option displayed
                    return;
                }
            }

            // Set default to smalles resolution
            int defaultIdx = 0;
            // Screen.SetResolution(res[defaultIdx].width, res[defaultIdx].height, screenMode.screenMode);
            gm.resolutionsDropdown.value = defaultIdx;
            gm.resolutionsDropdown.RefreshShownValue();
        }

        /// <summary>
        /// Converts the given screen resolution into a readable string format
        /// </summary>
        /// <param name="screenRes"></param>
        private string ResolutionToString(Resolution screenRes) => screenRes.width + " x " + screenRes.height;
    }
}
