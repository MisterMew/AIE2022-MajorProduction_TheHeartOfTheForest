/*
 * Date Created: 26.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: Nghia, Nick
 */

/*
 * CHANGE LOG:
 * Nghia: Added audio functionality
 * Nick: Added event "OnBounce"
 */

using HotF.Player;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Environment.Platform
{
    /// <summary>
    /// Handles the functionality for the Bounce-pad platform
    /// </summary>
    public class BouncePad : MonoBehaviour
    {
        /* Variables (public) */
        [Tooltip("AudioSource")]
        [SerializeField] AudioSource audioSource;

        [Header("Bounce Variables")]
        [Tooltip("The strength of the bounce that affects the acceleration.")]
        [SerializeField, Range(0F, 100F)] private float bounceForce;

        [Tooltip("The highest the bounce strength can get before the multiplier no longer works.")]
        [SerializeField, Range(0F, 100F)] private float bounceMaxForce = 0F;

        [Tooltip("Multiplier for how much higher an object can bounce with each additional bounce.")]
        [SerializeField, Range(0F, 100F)] private float bounceForceIncrement = 0F;

        [Tooltip("Multiplies tha normal direction.")]
        [SerializeField, Range(0F, 100F)] private float directionScale = 0F;

        [Tooltip("Bounce particle to spawn when the bounce pad is jumped on")]
        [SerializeField] private GameObject m_particle;

        [Tooltip("When the player bounces on the platform")]
        [SerializeField] private UnityEvent OnBounce;

        /* Variables (private) */
        private GameObject player = null;
        private Rigidbody2D pRb = null;
        private float tempBounceForce;

        private void Awake() => this.tag = "BouncePad";

        private void Start() => ResetBounceForce();

        private void OnEnable() => PlayerMovement.OnCancelBounce += ResetBounceForce;

        private void OnDisable() => PlayerMovement.OnCancelBounce -= ResetBounceForce;

        /// <summary>
        /// When something collides with the bounce pad
        /// </summary>
        /// <param name="contact"></param>
        private void OnCollisionEnter2D(Collision2D contact)
        {
            if (contact.gameObject.CompareTag("Player")) //Collision is player
            {
                player = contact.gameObject;                          //Get the player gameobject
                pRb = player.gameObject.GetComponent<Rigidbody2D>(); //Get the player rigidbody
                OnBounce.Invoke();                                  //Invoke bounce event

                if (m_particle)
                    Instantiate(m_particle, transform.position, Quaternion.identity);

                RaycastHit2D hit = Physics2D.Raycast(player.transform.position, player.transform.position - this.transform.position); //Raycast between player and bounce pad

                if (hit.collider != null)                   //hit collider exists
                    Bounce(transform.up * directionScale); //Set the direction of the bounce

                
                // Play audio on bouce (added by Nghia)
                if (!audioSource) audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);
            }
        }

        /// <summary>
        /// Handles the bounce pad functionality
        /// </summary>
        private void Bounce(Vector2 bounceDirection)
        {
            tempBounceForce += bounceForceIncrement;                            //Multiplier that makes bouncing higher incrementally
            tempBounceForce = Mathf.Clamp(tempBounceForce, 0, bounceMaxForce); //Clamp the bounce force

            pRb.velocity = bounceDirection * (tempBounceForce * 2F); //Add velocity to the player
        }

        /// <summary>
        /// Resets the force of the bounce to its default value
        /// </summary>
        public void ResetBounceForce() => tempBounceForce = bounceForce; //Resets the bounce force
    }
}