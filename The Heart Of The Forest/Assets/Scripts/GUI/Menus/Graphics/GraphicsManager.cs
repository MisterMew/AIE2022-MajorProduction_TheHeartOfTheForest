/*
 * Date Created: 08.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

namespace HotF.Graphics
{
    /// <summary>
    /// Stores and calls the initialisation for all graphics options components
    /// </summary>
    public class GraphicsManager : MonoBehaviour
    {
        /* Variables */
        [Header("Data")]
        [Tooltip("Graphics Data")]
        public SettingsData settingsData;
        public Volume graphicsVolume;

        [Header("Content")]
        public Slider brightnessSlider = null;
        public Slider gammaSlider = null;
        public TMP_Dropdown resolutionsDropdown = null;
        public TMP_Dropdown screenmodeDropdown = null;
        public TMP_Dropdown qualityDropdown = null;
        public TMP_Dropdown antiAliasingDropdown = null;
        public Toggle vSyncToggle = null;

        /// <summary>
        /// Initialises the setup for all relevant graphics components
        /// </summary>
        public void Setup()
        {
            //Screen Mode
            ScreenMode screenMode = GetComponent<ScreenMode>();
            screenMode.Init();

            //Screen Resolution
            ScreenResolution screenResolution = GetComponent<ScreenResolution>();
            screenResolution.Init();

            //Quality Levels
            GraphicsQuality graphicsQuality = GetComponent<GraphicsQuality>();
            graphicsQuality.Init();

            //Anti Aliasing
            AntiAliasing antiAliasing = GetComponent<AntiAliasing>();
            antiAliasing.Init();

            //Brightness and Gamma
            GameBrightness gameBrightness = GetComponent<GameBrightness>();
            gameBrightness.Init();

            //Vertical Sync
            VerticalSync verticalSync = GetComponent<VerticalSync>();
            verticalSync.Init();
        }
    }
}