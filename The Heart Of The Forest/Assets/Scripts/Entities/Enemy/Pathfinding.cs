/*
 * Date Created: 27/09/2022
 * Author: Nicholas Connell
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private Node[,] m_points;
    private Node m_startNode;
    private Node m_endNode;

    private float m_width;
    private float m_height;
    private float m_pointDensity;

    [SerializeField] private GridData m_gridData;

    //Getters and Setters
    public void SetGrid(GridData grid) => m_gridData = grid;

    public void SetupForPathfinding(GameObject thisObj, GameObject toObj)
    {
        m_points = m_gridData.m_nodeList;
        m_width = m_gridData.m_width;
        m_height = m_gridData.m_height;
        m_pointDensity = m_gridData.m_pointDensity;

        m_startNode = GetClosestNode(thisObj);
        m_endNode = GetLastNode(toObj);
    }

    /// <summary>
    /// Start node
    /// </summary>
    public Node GetClosestNode(GameObject obj)
    {
        Node closestNode = m_points[0, 0];
        Vector2 objPos = obj.transform.position;
        float dist = Vector2.Distance(objPos, closestNode.m_position);

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (m_points[j, i].m_nodeType == NodeType.TRAVERSABLE)
                {
                    if (Vector2.Distance(objPos, m_points[j, i].m_position) < dist)
                    {
                        dist = Vector2.Distance(objPos, m_points[j, i].m_position);
                        closestNode = m_points[j, i];
                    }
                }
            }
        }

        return closestNode;
    }

    /// <summary>
    /// End node
    /// </summary>
    public Node GetLastNode(GameObject obj)
    {
        Node closestNode = m_points[0, 0];
        Vector2 objPos = obj.transform.position;
        float dist = Vector2.Distance(objPos, closestNode.m_position);

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (m_points[j, i].m_nodeType == NodeType.TRAVERSABLE)
                {
                    if (Vector2.Distance(objPos, m_points[j, i].m_position) < dist)
                    {
                        dist = Vector2.Distance(objPos, m_points[j, i].m_position);
                        closestNode = m_points[j, i];
                    }
                }
            }
        }

        return closestNode;
    }

    List<Node> GetShortestPath()
    {
        List<Node> shortest = new List<Node>();
        shortest.Add(m_endNode);
        Node cur = m_endNode;

        while (cur != m_startNode)
        {
            shortest.Add(cur);
            cur = cur.m_parent;
        }

        return shortest;
    }

    static float SortByFScore(Node a, Node b)
    {
        return a.m_fScore.CompareTo(b.m_fScore);
    }

    /// <summary>
    /// A Star pathfinding
    /// </summary>
    public List<Node> AStar()
    {
        List<Node> list = new List<Node>();

        //Add first node to open list
        list.Add(m_startNode);

        //While the list isn't empty...
        while (list.Count > 0)
        {
            //Sort the list by F score
            list.Sort((a, b) => a.m_fScore.CompareTo(b.m_fScore));
            Node current = list.First();
            list.Remove(list.First());

            for (int i = 0; i < current.m_neighbours.Count; i++)
            {
                Node node = current.m_neighbours[i];

                if (!node.m_visited && node.m_nodeType == NodeType.TRAVERSABLE)
                {
                    node.m_gScore = current.m_gScore + Vector2.Distance(node.m_position, current.m_position);

                    float dx = Mathf.Abs(node.m_position.x - m_endNode.m_position.x);
                    float dy = Mathf.Abs(node.m_position.y - m_endNode.m_position.y);
                    float D = m_pointDensity;
                    float D2 = Mathf.Sqrt(2);
                    node.m_heuristic = D * (dx + dy) + (D2 - 2 * D) * Mathf.Min(dx, dy);

                    node.m_fScore = node.m_gScore + node.m_heuristic;
                    node.m_parent = current;
                    list.Add(node);

                    node.m_visited = true;
                }

                if (node == m_endNode)
                {
                    Debug.Log("Found end node");

                    foreach (var point in m_points)
                    {
                        point.m_visited = false;
                    }

                    return GetShortestPath();
                }
            }
        }

        return list;
    }
}
