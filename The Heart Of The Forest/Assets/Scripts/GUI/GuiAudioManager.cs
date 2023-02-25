/*
 * Date Created: 28.09.2022
 * Author: Nghia Do
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HotF.GUI
{
    /// <summary>
    /// Setup audio for GUI
    /// </summary>
    public class GuiAudioManager : MonoBehaviour
    {
        [Header("Audio")]
        [Tooltip("GUI AudioSource")]
        [SerializeField] AudioSource audioSource;
        [Tooltip("Button click audioClip")]
        [SerializeField] AudioClip btnClickClip;
        [Tooltip("Button hover audioClip")]
        [SerializeField] AudioClip btnHoverClip;

        [Tooltip("Parent Canvas")]
        [SerializeField] GameObject uiCanvas;
        [Tooltip("list of all buttons")]
        [SerializeField] Button[] buttons;

        /// <summary>
        /// Event Trigger for button hover
        /// </summary>
        private EventTrigger.Entry triggerEntry;

        // Start is called before the first frame update
        void Start()
        {
            if (!audioSource) audioSource = GetComponent<AudioSource>();

            // Create button event to be added to all buttons
            triggerEntry = new EventTrigger.Entry();
            triggerEntry.eventID = EventTriggerType.PointerEnter;
            triggerEntry.callback.AddListener(delegate { audioSource.PlayOneShot(btnHoverClip); });

            SetupButtons();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Setup button click and hover audio
        /// </summary>
        private void SetupButtons()
        {
            for (int  btnIdx = 0; btnIdx < buttons.Length; btnIdx++)
            {
                buttons[btnIdx].onClick.AddListener(delegate { audioSource.PlayOneShot(btnClickClip); });

                //Add an event trigger if not avaliable
                if (!buttons[btnIdx].GetComponent<EventTrigger>())
                    buttons[btnIdx].gameObject.AddComponent<EventTrigger>();

                buttons[btnIdx].GetComponent<EventTrigger>().triggers.Add(triggerEntry);
            }
        }
    }
}