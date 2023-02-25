/*
 * Date Created: 30.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Tools
{
    /// <summary>
    /// Directional 2D trigger events
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class DirectionalEventTrigger : MonoBehaviour
    {
        /// <summary>
        /// Sides of a BoxCollider2D
        /// </summary>
        public enum ColliderSide
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
        }

        /// <summary>
        /// A side of BoxCollider2D collider
        /// </summary>
        [System.Serializable]
        public struct BoxSide
        {
            [Tooltip("Side of the collider")]
            [SerializeField] public ColliderSide side;
            [Tooltip("Side enter event")]
            [SerializeField] public UnityEvent OnEnterEvent;
            [Tooltip("Side exit event")]
            [SerializeField] public UnityEvent OnExitEvent;

            /// <summary>
            /// Possition of the side
            /// </summary>
            [HideInInspector] public Vector2 pos;

            /// <summary>
            /// Default Constructor
            /// </summary>
            /// <param name="colliderSide">Side of the collider</param>
            public BoxSide(ColliderSide colliderSide)
            {
                side = colliderSide;
                OnEnterEvent = new UnityEvent();
                OnExitEvent = new UnityEvent();
                pos = Vector2.zero;
            }

            /// <summary>
            /// Setup BoxSide
            /// </summary>
            /// <param name="collider">Collider that this BoxSide is apart of</param>
            public void Setup(BoxCollider2D collider)
            {
                Vector2 globalPos;
                globalPos.x = collider.transform.position.x;
                globalPos.y = collider.transform.position.y;

                switch (side)
                {
                    case ColliderSide.UP:
                        pos = globalPos + collider.offset;
                        pos.y += collider.size.y / 2;
                        break;
                    case ColliderSide.DOWN:
                        pos = globalPos + collider.offset;
                        pos.y -= collider.size.y / 2;
                        break;
                    case ColliderSide.LEFT:
                        pos = globalPos + collider.offset;
                        pos.x -= collider.size.x / 2;
                        break;
                    case ColliderSide.RIGHT:
                        pos = globalPos + collider.offset;
                        pos.x += collider.size.x / 2;
                        break;
                }
            }
        }
        [Tooltip("Up side of BoxCollider2D")]
        [SerializeField] public BoxSide up = new BoxSide(ColliderSide.UP);
        [Tooltip("Down side of BoxCollider2D")]
        [SerializeField] public BoxSide down = new BoxSide(ColliderSide.DOWN);
        [Tooltip("Left side of BoxCollider2D")]
        [SerializeField] public BoxSide left = new BoxSide(ColliderSide.LEFT);
        [Tooltip("Right side of BoxCollider2D")]
        [SerializeField] public BoxSide right = new BoxSide(ColliderSide.RIGHT);

        /// <summary>
        /// This BoxCollider2D
        /// </summary>
        private BoxCollider2D collider;

        /// <summary>
        /// Side collided with
        /// </summary>
        private BoxSide collidedSide;

        /// <summary>
        /// Object with player tag
        /// </summary>
        private GameObject player = null;

        /// <summary>
        /// Currently colliding player
        /// </summary>
        [HideInInspector] public GameObject collidedPlayer { get { return player; } }

        /// <summary>
        /// Setup BoxSides
        /// </summary>
        public void Setup()
        {
            collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;

            up.Setup(collider);
            down.Setup(collider);
            left.Setup(collider);
            right.Setup(collider);
        }

        private void Start()
        {
            
        }

        /// <summary>
        /// Find the closest side to a possition
        /// </summary>
        /// <param name="pos">Possition to test</param>
        /// <returns></returns>
        private BoxSide ClosestSide(Vector2 pos)
        {
            float dist = 0;
            float minDist = float.MaxValue;
            BoxSide closestSide = new BoxSide();
            BoxSide boxSide = up;

            // Check each boxSide
            for (int idx = 0; idx < 4; idx++)
            {
                switch (idx)
                {
                    case 0:
                        boxSide = up;
                        break;
                    case 1:
                        boxSide = down;
                        break;
                    case 2:
                        boxSide = left;
                        break;
                    case 3:
                        boxSide = right;
                        break;
                }

                // Distance to a side
                dist = Vector2.Distance(boxSide.pos, pos);
                
                // Find the closest side
                if (dist < minDist)
                {
                    minDist = dist;
                    closestSide = boxSide;
                }
            }

            return closestSide;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                player = collision.gameObject;

                // Invoke event of the closest side
                collidedSide = ClosestSide(collision.transform.position);
                collidedSide.OnEnterEvent.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                // Invoke event of the closest side
                collidedSide = ClosestSide(collision.transform.position);
                collidedSide.OnExitEvent.Invoke();

                player = null;
            }
        }
    }

}