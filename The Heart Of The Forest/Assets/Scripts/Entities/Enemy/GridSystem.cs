/*
 * Date Created: 26/09/2022
 * Author: Nicholas Connell
 */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum NodeType
{
    TRAVERSABLE, NON_TRAVERSABLE
}

public class Node
{
    public Vector2 m_position;
    public Vector2 m_placement;
    public NodeType m_nodeType;

    public bool m_visited;

    public float m_fScore = 0;
    public float m_gScore = 0;
    public float m_heuristic = 0;

    public List<Node> m_neighbours;

    public Node m_parent;
}

public class GridSystem : MonoBehaviour
{
    private GameObject[] m_floorObjects;
    public Node[,] m_points;

    public float m_width;
    public float m_height;

    private Vector2 m_firstPoint;

    [Header("Settings")]
    [Tooltip("How close together the points are")]
    [SerializeField, Range(0.5f, 3)] public float m_pointDensity = 0.5f;
    [Tooltip("Extra thickness to the walls to make the surrounding nodes non-traversable")]
    [SerializeField, Range(0, 3)] private float m_wallThickness = 0f;

    [Tooltip("The grid data object to save all the points")]
    public GridData m_gridData;

    private void Awake()
    {
        InitialisePoints();
    }

    /// <summary>
    /// Clears the points
    /// </summary>
    public void ClearPoints()
    {
        //Destroy all current points
        m_points = new Node[0, 0];

        m_width = 0;
        m_height = 0;
    }

    /// <summary>
    /// Initialises the points
    /// </summary>
    public void InitialisePoints()
    {
        ClearPoints();

        //Get floor objects
        m_floorObjects = GameObject.FindGameObjectsWithTag("Floor");

        //Set the width and height
        Vector2 bounds = GetComponent<BoxCollider2D>().bounds.extents;
        m_width = (bounds.x * 2) / m_pointDensity;
        m_height = (bounds.y * 2) / m_pointDensity;

        //Where we will place the first point
        m_firstPoint = new Vector2(transform.position.x - bounds.x,
            transform.position.y - bounds.y);

        InitialiseArray();
    }

    /// <summary>
    /// Initialises the points array
    /// </summary>
    public void InitialiseArray()
    {
        //Initialise points array
        m_points = new Node[(int)m_width + 1, (int)m_height + 1];

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (m_points[j, i] == null)
                {
                    m_points[j, i] = new Node();
                    //Set the position
                    Vector2 newPoint = new Vector2(m_firstPoint.x + m_pointDensity * j, m_firstPoint.y + m_pointDensity * i);
                    m_points[j, i].m_position = newPoint;
                    m_points[j, i].m_placement = new Vector2(j, i);

                    for (int k = m_floorObjects.Length; k > 0; k--)
                    {
                        if (CheckExtents(m_points[j, i].m_position, m_floorObjects[k - 1]))
                        {
                            m_points[j, i].m_nodeType = NodeType.NON_TRAVERSABLE;

                            break;
                        }
                        else
                        {
                            m_points[j, i].m_nodeType = NodeType.TRAVERSABLE;
                        }
                    }
                }
            }
        }

        SetNeighbours();

        m_gridData.SaveGrid(m_points);
        m_gridData.m_width = m_width;
        m_gridData.m_height = m_height;
        m_gridData.m_pointDensity = m_pointDensity;
    }

    /// <summary>
    /// Sets the neighbours for all the node in the grid
    /// </summary>
    void SetNeighbours()
    {
        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                //Get the current node
                Node current = m_points[j, i];
                //Clear the neighbours list
                current.m_neighbours = new List<Node>();

                if (i > 0)
                {
                    //Down
                    current.m_neighbours.Add(m_points[j, i - 1]);
                }
                //Up
                if (i < m_height - 1)
                {
                    current.m_neighbours.Add(m_points[j, i + 1]);
                }
                //Left
                if (j > 0)
                {
                    current.m_neighbours.Add(m_points[j - 1, i]);

                    //Bottom left
                    if (j > 0 && i > 0)
                    {
                        current.m_neighbours.Add(m_points[j - 1, i - 1]);
                    }
                    //Top Left
                    if (j > 0 && i < m_height - 1)
                    {
                        current.m_neighbours.Add(m_points[j - 1, i + 1]);
                    }
                }
                //Right
                if (j < m_width - 1)
                {
                    current.m_neighbours.Add(m_points[j + 1, i]);
                    //Bottom Right
                    if (j < m_width - 1 && i > 0)
                    {
                        current.m_neighbours.Add(m_points[j + 1, i - 1]);
                    }
                    //Top Right
                    if (j < m_width - 1 && i < m_height - 1)
                    {
                        current.m_neighbours.Add(m_points[j + 1, i + 1]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if a point is within the extents of an object
    /// </summary>
    /// <param name="point">The point to check</param>
    /// <param name="ext">The object with bounds</param>
    /// <returns>True if the object is within the extents of the object</returns>
    bool CheckExtents(Vector2 point, GameObject ext)
    {
        Vector2 topLeft;
        topLeft.x = ext.transform.position.x - ext.GetComponentInChildren<BoxCollider2D>().bounds.extents.x - m_wallThickness;
        topLeft.y = ext.transform.position.y + ext.GetComponentInChildren<BoxCollider2D>().bounds.extents.y + m_wallThickness;

        Vector2 bottomRight;
        bottomRight.x = ext.transform.position.x + ext.GetComponentInChildren<BoxCollider2D>().bounds.extents.x + m_wallThickness;
        bottomRight.y = ext.transform.position.y - ext.GetComponentInChildren<BoxCollider2D>().bounds.extents.y - m_wallThickness;

        return (point.x >= topLeft.x && point.y <= topLeft.y &&
                point.x <= bottomRight.x && point.y >= bottomRight.y);
    }

    private void OnDrawGizmosSelected()
    {
        //If the length of the array is greater than 0
        if (m_points != null)
        {
            for (int i = 0; i < m_height; i++)
            {
                for (int j = 0; j < m_width; j++)
                {
                    switch (m_points[j, i].m_nodeType)
                    {
                        case NodeType.TRAVERSABLE:
                            Gizmos.DrawIcon(m_points[j, i].m_position, "curvekeyframe", true, Color.green);
                            break;
                        case NodeType.NON_TRAVERSABLE:
                            Gizmos.DrawIcon(m_points[j, i].m_position, "curvekeyframe", true, Color.red);
                            break;
                    }
                }
            }
        }

        foreach (var obj in m_floorObjects)
        {
            Vector3 size = new Vector3(obj.GetComponentInChildren<BoxCollider2D>().bounds.extents.x * 2 + m_wallThickness * 2,
                obj.GetComponentInChildren<BoxCollider2D>().bounds.extents.y * 2 + m_wallThickness * 2, 0);
            Gizmos.DrawWireCube(obj.transform.position, size);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor
{
    private void Awake()
    {
        var t = target as GridSystem;
        t.InitialiseArray();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = target as GridSystem;

        EditorGUILayout.Space();

        serializedObject.Update();

        ClearPoints(t);
        InitialisePoints(t);

        serializedObject.ApplyModifiedProperties();
    }

    void ClearPoints(GridSystem t)
    {
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Clear Points"))
        {
            Debug.Log("Clearing points");
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "clear points");
            t.ClearPoints();
        }
    }

    void InitialisePoints(GridSystem t)
    {
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Initialise Points"))
        {
            Debug.Log("Initialising points");
        }
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "initialise points");
            t.InitialisePoints();
        }
    }
}
#endif