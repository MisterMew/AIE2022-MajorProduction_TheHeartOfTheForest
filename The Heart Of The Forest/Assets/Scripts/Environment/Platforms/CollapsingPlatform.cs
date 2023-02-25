/*
 * Date Created: 24.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: Nghia
 */

/*
 * CHANGE LOG:
 * Nghia: added audio functionality, made meshRend a SerializeField
 */

using UnityEngine;

namespace HotF.Environment.Platform
{
    /// <summary>
    /// Handles the platforms that can collapse underneath the player
    /// </summary>
    public class CollapsingPlatform : MonoBehaviour
    {
        /* Variables */
        [Tooltip("Mesh Renderer")]
        [SerializeField] private MeshRenderer meshRend;
        private CapsuleCollider2D capCol;
        private Rigidbody2D rb2D;

        [Header("Collapse Options")]
        [Tooltip("The time taken before the platform will collapse (in seconds)")]
        [SerializeField, Range(0F, 10F)] private float collapseDelay = 0F;
        [Tooltip("The time taken before the platform will destroy itself (in seconds)")]
        [SerializeField, Range(0F, 10F)] private float destroyDelay = 0F;
        [Tooltip("The time taken before the platform will respawn after being destroyed (in seconds)")]
        [SerializeField, Range(0F, 10F)] private float respawnDelay = 0F;

        private Transform spawnTransform = null;


        /// <summary>
        /// AudioSource
        /// </summary>
        private AudioSource audioSource;

        // Start is called once before the Update method
        private void Start()
        {
            if (!meshRend) meshRend = GetComponent<MeshRenderer>();
            capCol = GetComponent<CapsuleCollider2D>();
            rb2D = GetComponentInChildren<Rigidbody2D>(); //Get the rigidbody component
            audioSource = GetComponent<AudioSource>();


            InitFragilePlatform();
        }

        /// <summary>
        /// Initialise the platform
        /// </summary>
        private void InitFragilePlatform()
        {
            rb2D.isKinematic = true;
            rb2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            spawnTransform = this.transform;
        }

        /// <summary>
        /// When a collision is detected
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == ("Player") && collision.GetContact(0).normal == -Vector2.up)
            {
                Invoke("CollapsePlatform", collapseDelay);
                if (destroyDelay != 0) Invoke("SetPlatformActive(false)", destroyDelay);
                Invoke("RespawnPlatform", respawnDelay);
            }
        }

        /// <summary>
        /// Collapse the platform using gravity
        /// </summary>
        private void CollapsePlatform()
        {
            audioSource?.Play();
            rb2D.isKinematic = false; //Set kinematic to false
        }

        /// <summary>
        /// Sets the paltforms active state
        /// </summary>
        /// <param name="setActive"></param>
        private void SetPlatformActive(bool setActive)
        {
            meshRend.enabled = setActive;
            capCol.enabled = setActive;
        }

        /// <summary>
        /// Handles respawning the platform
        /// </summary>
        private void RespawnPlatform()
        {
            rb2D.isKinematic = true;
            rb2D.velocity = Vector2.zero;
            transform.localPosition = Vector3.zero;
            meshRend.gameObject.transform.localPosition = Vector3.zero; //Set mesh object to zero, couldnt figure out what was going wrong
            SetPlatformActive(true);
        }
    }
}