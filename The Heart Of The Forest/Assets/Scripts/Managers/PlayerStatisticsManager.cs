/*
 * Date Created: 16.10.2022
 * Author: Nghia Do
 * Contributors: -
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (Scrapped System)
/// </summary>
public class PlayerStatisticsManager : StatisticsManager
{
    // Start is called before the first frame update
    void Start()
    { 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Increment functions
    public void IncrementBurrowCount() => data.burrowCount++;
    public void IncrementGlideCount() => data.glideCount++;
    public void IncrementGlowCount() => data.glowCount++;
    public void IncrementBurrowFailedCount()
    {
        data.burrowFailedCount++;
        data.burrowCount--;
    }
    public void IncrementGlideFailedCount() => data.glideFailedCount++;
    public void IncrementGlowFailedCount() => data.glowFailedCount++;

}
