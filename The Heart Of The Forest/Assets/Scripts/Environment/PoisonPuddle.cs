/*
 * Date Created: 23.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: Lewis
 */

using HotF.Interactable;
using HotF.Player;
using UnityEngine;
using System.Linq;

namespace HotF
{
    /// <summary>
    /// Handles the functionality for the poison puddles
    /// </summary>
    public class PoisonPuddle : MonoBehaviour
    {
        /* Variables */
        [Tooltip("Damage dealt to player (once inbetween invulnerable period)")]
        [SerializeField, Range(0, 10)] private int damageDealt = 0;
        private PlayerHealth playerHealth;

        [SerializeField]
        private Transform[] potentialHeartFragRespawnLocations;

        /// <summary>
        /// When the player is colliding with a poison puddle
        /// </summary>
        /// <param name="contact"></param>
        private void OnTriggerStay2D(Collider2D contact)
        {
            if (contact.gameObject.tag != "Player") { return; }

            playerHealth = contact.GetComponent<PlayerHealth>();             //Get the player health class
            playerHealth.RemoveLives(damageDealt, PlayerDamageType.Poison); //Remove lives from player

            if (playerHealth.CurrentLives <= 0 && playerHealth.DropHeartFragOnDeath)
                MakePlayerDropHeartFragment(contact.gameObject);
		}

        /// <summary>
        /// Forces the player to drop a heart fragment
        /// </summary>
        /// <param name="player"></param>
        private void MakePlayerDropHeartFragment(GameObject player) //Implemented by Lewis Comstive
        {
			// Drop singular heart fragment
			HeartFragInteractable heartFragment = player.GetComponentInChildren<HeartFragInteractable>(true);
            if (!heartFragment || potentialHeartFragRespawnLocations.Length == 0)
                return;

            // Sort heart fragment spawn locations by distance from player, then get the first/closest one
            Vector3 playerPosition = player.transform.position;
            Transform closest = potentialHeartFragRespawnLocations
                                .OrderBy(x => Vector3.Distance(x.position, playerPosition)).First();
            heartFragment.transform.position = closest.position;

            // Re-activate the heart fragment
			heartFragment.UpdateState(HeartFragmentState.UNCOLLECTED);
		}
	}
}