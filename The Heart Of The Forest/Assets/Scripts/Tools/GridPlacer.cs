/*
 * Date Created: 29.09.2022
 * Author: Nghia Do
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HotF.Tools
{
    /// <summary>
    /// Place assets in a grid layout
    /// </summary>
    public class GridPlacer : MonoBehaviour
    {
        [Tooltip("Parent offset transform")]
        [SerializeField] private Transform parentOffset;
        [Tooltip("Max number of objects per row"),Range(1, 100)]
        [SerializeField] private int objectsPerRow;
        [Tooltip("Space between objects")]
        [SerializeField] private float spacing;
        [Tooltip("List of objects to place in a grid layout")]
        [SerializeField] private GameObject[] objects;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Place objects in a grid layout
        /// </summary>
        public void Gridify()
        {
            int x = 0;
            int y = 0;

            for (y = 0; y * objectsPerRow < objects.Length; y++)
            {
                for (x = 0; x < objectsPerRow; x++)
                {
                    objects[y*objectsPerRow + x].transform.position = new Vector3(x * spacing, y * spacing, 0) + parentOffset.position;
                }
            }
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for GridPlacer
    /// </summary>
    [CustomEditor(typeof(GridPlacer))]
    [CanEditMultipleObjects]
    public class GridPlacerEditor : Editor
    {
        /// <summary>
        /// This data
        /// </summary>
        private GridPlacer data;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            data = (GridPlacer)target;
        }

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(data);

            // Gridify
            if (GUILayout.Button("Gridify")) data.Gridify();

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}