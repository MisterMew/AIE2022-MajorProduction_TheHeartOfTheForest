/*
 * Date Created: 05/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using HotF.AI;
using HotF.Enemy.CorruptedMushroom;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Enemy.SporeEnemy
{
    public class Spore_Attack : Attack
    {
        private AttackArea m_attackArea;

        private bool m_inAttackRadius;
        private bool m_isAttacking;

        [Header("Settings")]
        [Tooltip("The radius of the attack trigger")]
        [SerializeField] private float m_attackRadius;
        [Tooltip("The amount of time between each attack")]
        [SerializeField] private float m_timeBetweenAttacks;

        [SerializeField] private GameObject m_cannonProjectile;

        [Tooltip("The spawn transform of the projectile")]
        [SerializeField] private Transform m_spawnPoint;
        [Tooltip("The time it takes for the projectile to reach the player")]
        [SerializeField] private float m_projectileTime = 0.5f;
        [Tooltip("The delay to be applied, so the projectile is fired in line with the animation.")]
        [SerializeField] private float m_shootDelay = 1.0f;

        [Space] 
        [Header("Spore Enemy Events")] 
        [SerializeField] private UnityEvent OnFireProjectile;

        //Getters and Setters
        public float AttackRadius { get { return m_attackRadius; } }

        protected override void Awake()
        {
            base.Awake();

            InstantiateAttackTrigger();
        }

        public override void Enter()
        {
            base.Enter();

            if (!m_isAttacking)
            {
                StartCoroutine(AttackTimer());
            }
        }

        public override void EnterAttackTrigger()
        {
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.ATTACK);
            //m_inAttackRadius = true;
        }

        public override void StayAttackTrigger()
        {
            //Make a layer mask which will be ignored by the raycast
            int layerMask = 1 << ENEMY_LAYER;
            layerMask = ~layerMask;

            //Create a raycast 2D
            Vector2 toPlayer = m_playerTransform.position - m_transform.position;
            RaycastHit2D hit = Physics2D.Raycast(m_transform.position, toPlayer.normalized, Mathf.Infinity, layerMask);

            //If we dont hit a floor object...
            if (!hit.collider.CompareTag("Floor"))
            {
                m_inAttackRadius = true;
            }
            else
            {
                m_inAttackRadius = false;
            }
        }

        public override void ExitAttackTrigger()
        {
            m_inAttackRadius = false;
        }

        IEnumerator SpawnCannonProjectile()
        {
            yield return new WaitForSeconds(m_shootDelay);

            //Invoke event
            OnFireProjectile.Invoke();

            float actualTime;
            if (m_playerTransform.position.y < m_transform.position.y)
                actualTime = m_projectileTime / 2;
            else
                actualTime = m_projectileTime;

            Vector3 velocity = GetCorrectVelocity(m_playerTransform.position, actualTime);

            GameObject spore = Instantiate(m_cannonProjectile);
            Rigidbody2D rb = spore.GetComponent<Rigidbody2D>();
            rb.transform.position = m_spawnPoint.position;
            rb.AddForce(velocity, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Gets the correct velocity for the projectile to hit the player.
        /// The following youtube link helped me greatly when writing this function:
        /// https://www.youtube.com/watch?v=03GHtGyEHas
        /// </summary>
        /// <param name="target">The target for the projectile to hit</param>
        /// <param name="time">The time it takes for the projectile to hit the player</param>
        /// <returns>The calculated velocity depending on the players position</returns>
        Vector3 GetCorrectVelocity(Vector3 target, float time)
        {
            Vector3 distance = target - m_spawnPoint.position;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0;

            float Sy = distance.y;
            float Sxz = distanceXZ.magnitude;

            float Vxz = Sxz / time;
            float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

            Vector3 result = distanceXZ.normalized;
            result *= Vxz;
            result.y = Vy;

            return result;
        }

        void SpawnProjectile()
        {
            //Play the attack animation
            m_animator.CrossFade("Attack", 0.1f, -1, 0);

            StartCoroutine(SpawnCannonProjectile());
        }

        /// <summary>
        /// Recursive enumerator 
        /// </summary>
        IEnumerator AttackTimer()
        {
            //Set elapsed and wait time
            float elapsedTime = 0;
            float waitTime = m_inAttackRadius ? m_timeBetweenAttacks : 0.2f;//m_timeBetweenAttacks;

            m_isAttacking = true;

            if (m_inAttackRadius)
            {
                SpawnProjectile();
            }

            //While elapsed time is less than wait time...
            while (elapsedTime < waitTime)
            {
                //Add delta time to elapsed time
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            m_isAttacking = false;

            //Restart the timer
            if (m_inAttackRadius)
            {
                StartCoroutine(AttackTimer());
            }
            else
            {
                m_stateMachine.ChangeStateByType(NPCStateTypeEnum.IDLE);
            }
        }

        protected override void InstantiateAttackTrigger()
        {
            //Instantiate and add components
            GameObject attackObj = new GameObject("Attack Area");
            attackObj.AddComponent<AttackArea>();
            attackObj.AddComponent<CircleCollider2D>();

            //Cache components
            Transform attackTransform = attackObj.transform;
            m_attackArea = attackTransform.GetComponent<AttackArea>();
            CircleCollider2D col = attackObj.GetComponent<CircleCollider2D>();

            //Set the parent, position and size
            attackTransform.SetParent(m_transform);
            attackTransform.position = m_transform.position;
            col.radius = m_attackRadius;
            //Set collider to a trigger
            col.isTrigger = true;

            //Set the layer
            attackObj.layer = ENEMY_LAYER;
            attackObj.tag = "Untagged";

            //Set attack area variables
            m_attackArea.SetCollider(col);
            m_attackArea.SetAttack(this);
        }
    }
}
