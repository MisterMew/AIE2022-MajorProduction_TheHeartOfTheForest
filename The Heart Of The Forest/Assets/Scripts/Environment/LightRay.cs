/*
 * Date Created: 06/10/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    private Transform m_transform;

    [Tooltip("If the ray is moving left")]
    [SerializeField] private bool m_moveLeft;

    [Tooltip("How far the ray will travel")]
    [SerializeField] private float m_distance = 0.6f;
    [Tooltip("How long it will take for the ray to move from x to y")]
    [SerializeField] private Vector2 m_moveTime;
    [Tooltip("How long the ray will stop after it has reached its end point")]
    [SerializeField] private Vector2 m_stopTime;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveRay());
    }

    IEnumerator MoveRay()
    {
        float elapsedTime = 0;
        float waitTime = Random.Range(m_moveTime.x, m_moveTime.y);

        //Get the position
        Vector2 oldPos = m_transform.position;

        while (elapsedTime < waitTime)
        {
            Vector2 newPos = m_transform.position;

            //If we're moving left, then move left, otherwise move right
            if (m_moveLeft)
                newPos.x = Mathf.Lerp(oldPos.x, oldPos.x - m_distance, Mathf.SmoothStep(0, 1, elapsedTime / waitTime));
            else
                newPos.x = Mathf.Lerp(oldPos.x, oldPos.x + m_distance, Mathf.SmoothStep(0, 1, elapsedTime / waitTime));

            //Set the new position
            m_transform.position = newPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //Swap the direction
        m_moveLeft = m_moveLeft ? false : true;

        //Get random stop time
        float stopTime = Random.Range(m_stopTime.x, m_stopTime.y);
        //Wait for the stop time amount
        yield return new WaitForSeconds(stopTime);

        //Restart the coroutine
        StartCoroutine(MoveRay());
    }
}
