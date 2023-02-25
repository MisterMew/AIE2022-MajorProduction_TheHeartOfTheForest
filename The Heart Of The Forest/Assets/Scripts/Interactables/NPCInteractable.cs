/*
 * Date Created: 22.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF.Interactable
{
    /// <summary>
    /// Interaction override for the games NPCs
    /// </summary>
    public class NPCInteractable : Interactable
    {
        public override void Interact(GameObject interaction) => Debug.Log("Interaction: NPC");
    }
}