/*
 * Date Created: 27.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotF.Tools
{
    /// <summary>
    /// Follow a target
    /// </summary>
    public class ObjectFollow : MonoBehaviour
    {
        [Tooltip("Target to follow")]
        [SerializeField] Transform target;

        // Start is called before the first frame update
        void Start()
        {
            // If there is no set target then find a player
            if (!target) target = FindObjectOfType<Player.PlayerHealth>().transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (target) FollowExact();
        }

        /// <summary>
        /// Follow the target's position exactly
        /// </summary>
        private void FollowExact() => transform.position = target.position;
    }

}