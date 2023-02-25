/*
 * Date Created: 24.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: Nghia
 */

/*
 * CHANGE LOG:
 * Nghia: added loading of brightness/gamma from saved data
 */


using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HotF.Graphics
{
    /// <summary>
    /// Handles the games brightness and gamma graphics options and initialisation
    /// </summary>
    public class GameBrightness : MonoBehaviour
    {
        /* Variables */
        private GraphicsManager gm;

        [Header("Brightness Values")]
        [SerializeField, Range(-10F, 0F)] private float minBrightnessValue = -3F;
        [SerializeField, Range(-0F, 10F)] private float maxBrightnessValue = 3F;

        [Header("Gamma Values")]
        [SerializeField, Range(-10F, 0F)] private float minGammaValue = -3F;
        [SerializeField, Range(-0F, 10F)] private float maxGammaValue = 3F;

        /// <summary>
        /// Initialisation for the graphics quality
        /// </summary>
        public void Init()
        {
            // Get
            gm = GetComponent<GraphicsManager>();

            // Load data
            LoadBrightness();

            // Initialisation
            InitBrightness();
            InitGamma();

            // Listeners
            gm.brightnessSlider.onValueChanged.AddListener(delegate { SetBrightness(gm.brightnessSlider.value); });
            gm.brightnessSlider.onValueChanged.AddListener(delegate { gm.settingsData.brightness = gm.brightnessSlider.value; });
            gm.gammaSlider.onValueChanged.AddListener(delegate { SetGamma(gm.gammaSlider.value); });
            gm.gammaSlider.onValueChanged.AddListener(delegate { gm.settingsData.gamma = gm.gammaSlider.value; });
        }

        /// <summary>
        /// Load brightness and gamma from saved settings (added by Nghia)
        /// </summary>
        private void LoadBrightness()
        {
            gm.brightnessSlider.SetValueWithoutNotify(gm.settingsData.brightness);
            gm.gammaSlider.SetValueWithoutNotify(gm.settingsData.gamma);
        }

        /// <summary>
        /// Initialises and sets the brightness values 
        /// </summary>
        private void InitBrightness()
        {
            gm.brightnessSlider.minValue = minBrightnessValue;  //Set brightnessSlider min value
            gm.brightnessSlider.maxValue = maxBrightnessValue; //Set brightnessSlider max value

            SetBrightness(gm.brightnessSlider.value); //Set brightness
        }

        /// <summary>
        /// Initialises and sets the gamma values 
        /// </summary>
        private void InitGamma()
        {
            gm.gammaSlider.minValue = minGammaValue;  //Set gammaSlider min value
            gm.gammaSlider.maxValue = maxGammaValue; //Set gammaSlider max value

            SetGamma(gm.gammaSlider.value); //Set gamma
        }

        /// <summary>
        /// Sets the volume setting for brightness
        /// </summary>
        /// <param name="currentValue">Current value of UI slider</param>
        private void SetBrightness(float currentValue)
        {
            if (gm.graphicsVolume.profile.TryGet(out ColorAdjustments colourAdjust)) //Access colour adjustments volume
                colourAdjust.postExposure.Override(currentValue); //Override postExposure value (the one for brightness)
        }

        /// <summary>
        /// Sets the volume setting for gamma
        /// </summary>
        /// <param name="currentValue">Current value of UI slider</param>
        private void SetGamma(float currentValue)
        {
            if (gm.graphicsVolume.profile.TryGet(out LiftGammaGain liftGammaGain)) //Access lift gamma gain volume
            {
                Vector4 gamma = liftGammaGain.gamma.value;
                liftGammaGain.gamma.Override(new Vector4(gamma.x, gamma.x, gamma.x, currentValue)); //Override gamma alpha channel
            }
        }
    }
}