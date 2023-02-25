/*
 * Date Created: 25/08/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using HotF.Enemy;
using HotF.Enemy.CorruptedMushroom;
using UnityEngine;

namespace HotF.AI
{
    public enum ClimbSurfaceType
    {
        LEFT_IS_HIGHER_THAN_RIGHT, RIGHT_IS_HIGHER_THAN_LEFT
    }

    public class ClimbableSurface : MonoBehaviour
    {
        private BoxCollider2D m_collider;
        private Transform m_transform;

        [SerializeField] private ClimbSurfaceType m_climbType;

        [Tooltip("The left point the enemy will climb to")]
        public Vector3 m_leftPoint;
        [Tooltip("The right point the enemy will climb to")]
        public Vector3 m_rightPoint;

        //Getters and Setters
        public BoxCollider2D Collider { get { return m_collider; } set { m_collider = value; } }
        public Transform Transform { get { return m_transform; } set { m_transform = value; } }
        public ClimbSurfaceType ClimbType { get { return m_climbType; } }

        private void Awake()
        {
            //Cache components
            m_collider = GetComponent<BoxCollider2D>();
            m_transform = GetComponent<Transform>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //If the collided object is an enemy or villager...
            if (col.gameObject.GetComponent<Climb>())
            {
                //Set the climbable surface on the NPC
                col.gameObject.GetComponent<Climb>().SetClimbableSurface(this);
                //Change the state to climb
                col.gameObject.GetComponent<Climb>().EnterClimbTrigger();
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<Climb>())
            {
                other.gameObject.GetComponent<Climb>().ExitClimbTrigger();
            }
        }
    }
}
