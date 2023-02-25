/*
 * Date Created: 25/10/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEngine.Events;

public class GlowMush : MonoBehaviour
{
    [SerializeField] private float m_velocityDetection = 0.3f;

    [SerializeField] private UnityEvent OnPlayerJump;

    private void OnCollisionEnter2D(Collision2D col)
    {
        //If the collided object doesn't have a rigidbody, return
        if (!col.gameObject.GetComponent<Rigidbody2D>()) return;

        //Invoke "OnPlayerJump" event
        OnPlayerJump?.Invoke();
    }
}
