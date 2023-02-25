/*
 * Date Created: 27.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotF.Environment.Platform
{
    /// <summary>
    /// Handles the functionality for two-way passable platforms
    /// </summary>
    public class PassablePlatform : MonoBehaviour
    {
        /* Variables */
        private Collider2D col2D = null;
        private bool playerOnPlatform = false;
        [SerializeField]
        private  InputActionReference downInputAction = null;

        // Start is called before the first frame update
        void Start()
        {
            col2D = GetComponent<Collider2D>(); //Get 2D collider
        }


        // Update is called once every frame
        private void Update()
        {
            if (playerOnPlatform && downInputAction.action.triggered) //Player is on platform AND down input triggered
            {
                col2D.enabled = false;
                StartCoroutine(EnableCollider()); //Enablee collider
            }
        }

        /// <summary>
        /// Used to temporarly create one-way platform
        /// </summary>
        /// <returns></returns>
        private IEnumerator EnableCollider()
        {
            yield return new WaitForSeconds(0.5F);
            col2D.enabled = true;
        }

        /// <summary>
        /// Flag method to determine if player is ON the platforms
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="value"></param>
        private void SetPlayerOnPlatform(Collision2D contact, bool value)
        {
            if (contact.gameObject.tag != "Player") //Contact/collision is NOT the player
                return; //Returns

            playerOnPlatform = value; //Set tag
        }

        /// <summary>
        /// When a collision is detected
        /// </summary>
        /// <param name="contact"></param>
        private void OnCollisionEnter2D(Collision2D contact)
        {
            SetPlayerOnPlatform(contact, true); //Set true
        }

        /// <summary>
        /// When a collision is no longer detected
        /// </summary>
        /// <param name="contact"></param>
        private void OnCollisionExit2D(Collision2D contact)
        {
            SetPlayerOnPlatform(contact, false); //Set false
        }
    }
}
