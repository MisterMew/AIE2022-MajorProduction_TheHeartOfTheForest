/*
 * Date Created: 22.08.2022
 * Author: Jazmin Fazzolari
 * Contributors: -
 */

using HotF.Player;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Debug Panel
/// </summary>
public class DebugPanel : MonoBehaviour
{
    /* Variables */

    [Header("Player")]
    public int heartFragCount = 0;
    public Text heartFragText;
    public PlayerHealth playerHealth;
    public Text currentLivesText;

    [Header("Debug")]
    public Text currentKeyText;

    private void Update()
    {
        heartFragText.text = heartFragCount.ToString();
        currentLivesText.text = playerHealth.CurrentLives.ToString();
    }

    public void AddHeartCount()
    {
        heartFragCount++;
    }

    public void GetCurrentKeyDown()
    {

    }
}
