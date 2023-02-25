/*
 * Date Created: 01/11/2022
 * Author: Nicholas Connell
 */

using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    [Tooltip("The prefab to spawn")]
    [SerializeField] private GameObject m_prefab;

    [Tooltip("")]
    [SerializeField] private float m_destroyTimer = 2.0f;

    /// <summary>
    /// Spawns a prefab
    /// </summary>
    public void Spawn()
    {
        //If the prefab exists...
        if (m_prefab)
        {
            //Spawn the prefab
            GameObject prefab = Instantiate(m_prefab, transform.position, Quaternion.identity);
            //Destroy the prefab after a amount of time
            Destroy(prefab, m_destroyTimer);
        }
    }
}
