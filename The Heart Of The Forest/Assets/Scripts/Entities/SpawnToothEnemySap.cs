/*
 * Date Created: 05.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.Entities
{
    /// <summary>
    /// Spawns the sap puddles on the first valid surface below a tooth enemy
    /// </summary>
    public class SpawnToothEnemySap : MonoBehaviour
    {
        /* Variables */
        [SerializeField] GameObject sapPuddlePrefab = null;
        [Tooltip("Maximum distance that a puddle can be spawned at.")]
        [SerializeField, Range(0F, 1000F)] private float maxDistance = 0F;

        private RaycastHit2D hit;

        /// <summary>
        /// Called once before the update
        /// </summary>
        void Start()
        {
            if (sapPuddlePrefab != null && FindFlatSurface()) //Validate
                Instantiate(sapPuddlePrefab, new Vector3(hit.point.x, hit.point.y, this.transform.position.z), Quaternion.identity); //Instantiate the puddle
        }

        /// <summary>
        /// Finds the first available surface using a raycast
        /// </summary>
        /// <returns></returns>
        private bool FindFlatSurface() {
            hit = Physics2D.Raycast(transform.position, -Vector3.up, maxDistance);

            if (hit.collider == null) 
                return false;

            return ValidateSurface();
        }

        /// <summary>
        /// Validates what the surface is and whether the sap puddle can spawn
        /// </summary>
        private bool ValidateSurface()
        {
            if (hit.collider.tag != "Floor")
                return false;

            return true;
        }
    }
}