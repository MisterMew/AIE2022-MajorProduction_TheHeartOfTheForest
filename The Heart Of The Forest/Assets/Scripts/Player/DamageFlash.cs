/*
 * Date Created: 17.10.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using HotF.Player;
using System.Collections;
using UnityEngine;

namespace HotF
{
    /// <summary>
    /// The visual flash applied to the player when any damage is taken
    /// </summary>
    public class DamageFlash : MonoBehaviour
    {
        /* Variables */
        private PlayerHealth playerHealth = default;

        [SerializeField] private SkinnedMeshRenderer skinMeshRendBody = null;
        [SerializeField] private MeshRenderer MeshRendCap = null;
        private Color defaultColor = default(Color);

        [Header("Flash Variables")]
        [SerializeField] private float flashDuration = 0F;
        [SerializeField] private float flashIntensity = 0F;
        [SerializeField] private Color flashColour = default;
        private float flashTimer = 0F;
        private float lerp, intensity = 0F;

        /// <summary>
        /// Called once 
        /// </summary>
        private void Start()
        {
            playerHealth = GetComponent<PlayerHealth>(); //Get the player health
            defaultColor = skinMeshRendBody.material.color;     //Get the players material
            defaultColor = MeshRendCap.material.color;     //Get the players material
        }

        /// <summary>
        /// Enable the flash when player takes damage
        /// </summary>
        private void OnEnable() => PlayerHealth.OnPlayerHurt += DmgFlash; //Subsribe to player being hurt

        /// <summary>
        /// Disable the flash when the player stops taking damage
        /// </summary>
        private void OnDisable() => PlayerHealth.OnPlayerHurt -= DmgFlash; //Unsubsribe from player being hurt

        /// <summary>
        /// Calls the damage flash for the desired duration
        /// </summary>
        private void DmgFlash()
        {
            flashTimer = flashDuration; //Reset timer
            StartCoroutine(Flash());
        }


        /// <summary>
        /// Handles the damage flash functionality
        /// </summary>
        /// <returns></returns>
        private IEnumerator Flash()
        {
            flashTimer -= Time.deltaTime; //Decrease flash timer

            lerp = Mathf.Clamp01(flashTimer / flashDuration);            //Get blending timer
            intensity = (lerp * flashIntensity) + 1F;                   //Calculate intensity
            skinMeshRendBody.material.color = flashColour * intensity; //Set flash
            MeshRendCap.material.color = flashColour * intensity;     //Set flash

            yield return new WaitForSeconds(flashDuration);

            skinMeshRendBody.material.color = defaultColor; //Return to original colour
            MeshRendCap.material.color = defaultColor;     //Return to original colour
        }
    }
}