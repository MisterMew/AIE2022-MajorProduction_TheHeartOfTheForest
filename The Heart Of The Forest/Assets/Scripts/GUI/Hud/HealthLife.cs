/*
 * Date Created: 13.09.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using UnityEngine;
using UnityEngine.UI;

namespace HotF.GUI
{
    /// <summary>
    /// Handles the hud dispaly for the healthy system's lives
    /// </summary>
    public class HealthLife : MonoBehaviour
    {
        /* Variables */
        [Header("Health Components")]
        [SerializeField] private Sprite fullLife = null;
        [SerializeField] private Sprite emptyLife = null;
        private Image lifeImage = null;

        private void Awake() => lifeImage = GetComponent<Image>();

        /// <summary>
        /// Sets the image sprite to correspond to the status of the lives
        /// </summary>
        /// <param name="status"></param>
        public void SetLifeImage(LifeStatus status = 0)
        {
            switch (status)
            {
                case LifeStatus.FULL:
                    lifeImage.sprite = fullLife;
                    break;

                case LifeStatus.EMPTY:
                    lifeImage.sprite = emptyLife;
                    break;
            }
        }
    }

    public enum LifeStatus { EMPTY = 0, FULL = 1 }
}