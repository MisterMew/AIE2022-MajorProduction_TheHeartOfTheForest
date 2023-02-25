/*
 * Date Created: 22.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using HotF.Hud;
using UnityEngine;

namespace HotF.Interactable
{
    /// <summary>
    /// Interface Interactable for the players interaction systems
    /// </summary>
    public interface IInteractable //interface doesn't inherit
    {
        /* Variables */
        public string hudMessage { get; set; }

        public void Interact(GameObject interaction);
    }
}