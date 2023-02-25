/*
 * Date Created: 25/10/2022
 * Author: Nicholas Connell
 */

using UnityEngine;
using UnityEngine.Events;

public class PassThroughBottom : MonoBehaviour
{
    [Tooltip("If the trigger will be used sideways")]
    [SerializeField] private bool m_sideways;

    [Tooltip("When the player passes below the trigger")]
    [SerializeField] private UnityEvent OnPassBelow;
    [Tooltip("When the player passes above the trigger")]
    [SerializeField] private UnityEvent OnPassAbove;

    [Tooltip("When the player passes to the left of the trigger")]
    [SerializeField] private UnityEvent OnPassLeft;
    [Tooltip("When the player passes to the right of the trigger")]
    [SerializeField] private UnityEvent OnPassRight;


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (m_sideways)
        {
            if (other.gameObject.transform.position.x < transform.position.x)
            {
                OnPassLeft.Invoke();
            }
            else
            {
                OnPassRight.Invoke();
            }
        }
        else
        {
            if (other.gameObject.transform.position.y < transform.position.y)
            {
                OnPassBelow.Invoke();
            }
            else
            {
                OnPassAbove.Invoke();
            }
        }
    }
}
