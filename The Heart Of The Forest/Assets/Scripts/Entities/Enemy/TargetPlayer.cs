/*
 * Date Created: 28.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.Entities
{
    /// <summary>
    /// Used by entities to target the player and rotate towards them
    /// </summary>
    public class TargetPlayer : MonoBehaviour
    {
        /* Variables */

        [Header("Targeting Parameters")]
        [Tooltip("The target.")]
        [SerializeField] private Transform target = null;

        [Tooltip("The max range the entity can see.")]
        [SerializeField, Range(0F, 100F)] private float perceptionRadius = 0F;

        [Header("Rotation Parameters")]
        [Tooltip("The gameobject which will rotate (including its children).")]
        [SerializeField] private Transform rotatingComponent = null;
        [Tooltip("How fast the gameobject will rotate.")]
        [SerializeField, Range(0F, 100F)] private float rotationSpeed = 0F;

        private void Update()
        {
            /* Target Validation */
            if (target == null) { return; }

            /* Target Lock */
            rotatingComponent.LookAt(target);
            //Vector3 direction = target.position - rotatingComponent.position; //Find the direction the target is in
            //Quaternion lookRotation = Quaternion.LookRotation(direction.normalized, Vector3.up); //Entity will rotate to face the target
            //Quaternion rotation = Quaternion.Slerp(rotatingComponent.rotation, lookRotation, rotationSpeed * Time.deltaTime); //Smoothly rotate 
            //rotatingComponent.rotation = rotation; //Apply the rotation changes
        }

        /// <summary>
        /// Draws the perception and attack radius' of this entity
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, perceptionRadius);
        }
    }
}