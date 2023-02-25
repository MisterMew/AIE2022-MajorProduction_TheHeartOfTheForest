/*
 * Date Created: 02/09/2022
 * Author: Nicholas Connell
 */

/*
 * CHANGE LOG:
 * Nghia: added OnLandEvent to play audio on land
 */

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using HotF.AI;
using HotF.Enemy.CorruptedMushroom;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Enemy.ToothEnemy
{
    public class Tooth_Attack : Attack
    {
        private AttackArea m_attackArea;

        private float waitTime;
        private float elapsedTime;

        private Vector3 m_tendrilStartPos;

        [Tooltip("The tendril gameobject")]
        [SerializeField] private GameObject m_tendril;

        [Tooltip("The speed the Tooth enemy falls")]
        [SerializeField] private float m_fallSpeed;
        [Tooltip("The total amount of time ")]
        [SerializeField] private float m_downTime;

        [SerializeField] private Vector2 m_attackTriggerSize;
        [SerializeField] private Vector2 m_attackTriggerPosition;

        [SerializeField] private GameObject m_sapPuddle;

        //Getters and Setters
        public Vector2 AttackTriggerSize { get { return m_attackTriggerSize; } }
        public Vector2 AttackTriggerPosition { get { return m_attackTriggerPosition; } set { m_attackTriggerPosition = value; } }
        public GameObject Tendril { get { return m_tendril; } set { m_tendril = value; } }
        public Vector3 TendrilStartPosition { get { return m_tendrilStartPos; } }

        [Tooltip("Event on landing on the floor")]
        [SerializeField] private UnityEvent OnLandEvent;

        protected override void Awake()
        {
            base.Awake();

            InstantiateAttackTrigger();

            SetupTendril();

            //Set the gravity scale
            m_rigidBody.gravityScale = 0f;
        }

        protected override void Start()
        {
            base.Start();

            Vector2 sapPos = new Vector2(AttackTriggerPosition.x, AttackTriggerPosition.y - AttackTriggerSize.y / 2);
            Instantiate(m_sapPuddle, sapPos, Quaternion.identity);
        }

        public override void Enter()
        {
            base.Enter();

            //Play animation
            m_animator.CrossFade("Attack", 0.1f, -1, 0);

            //Set the fall speed
            m_rigidBody.gravityScale = m_fallSpeed;

            //Start the down time coroutine
            StartCoroutine(DownTimer());
        }

        public override void UpdateLogic()
        {
            //Set the tendril start position
            m_tendril.transform.position = m_tendrilStartPos;

            //Set the tendril to the correct scale
            Vector3 tendrilScale = m_tendril.transform.lossyScale;
            tendrilScale.y = Vector3.Distance(m_tendrilStartPos, m_transform.position) / 2;
            m_tendril.transform.localScale = tendrilScale;
        }

        public override void Exit()
        {
            base.Exit();

            m_rigidBody.gravityScale = 0f;
        }

        public override void EnterAttackTrigger()
        {
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.ATTACK);
        }

        IEnumerator DownTimer()
        {
            elapsedTime = 0;
            waitTime = m_downTime;

            //While elapsed time is less than the wait time...
            while (elapsedTime < waitTime)
            {
                //Add delta time to the elapsed time
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            //Change the state to post attack
            m_stateMachine.ChangeStateByType(NPCStateTypeEnum.POST_ATTACK);
        }

        protected override void InstantiateAttackTrigger()
        {
            //Instantiate and add components
            GameObject attackObj = new GameObject("Attack Area");
            attackObj.AddComponent<AttackArea>();
            attackObj.AddComponent<BoxCollider2D>();

            //Cache components
            Transform attackTransform = attackObj.transform;
            m_attackArea = attackTransform.GetComponent<AttackArea>();
            BoxCollider2D col = attackObj.GetComponent<BoxCollider2D>();

            //Set the parent and size
            attackTransform.SetParent(m_transform);
            col.size = m_attackTriggerSize;
            //Set collider to a trigger
            col.isTrigger = true;

            //Set the layer
            attackObj.layer = ENEMY_LAYER;
            attackObj.tag = "Untagged";

            //Set the position of the collider
            Vector3 triggerPos = m_attackTriggerPosition;
            triggerPos.x = m_transform.position.x;
            attackTransform.position = triggerPos;

            //Set attack area variables
            m_attackArea.SetCollider(col);
            m_attackArea.SetAttack(this);
        }

        /// <summary>
        /// Gets the tendril ready
        /// </summary>
        void SetupTendril()
        {
            m_tendrilStartPos = m_tendril.transform.position;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // When landing on the floor
            if (collision.gameObject.CompareTag("Floor")) OnLandEvent.Invoke();
        }
    }
}

