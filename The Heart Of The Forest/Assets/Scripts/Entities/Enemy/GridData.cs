/*
 * Date Created: 28/09/2022
 * Author: Nicholas Connell
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "GridData", menuName = "HotF/Data/GridData")]
public class GridData : ScriptableObject
{
    public Node[,] m_nodeList;
    public Node m_startNode;
    public Node m_endNode;

    public float m_width;
    public float m_height;
    public float m_pointDensity;

    public void SaveGrid(Node[,] nodeList)
    {
        m_nodeList = nodeList;
        Debug.Log("Saving grid to: " + name);

        SaveAsset();
    }

    public void SaveAsset()
    {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif
    }
}
