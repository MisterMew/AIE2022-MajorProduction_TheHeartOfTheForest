/*
 * Date Created: 29.09.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HotF.Environment
{
    /// <summary>
    /// Swiches assets based on a dictionary key/value pairs
    /// /// </summary>
    [ExecuteInEditMode]
    public class AssetSwitcher : MonoBehaviour
    {
        [Header("Material Dictionary key/value")]
        [SerializeField] Material[] matKey;
        [SerializeField] Material[] matValue;
        [Tooltip("Also reverse key and value pairs")]
        [SerializeField] bool reversePairMat = true;

        [Header("Sprite Dictionary key/Value")]
        [SerializeField] Sprite[] sprKey;
        [SerializeField] Sprite[] sprValue;
        [Tooltip("Also reverse key and value pairs")]
        [SerializeField] bool reversePairSpr = true;

        [Header("AudioClip Dictionary key/Value")]
        [SerializeField] AudioClip[] clipKey;
        [SerializeField] AudioClip[] clipValue;
        [Tooltip("Also reverse key and value pairs")]
        [SerializeField] bool reversePairClip = true;

        [Header("Objects to switch")]
        [SerializeField] public GameObject[] goKey;
        [SerializeField] public GameObject[] goValue;
        [Tooltip("Also reverse key and value pairs")]
        [SerializeField] bool reversePairGo = true;

        [Header("Assets to switch")]
        [Tooltip("MeshRenerer Materials to switch")]
        [SerializeField] public GameObject[] meshRenderers;
        [Tooltip("SpriteRenderer Sprites to switch")]
        [SerializeField] public GameObject[] spriteRenderers;
        [Tooltip("AudioSources audioClips to switch")]
        [SerializeField] public GameObject[] audioSources;
        [Tooltip("GameObjects to switch")]
        [SerializeField] public GameObject[] gameObjectSources;

        /// <summary>
        /// Dictionary for materials
        /// </summary>
        private Dictionary<string, Material> matDict = new Dictionary<string, Material>();

        /// <summary>
        /// Dictionary for sprites
        /// </summary>
        private Dictionary<string, Sprite> sprDict = new Dictionary<string, Sprite>();

        /// <summary>
        /// Dictionary for AudioSources
        /// </summary>
        private Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();

        /// <summary>
        /// Dictionary for GameObjects
        /// </summary>
        private Dictionary<string, GameObject> goDict = new Dictionary<string, GameObject>();

        /// <summary>
        /// Text added to end of instanced objects
        /// </summary>
        private string instanceTxt = " (Instance)";

        /// <summary>
        /// Setup AssetSwitcher
        /// </summary>
        public void Setup()
        {
            // Setup Material dictionary
            for (int idx = 0; idx < matKey.Length && idx < matValue.Length; idx++)
            {
                matDict.Add(matKey[idx].name + instanceTxt, matValue[idx]);
                // Use values as keys and keys as values to create reverse entries
                if (reversePairMat) matDict.Add(matValue[idx].name + instanceTxt, matKey[idx]);
            }

            // Setup Sprite dictionary
            for (int idx = 0; idx < sprKey.Length && idx < sprValue.Length; idx++)
            {
                sprDict.Add(sprKey[idx].name, sprValue[idx]);
                // Use values as keys and keys as values to create reverse entries
                if (reversePairSpr) sprDict.Add(sprValue[idx].name, sprKey[idx]);
            }

            // Setup GameObject dictionary
            for (int idx = 0; idx < goKey.Length && idx < goValue.Length; idx++)
            {
                goDict.Add(goKey[idx].name, goValue[idx]);
                // Use values as keys and keys as values to create reverse entries
                if (reversePairGo) goDict.Add(goValue[idx].name, goKey[idx]);
            }

            // Setup AudioClip dictionary
            for (int idx = 0; idx < clipKey.Length && idx < clipValue.Length; idx++)
            {
                clipDict.Add(clipKey[idx].name, clipValue[idx]);
                // Use values as keys and keys as values to create reverse entries
                if (reversePairClip) clipDict.Add(clipValue[idx].name, clipKey[idx]);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Switch a MeshRenderer's material using a key/value pair
        /// </summary>
        /// <param name="goList">List of MeshRenderer Keys</param>
        public void SwitchMaterials(GameObject[] goList)
        {
            MeshRenderer[] renderers;
            Material mat;

            for (int idx = 0; idx < goList.Length; idx++)
            {
                // Check for empty gameobject
                if (goList[idx] == null)
                    continue;

                // Switch material based on its name
                renderers = goList[idx].GetComponentsInChildren<MeshRenderer>();
                for (int childIdx = 0; childIdx < renderers.Length; childIdx++)
                {
                    // Check if not null and has an entry in the dictionary
                    if (renderers[childIdx] && matDict.TryGetValue(renderers[childIdx].material.name, out mat)) 
                        renderers[childIdx].material = mat;
                }
            }
        }

        /// <summary>
        /// Switch a SpriteRenderer's Sprite using a key/value pair
        /// </summary>
        /// <param name="goList">List of SpriteRenderer Keys</param>
        public void SwitchSprites(GameObject[] goList)
        {
            SpriteRenderer[] renderers;
            Sprite spr;

            for (int idx = 0; idx < goList.Length; idx++)
            {
                // Check for empty gameobject
                if (goList[idx] == null)
                    continue;

                // Switch material based on its name
                renderers = goList[idx].GetComponentsInChildren<SpriteRenderer>();
                for (int childIdx = 0; childIdx < renderers.Length; childIdx++)
                {
                    // Check if not null and has an entry in the dictionary
                    if (renderers[childIdx] && sprDict.TryGetValue(renderers[childIdx].sprite.name, out spr))
                        renderers[childIdx].sprite = spr;
                }
            }
        }

        /// <summary>
        /// Switch a AudioSource's audioClip using a key/value pair
        /// </summary>
        /// <param name="goList">List of AudioSources Keys</param>
        public void SwitchAudioClips(GameObject[] goList)
        {
            AudioSource[] renderers;
            AudioClip clip;

            for (int idx = 0; idx < goList.Length; idx++)
            {
                // Check for empty gameobject
                if (goList[idx] == null)
                    continue;

                // Switch material based on its name
                renderers = goList[idx].GetComponentsInChildren<AudioSource>();
                for (int childIdx = 0; childIdx < renderers.Length; childIdx++)
                {
                    // Check if not null and has an entry in the dictionary
                    if (renderers[childIdx] && clipDict.TryGetValue(renderers[childIdx].clip.name, out clip))
                        renderers[childIdx].clip = clip;
                }
            }
        }

        /// <summary>
        /// Switch active states of GameObjects using a key/value pair
        /// </summary>
        /// <param name="goList">List of AudioSources Keys</param>
        public void SwitchGameObjects(GameObject[] goList)
        {
            GameObject go;

            for (int idx = 0; idx < goList.Length; idx++)
            {
                // Check for empty gameobject
                if (goList[idx] == null)
                    continue;

                // Check if has an entry in the dictionary
                if (goDict.TryGetValue(goList[idx].name, out go))
                {
                    go.SetActive(!go.activeSelf);
                    goList[idx].SetActive(!goList[idx].activeSelf);
                }
            }
        }

        /// <summary>
        /// Switch a MeshRenderer's material using a key/value pair
        /// </summary>
        public void SwitchMaterials() => SwitchMaterials(meshRenderers);

        /// <summary>
        /// Switch a SpriteRenderer's Sprite using a key/value pair
        /// </summary>
        public void SwitchSprites() => SwitchSprites(spriteRenderers);

        /// <summary>
        /// Switch a AudioSources's audioClip using a key/value pair
        /// </summary>
        public void SwitchAudioClips() => SwitchAudioClips(audioSources);

        /// <summary>
        /// Switch a AudioSources's audioClip using a key/value pair
        /// </summary>
        public void SwitchGameObjects() => SwitchGameObjects(gameObjectSources);
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for AssetSwitcher
    /// </summary>
    [CustomEditor(typeof(AssetSwitcher))]
    [CanEditMultipleObjects]
    public class AssetSwitcherEditor : Editor
    {
        /// <summary>
        /// This data
        /// </summary>
        private AssetSwitcher data;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            data = (AssetSwitcher)target;
        }

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(data);

            // Buttons to Switch Materials
            if (GUILayout.Button("Switch Materials")) data.SwitchMaterials();
            if (GUILayout.Button("Switch Sprites")) data.SwitchSprites();
            if (GUILayout.Button("Switch AudioClip")) data.SwitchAudioClips();
            if (GUILayout.Button("Switch GameObject")) data.SwitchGameObjects();

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}