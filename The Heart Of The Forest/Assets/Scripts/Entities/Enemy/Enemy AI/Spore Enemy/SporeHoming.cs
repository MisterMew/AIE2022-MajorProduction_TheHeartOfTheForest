/*
 * Date Created: 28/09/2022
 * Author: Nicholas Connell
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public enum SporeQuadrant
{
    TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT
}
public class SporeHoming : MonoBehaviour
{
    private GameObject m_player;
    private Transform m_playerTransform;
    private Transform m_transform;

    public List<Node> m_shortestPath;
    private Pathfinding m_pathfinder;

    private SporeQuadrant m_lastQuadrant;
    private SporeQuadrant m_currentQuadrant;

    private bool m_completedSpawnSequence = false;

    [Tooltip("The time it takes to move from one node to the next")]
    [SerializeField] private float m_lerpTime = 1f;

    private void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_transform = transform;
        m_playerTransform = m_player.transform;
        m_pathfinder = GetComponent<Pathfinding>();
    }

    private void Start()
    {
        //Start the spawn sequence
        StartCoroutine(SpawnSequence());
    }

    private void Update()
    {
        SetQuadrant();

        //If the spawn sequence is completed, check the quadrant
        if (m_completedSpawnSequence) StartCoroutine(CheckQuadrant());
    }

    /// <summary>
    /// The spawn sequence for the spore projectile
    /// </summary>
    IEnumerator SpawnSequence()
    {
        float elapsedTime = 0;
        float waitTime = m_lerpTime;
        Vector2 pos = m_transform.position;
        Vector2 newPos = m_transform.position + m_transform.up * 2;

        while (elapsedTime < waitTime)
        {
            m_transform.position = Vector2.Lerp(pos, newPos, elapsedTime / waitTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_completedSpawnSequence = true;
        GetPath();
    }

    /// <summary>
    /// Sets which quadrant the spore is according to the players position
    /// </summary>
    void SetQuadrant()
    {
        m_lastQuadrant = m_currentQuadrant;

        switch (m_playerTransform.position.x < m_transform.position.x)
        {
            case true:
                switch (m_playerTransform.position.y > m_transform.position.y)
                {
                    case true:
                        m_currentQuadrant = SporeQuadrant.TOP_LEFT;
                        break;
                    case false:
                        m_currentQuadrant = SporeQuadrant.BOTTOM_LEFT;
                        break;
                }
                break;
            case false:
                switch (m_playerTransform.position.y > m_transform.position.y)
                {
                    case true:
                        m_currentQuadrant = SporeQuadrant.TOP_RIGHT;
                        break;
                    case false:
                        m_currentQuadrant = SporeQuadrant.BOTTOM_RIGHT;
                        break;
                }
                break;
        }
    }

    /// <summary>
    /// Checks which quadrant the spore is according to the player
    /// </summary>
    IEnumerator CheckQuadrant()
    {
        yield return new WaitForEndOfFrame();

        if (m_currentQuadrant != m_lastQuadrant)
        {
            GetPath();
        }
    }

    /// <summary>
    /// Gets the shortest path using A star
    /// </summary>
    public void GetPath()
    {
        m_pathfinder.SetupForPathfinding(gameObject, m_player);

        //Get the shortest path
        if (m_shortestPath != null && m_shortestPath.Count > 0)
        {
            m_shortestPath.Clear();
        }
        m_shortestPath = m_pathfinder.AStar();
        m_shortestPath.Reverse();
        m_shortestPath.Remove(m_shortestPath.Last());

        StopAllCoroutines();
        StartCoroutine(MoveToNextNode());
    }

    /// <summary>
    /// Moves the spore to the next node
    /// </summary>
    public IEnumerator MoveToNextNode()
    {
        float elapsedTime = 0;
        float waitTime = m_lerpTime;
        Vector2 pos = m_transform.position;

        while (elapsedTime < waitTime)
        {
            m_transform.position = Vector2.Lerp(pos, m_shortestPath.First().m_position, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        m_shortestPath.Remove(m_shortestPath.First());

        if (m_shortestPath.Count > 0)
        {
            StartCoroutine(MoveToNextNode());
        }
        else
        {
            GetPath();

            if (m_shortestPath.Count == 0)
            {
                Explode();
            }
        }
    }

    /// <summary>
    /// Explodes the spore
    /// </summary>
    void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            Explode();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SporeHoming))]
public class SporeHomingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = target as SporeHoming;

        serializedObject.Update();

        /*if (GUILayout.Button("A Star Test"))
        {
            t.GetPath();
        }*/

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
