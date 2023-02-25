/*
 * Date Created: 26.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotF.Interactable
{
    /// <summary>
    /// Interaction with God
    /// </summary>
    public class GodInteractable : Interactable
    {
        [Tooltip("On Heart Fragment returned event")]
        public UnityEvent OnReturned;

        [Header("Managers")]
        [Tooltip("GameController")]
        [SerializeField] private GameController gameController;
        [Tooltip("GameDataManager")]
        [SerializeField] private GameDataManager gameDataManager;
        [Tooltip("God's dialogue")]
        [SerializeField] private Dialogue.Dialogue godDialogue;
        [Header("Player")]
        [SerializeField] private Player.PlayerMovement playerMovement;
        [SerializeField] private Player.PlayerAbilityHandler playerAbility;

        [Header("Heart Fragments")]
        [SerializeField] private GameObject fullHeart;
        [SerializeField] private GameObject[] heartFragments;
        [SerializeField] private AnimationCurve heartFragScaleCurve;

        [Header("Heart Fragment returned event sequences")]
        [Tooltip("")]
        [SerializeField] Tools.EventSequencer[] eventSequencers;

        /// <summary>
        /// AudioSource
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Can interact with God
        /// </summary>
        private bool canInteract = true;

        /// <summary>
        /// Setup God Interactable
        /// </summary>
        public void Setup()
        {
            if (!gameController) gameController = FindObjectOfType<GameController>();
            if (!gameDataManager) gameDataManager = FindObjectOfType<GameDataManager>();
            if (!godDialogue) godDialogue = GetComponentInChildren<Dialogue.Dialogue>();
            if (!playerMovement) playerMovement = FindObjectOfType<Player.PlayerMovement>();
            if (!playerAbility) playerAbility = FindObjectOfType<Player.PlayerAbilityHandler>();
            audioSource = GetComponent<AudioSource>();

            int activeFragments = 0;
            bool isActive = false;

            // Show returned HeartFragments in Forest God
            for (int hfIdx = 0; hfIdx < Mathf.Min(gameDataManager.heartFragmentList.Length, heartFragments.Length); hfIdx++)
            {
                isActive = gameDataManager.heartFragmentList[hfIdx].State == HeartFragmentState.RETURNED;

                // Show collected HeartFragments, hide non-collected ones
                heartFragments[hfIdx].SetActive(isActive);

                if (isActive) activeFragments++;
            }
            fullHeart.SetActive(false);

            // All fragments returned
            if (activeFragments >= heartFragments.Length)
                ShowFullHeart();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            
		}

		/// <summary>
		/// Start interaction with god
		/// </summary>
		/// <param name="interaction"></param>
		public override void Interact(GameObject interaction)
        {
            if (!canInteract) return;

            bool hasHF = false;

            TriggerInteractedEvent();

            // Check for collected Heart Fragments
            for (int hfIdx = 0; hfIdx < gameDataManager.heartFragmentList.Length; hfIdx++)
            {
                if (gameDataManager.heartFragmentList[hfIdx].State == HeartFragmentState.COLLECTED)
                {
                    hasHF = true;
                    // Run cutscene of returned HeartFragment
                    eventSequencers[hfIdx].Run();
                }
            }

            // If not returning Heart Fragment, repeat text
            godDialogue.conversationIndex = gameDataManager.gameData.heartFragmentsReturned;
            if (!hasHF) godDialogue.Interact(interaction);

            // Save player spawn point
            gameDataManager.SaveWaypoint(transform);
		}

        /// <summary>
        /// Return collected HeartFragment
        /// </summary>
        public void ReturnHeartFragment()
        {
            int returnedCount = 0;

            // Set HeartFragment state from Collected to Returned
            for (int hfIdx = 0; hfIdx < gameDataManager.heartFragmentList.Length; hfIdx++)
            {
                // Return collected HeartFragments
                if (gameDataManager.heartFragmentList[hfIdx].State == HeartFragmentState.COLLECTED)
                {
                    gameDataManager.heartFragmentList[hfIdx].UpdateState(HeartFragmentState.RETURNED);
                    gameController.UpdateWorldState(hfIdx);

                    // Show visual of heart fragment
                    StartCoroutine(ShowHeartFragment(hfIdx));
                }

                // Count retured HeartFragments
                if (gameDataManager.heartFragmentList[hfIdx].State == HeartFragmentState.RETURNED)
                {
                    returnedCount++;
                }
            }

            gameDataManager.gameData.heartFragmentsReturned = returnedCount;
            godDialogue.conversationIndex = gameDataManager.gameData.heartFragmentsReturned;
            gameDataManager.SaveHeartFragments();

            // All fragments returned
            if (gameDataManager.gameData.heartFragmentsReturned >= heartFragments.Length)
                ShowFullHeart();
        }

        /// <summary>
        /// Show returned HeartFragment inside ForestGod
        /// </summary>
        /// <param name="index">Index of returned HeartFragment</param>
        /// <returns></returns>
        private IEnumerator ShowHeartFragment(int index)
        {
            float time = 0;
            float maxTime = heartFragScaleCurve.keys[heartFragScaleCurve.keys.Length - 1].time;

            heartFragments[index].SetActive(true);
            Transform heartTransform = heartFragments[index].transform;
            heartTransform.localScale = Vector3.zero;

            while(time < maxTime)
            {
                heartTransform.localScale = Vector3.one * heartFragScaleCurve.Evaluate(time);
                yield return new WaitForEndOfFrame();
                time += Time.deltaTime;
            }
        }

        /// <summary>
        /// Toggle CanInteract
        /// </summary>
        /// <param name="state">Interact state</param>
        public void ToggleCanInteract(bool state) => canInteract = state;

        /// <summary>
        /// Show all HeartFragments in ForestGod
        /// </summary>
        private void ShowFullHeart()
        {
            foreach (GameObject heartFrag in heartFragments)
                heartFrag.SetActive(false);
            fullHeart.SetActive(true);
        }
    }
}