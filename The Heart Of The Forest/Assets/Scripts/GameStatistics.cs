/*
 * Date Created: 06.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;

namespace HotF
{
    /// <summary>
    /// Stores the games statstics for the current game save
    /// </summary>
    public class GameStatistics : MonoBehaviour
    {
        /* Variables */
        [Header("Game Stats")]
        public float playtimeThisSession = 0F;
        public float playtimeThisSave = 0F;

        [Header("Player Stats")]
        public float totalPlayerDeaths = 0F;
        public float totalDistanceTravelled = 0F;
        public float totalDamageTaken = 0F;

        [Header("Ability Stats")]
        public int numTimesBurrowed = 0;
        public float longestTimeSpentBurrowed = 0F;
        public int numTimesGlowed = 0;
        public int numTimesOvercharged = 0;

        [Header("World Stats")]
        public int numNPCsSpokenWith = 0;
    }
}