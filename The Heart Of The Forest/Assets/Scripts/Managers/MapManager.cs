/*
 * Date Created: 17.10.2022
 * Author: Nghia Do
 * Contributors: -
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HotF
{
    /// <summary>
    /// Map Manager (Old Map system)
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        /// <summary>
        /// Holds map events for specific mapInteractables
        /// </summary>
        [System.Serializable]
        public struct MapEvent 
        {
            [SerializeField] public UnityEvent OnMapToggledOn;
            [SerializeField] public UnityEvent OnMapToggledOff;
            [SerializeField] public Interactable.MapInteractable[] mapInteractables;
        }

        [Header("General map Events")]
        [SerializeField] private MapEvent generalMapEvents;

        [Header("Specific area events")]
        [Tooltip("Map events in order of areas \n Tutorial, Village, Top, Mid, Bot")]
        [SerializeField] private MapEvent[] m_MapEvents;

        // Start is called before the first frame update
        void Start()
        {
            Setup();       
        }

        private void Setup()
        {
            MapEvent mapEvent;

            // Setup map events for specific areas
            for (int idx = 0; idx < m_MapEvents.Length; idx++)
            {
                // Set up for each mapInteractable in the area
                for (int miIdx = 0; miIdx < m_MapEvents[idx].mapInteractables.Length; miIdx++)
                {
                    mapEvent = m_MapEvents[idx];
                    m_MapEvents[idx].mapInteractables[miIdx].Setup();
                    m_MapEvents[idx].mapInteractables[miIdx].OnMapToggledOn.AddListener(delegate { mapEvent.OnMapToggledOn.Invoke(); });
                    m_MapEvents[idx].mapInteractables[miIdx].OnMapToggledOn.AddListener(delegate { generalMapEvents.OnMapToggledOn.Invoke(); });
                    m_MapEvents[idx].mapInteractables[miIdx].OnMapToggledOff.AddListener(delegate { generalMapEvents.OnMapToggledOff.Invoke(); });
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}