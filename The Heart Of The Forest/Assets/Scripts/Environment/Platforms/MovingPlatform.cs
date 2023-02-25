/*
 * Date Created: 24.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HotF.Environment.Platform
{
    /// <summary>
    /// Handles the transit system for moving platforms.
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        /* Variables */
        [Header("PLATFORM")]
        [Tooltip("This object MUST be in the scene, NOT a prefab.")]
        [SerializeField] private Transform platform = null;


        [Header("WAYPOINTS")]
        [Tooltip("List of transforms that determine where the platform will move between.")]
        [SerializeField] private List<Transform> platformWaypoints = null;
        private int targetWaypoint = 0;


        [Header("CONFIG")]
        [Tooltip("Enable to allow movement automatically.")]
        [SerializeField] private bool reverseMovement = false;
        [Tooltip("The travel speed of the platform.")]
        [SerializeField, Range(0F, 100F)] private float platformSpeed = 0F;

        [Header("Platform Stopping")]
        [SerializeField] private bool delayAtDestination = false;
        [SerializeField, Range(1F, 60F)] private float delayDuration = 0F;

        [Header("Player Interaction")]
        [Tooltip("if TRUE: Platform will only move while the player is touching it")]
        [SerializeField] private bool playerActivated = false;
        [SerializeField] private bool lockUntilActivated = false;
        [Tooltip("The waypoint that a platform will ALWAYS return to (element number from 'Platform Waypoints' list)")]
        [SerializeField] private int homeWaypoint = 0;

        private bool movePlatform = true;
        private bool atDestination = false;
        private bool playerMissing = true;

        /// <summary>
        /// Called once on before first Update frame
        /// </summary>
        private void Start()
        {
            /* Set Starting Position */
            if (!lockUntilActivated)
                platform.transform.position = platformWaypoints[homeWaypoint].position;

            /* Check if player Activated */
            if (playerActivated)
                movePlatform = false;
        }

        /// <summary>
        /// Runs once every fixed update frame
        /// </summary>
        private void FixedUpdate() => MovePlatform(movePlatform); //Moves the platform

        /// <summary>
        /// Called once every frame
        /// </summary>
        private void Update()
        {
            /* Validate Destination */
            if (AtDestination(targetWaypoint))
                atDestination = true;
            else
                atDestination = false;

            /* Validate Player Activation */
            if (playerActivated && AtDestination(homeWaypoint) && playerMissing)
                movePlatform = false;
        }

        /// <summary>
        /// If a collisison is detected
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return; //Return if NOT player

            playerMissing = false;

            if (playerActivated && AtDestination(homeWaypoint)) //If at homePoint
                ValidatePlatformPosition();                    //Validate next destination

            movePlatform = true;
        }

        /// <summary>
        /// If a collision is no longer detected
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return; //Return if NOT player

            playerMissing = true;
        }

        /// <summary>
        /// Validate if the platform is at the home waypoint
        /// </summary>
        /// <returns></returns>
        private bool AtDestination(int waypoint)
        {
            if (platform.transform.position == platformWaypoints[waypoint].position) 
                return true;
            else 
                return false;
        }

        /// <summary>
        /// Moves the platform to the next waypoint
        /// </summary>
        /// <param name="move">False to prevent movement</param>
        private void MovePlatform(bool move = true)
        {
            if (move == false) return;

            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platformWaypoints[targetWaypoint].position, platformSpeed * Time.deltaTime); //Move the platform

            if (atDestination) //If at destination
                ValidatePlatformPosition(); //Validate next move
        }

        /// <summary>
        /// Handles the platforms waypoint to waypoint movement
        /// </summary>
        private async void ValidatePlatformPosition()
        {
            movePlatform = false;

            /* Delay Destination */
            if (delayAtDestination) //If delay is enabled
                await Task.Delay((int)(delayDuration * 1000)); //Delay task

            SetNextDestination(); //Sets the platforms next destination
        }

        /// <summary>
        /// Determine which way the platforms will transition
        /// </summary>
        private void SetNextDestination()
        {
            if (atDestination)
            {
                if (reverseMovement)
                    /* Reversed Movement */
                    targetWaypoint = (targetWaypoint == 0) ? platformWaypoints.Count : (targetWaypoint - 1);
                else
                    /* Normal Movement */
                    targetWaypoint = targetWaypoint == (platformWaypoints.Count - 1) ? 0 : (targetWaypoint + 1);

                movePlatform = true;
                atDestination = false;
            }
        }
    }
}