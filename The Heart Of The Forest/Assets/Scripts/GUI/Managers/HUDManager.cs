/*
 * Date Created: 22.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using HotF.Abilities;
using HotF.GUI;
using HotF.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HotF.Hud
{
    /// <summary>
    /// Handles the in-game heads-up display and other like systems
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        /* Variables */
        [Header("Messages")]
        [SerializeField] private Image messagePanelBg = null;
        [SerializeField] private TMP_Text messagePanelText = null;

        [Header("Health/Lives")]
        [SerializeField] private Transform healthPosition = null;
        [SerializeField] private GameObject lifePrefab = null;
        [Tooltip("The player")]
        [SerializeField] private PlayerHealth playerHealth;
        List<HealthLife> lives = new List<HealthLife>();

        [Header("Glow Charge")]
        public GlowAbility glowAbility;
        public Slider chargeSlider = null;

        [Header("Glyphs")]
        [SerializeField] private bool overrideGlyphStyle = false;
        [SerializeField] private GlyphInfo.FormattingOptions glyphFormatting;

        /// <summary>
        /// Current text, before applying glyph formatting
        /// </summary>
        private string currentText = string.Empty;

        private void Awake() => chargeSlider.value = CalculateGlowValue(); //Set the glow charge value

        private void Start()
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            DrawLives();
        }

        /// <summary>
        /// Update called once every frame
        /// </summary>
        private void Update() => chargeSlider.value = CalculateGlowValue(); //Set the slider to the normalised glow charge (percentage)

        /// <summary>
        /// OnEnable, perform subscriptions
        /// </summary>
        private void OnEnable()
        {
            PlayerHealth.OnLivesChanged += DrawLives;
            ControlGlyphSelector.GlyphShouldChange += OnGlyphShouldChange;
        }

        /// <summary>
        /// On Disable, cancel subscriptions
        /// </summary>
        private void OnDisable()
        {
            PlayerHealth.OnLivesChanged -= DrawLives;
			ControlGlyphSelector.GlyphShouldChange -= OnGlyphShouldChange;
		}

		#region MESSAGES
		/// <summary>
		/// Open the message panel with a custom message.
		/// </summary>
		/// <param name="message">Custom message to display in the message pop up/./param>
		public void OpenMessagePanel(string message)
        {
            messagePanelBg.gameObject.SetActive(true);
            messagePanelText.text = ControlGlyphSelector.ReplaceText(currentText = message, glyphFormatting, !overrideGlyphStyle);
        }

        /// <summary>
        /// Closes the message panel.
        /// </summary>
        public void CloseMessagePanel() => messagePanelBg.gameObject.SetActive(false);

		private void OnGlyphShouldChange()
		{
            if (messagePanelText.gameObject.activeInHierarchy)
                messagePanelText.text = ControlGlyphSelector.ReplaceText(currentText, glyphFormatting, !overrideGlyphStyle);
		}
		#endregion

		#region HEALTH/LIVES
		/// <summary>
		/// When instantiating a life, it ionstantiates as empty
		/// </summary>
		public void CreateEmptyLife()
        {
            GameObject newLife = Instantiate(lifePrefab); //Instantiate the prefab
            newLife.transform.SetParent(healthPosition); //Set the parent transform

            HealthLife lifeComponent = newLife.GetComponent<HealthLife>(); //Get the life component
            lifeComponent.SetLifeImage(LifeStatus.EMPTY); //Set the component to empty
            lives.Add(lifeComponent); //Add it to the list
        }

        /// <summary>
        /// Clear the hud lives
        /// </summary>
        public void ClearLives()
        {
            foreach (Transform t in healthPosition)
                Destroy(t.gameObject); //Destroy the life

            lives = new List<HealthLife>(); //Set the list
        }

        /// <summary>
        /// Draws the health lives
        /// </summary>
        private void DrawLives()
        {
            ClearLives(); //Clear the lives

            int livesToDraw = playerHealth.MaxLives;
            for (int i = 0; i < livesToDraw; i++)
                CreateEmptyLife();

            int currentLives = playerHealth.CurrentLives;
            for (int i = 0; i < lives.Count; i++)
            {
                if (i < currentLives)
                    lives[i].SetLifeImage(LifeStatus.FULL);

                if (i == currentLives) 
                    lives[i].SetLifeImage(LifeStatus.EMPTY);
            }
        }
        #endregion

        private float CalculateGlowValue() => 1 - (glowAbility.GlowTime / glowAbility.MaxGlowTime);

#if UNITY_EDITOR
        [CustomEditor(typeof(HUDManager))]
        private class HUDManagerEditor : Editor
        {
            private HUDManager _target;

            private void OnEnable() => _target = (HUDManager)target;

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (_target.overrideGlyphStyle &&
                    Application.isPlaying &&
                    GUILayout.Button("Refresh Glyph"))
                    _target.OnGlyphShouldChange();
            }
        }
#endif
    }
}