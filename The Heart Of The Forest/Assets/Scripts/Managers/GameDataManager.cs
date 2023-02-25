/*
 * Date Created: 24.08.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEditor;
using HotF.Interactable;

namespace HotF
{
    /// <summary>
    /// Manages game data
    /// </summary>
    public class GameDataManager : MonoBehaviour
    {
        [Header("Managers/Controllers")]
        [SerializeField] GameController gameController;
        [SerializeField] AudioManager audioManager;
        [SerializeField] Graphics.GraphicsManager graphicsManager;

        [Header("Data")]
        [Tooltip("Game Data")]
        [SerializeField] public GameData gameData;
        [Tooltip("Setting Data")]
        [SerializeField] public SettingsData settingsData;
        [Tooltip("Statistics Data")]
        [SerializeField] public StatisticsData statisticsData;
        [Tooltip("Player controls (input actions)")]
        [SerializeField] private InputActionAsset inputActionAsset;

        [Header("Initial positions")]
        [Tooltip("List of heart fragments")]
        [SerializeField] public Interactable.HeartFragInteractable[] heartFragmentList;
        [Tooltip("Default spawn point")]
        [SerializeField] private Transform defaultSpawnpoint;

        [Header("Statistics")]
        [SerializeField] private string dataPath = "";
        [SerializeField] private string dataParentFolder = "";
        [SerializeField] private string dataFilename = "";

        [Header("Default values: Game Data")]
        [SerializeField] private string defaultVersion;
        [SerializeField] private Vector3 defaultWaypoint;
        [SerializeField] private int defaultHfCount;

        [Header("Default values: Audio Data")]
        [SerializeField] private float defaultMasterVolume;
        [SerializeField] private float defaultBgmVolume;
        [SerializeField] private float defaultAmbienceVolume;
        [SerializeField] private float defaultSfxVolume;
        [SerializeField] private float defaultGuiSfxVolume;

        [Header("Default values: Graphics Settings")]
        [SerializeField] private string defaultScreenMode;
        [SerializeField] private int defaultScreenWidth;
        [SerializeField] private int defaultScreenHeight;
        [SerializeField] private float defaultBrightness;
        [SerializeField] private float defaultGamma;
        [SerializeField] private bool defaultVSync;
        [SerializeField] private int defaultAntiAliasing;
        [SerializeField] private int defaultGraphicsQuality;

        /// <summary>
        /// Default Spawn Point
        /// </summary>
        public Transform DefaultSpawnPoint 
        {
            get { return defaultSpawnpoint; }
            set { defaultSpawnpoint = value; }
        }

        /// <summary>
        /// An Index as a string
        /// </summary>
        string indexStr;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        /// <summary>
        /// Setup game data manager
        /// </summary>
        public void Setup()
        {
            // Find all heart fragments in the scene if it isn't set
            if (heartFragmentList.Length == 0) heartFragmentList = FindObjectsOfType<Interactable.HeartFragInteractable>();
            // Find the graphics settings if it isn't set
            if (!graphicsManager) graphicsManager = FindObjectOfType<Graphics.GraphicsManager>();

            // Set save location default path
            dataPath = Application.dataPath;
        }

        /// <summary>
        /// Setup Statistics serialization
        /// </summary>
        /// <param name="path">Path to save location</param>
        /// <param name="folder">Save folder name</param>
        /// <param name="filename">Name of save file</param>
        public void SetupStatistics(string path, string folder, string filename)
        {
            dataPath = path;
            dataParentFolder = folder;
            dataFilename = filename;
        }

        /// <summary>
        /// Reset save data gameState
        /// </summary>
        public void ResetGameData()
        {
            // Reset spawn point
            gameData.wayPoint = defaultWaypoint;

            for (int hfIdx= 0; hfIdx < heartFragmentList.Length; hfIdx++)
            {
                heartFragmentList[hfIdx].UpdateState(HeartFragmentState.UNCOLLECTED);
            }

            gameData.heartFragmentsReturned = 0;

            SetIsNewGame(false);
            SaveGameState();
        }

        /// <summary>
        /// Check if the save file version is the same as the build version
        /// </summary>
        public bool IsSaveVersionCurrent()
        {
            return (string.Compare(gameData.version, defaultVersion) == 0);
        }

        /// <summary>
        /// Reset all save data to defaults
        /// </summary>
        public void ResetAllToDefault()
        {
            // Reset gameData
            gameData.version = defaultVersion;
            gameData.wayPoint = defaultWaypoint;
            gameData.heartFragmentsReturned = 0;
            gameData.isNewGame = true;

            // Reset Heart Fragments
            for (int hfIdx = 0; hfIdx < defaultHfCount; hfIdx++)
            {
                indexStr = Convert.ToString(hfIdx);

                // Set Heart Fragment state to Uncollected
                PlayerPrefs.SetInt($"{indexStr}_heatFragmentState", 0);
            }

            // Reset Audio
            settingsData.masterVolume = defaultMasterVolume;
            settingsData.bgmVolume = defaultBgmVolume;
            settingsData.ambienceVolume = defaultAmbienceVolume;
            settingsData.sfxVolume = defaultSfxVolume;
            settingsData.guiSfxVolume = defaultGuiSfxVolume;

            // Reset Graphics
            settingsData.screenMode = defaultScreenMode;
            settingsData.screenWidth = defaultScreenWidth;
            settingsData.screenHeight = defaultScreenHeight;
            settingsData.brightness = defaultBrightness;
            settingsData.gamma = defaultGamma;
            settingsData.vSync = defaultVSync;
            settingsData.antiAliasing = defaultAntiAliasing;

            SaveGameVersion();
            SaveWaypoint();
            SaveIsNewGame();
            SaveAudioSettings();
            SaveGraphicSettings();

            LoadAudioSettings();
            LoadGraphicsSettings();
        }

        /// <summary>
        /// Save game data
        /// </summary>
        public void SaveSettings()
        {
            SaveAudioSettings();
            SaveControlBindings();
            SaveGraphicSettings();
            settingsData.SaveAsset();
            gameData.SaveAsset();
        }

        /// <summary>
        /// Load game Data
        /// </summary>
        public void LoadSettings()
        {
            LoadAudioSettings();
            LoadControlBindings();
            LoadGraphicsSettings();
            LoadControlBindings();
        }

        /// <summary>
        /// Save game version
        /// </summary>
        public void SaveGameVersion() => PlayerPrefs.SetString("version", gameData.version);

        /// <summary>
        /// Save Game State
        /// </summary>
        public void SaveGameState()
        {
            SaveIsNewGame();
            SaveWaypoint();
            SaveHeartFragments();
        }

        /// <summary>
        /// Save Game State with waypoint
        /// </summary>
        public void SaveGameState(Transform waypoint)
        {
            SaveIsNewGame();
            SaveWaypoint(waypoint);
            SaveHeartFragments();
        }

        /// <summary>
        /// Load game version
        /// </summary>
        public void LoadGameVersion() => gameData.version = PlayerPrefs.GetString("version", "version number");

        /// <summary>
        /// Load Game State
        /// </summary>
        public void LoadGameState()
        {
            LoadIsNewGame();
            
            // Load new game
            if (gameData.isNewGame) ResetGameData();

            LoadHeartFragments();
            LoadWaypoint();
            LoadSettings();
        }

        /// <summary>
        /// Save IsNewGame
        /// </summary>
        public void SaveIsNewGame() => PlayerPrefs.SetInt("isNewGame", gameData.isNewGame ? 1 : 0);

        /// <summary>
        /// Save IsNewGame
        /// </summary>
        public void SaveIsNewGame(bool newGameState)
        {
            SetIsNewGame(newGameState);
            SaveIsNewGame();
        }
        /// <summary>
        /// Save waypoint location
        /// </summary>
        public void SaveWaypoint()
        {
            PlayerPrefs.SetFloat("wayPoint_X", gameData.wayPoint.x);
            PlayerPrefs.SetFloat("wayPoint_Y", gameData.wayPoint.y);
            PlayerPrefs.SetFloat("wayPoint_Z", gameData.wayPoint.z);
        }

        /// <summary>
        /// Save waypoint location with given waypoint
        /// </summary>
        public void SaveWaypoint(Transform waypoint)
        {
            SetWaypoint(waypoint);
            PlayerPrefs.SetFloat("wayPoint_X", gameData.wayPoint.x);
            PlayerPrefs.SetFloat("wayPoint_Y", gameData.wayPoint.y);
            PlayerPrefs.SetFloat("wayPoint_Z", gameData.wayPoint.z);
        }

        /// <summary>
        /// Save Heart Fragment state and locations
        /// </summary>
        public void SaveHeartFragments()
        {
            for (int hfIdx = 0; hfIdx < heartFragmentList.Length; hfIdx++)
            {
                indexStr = Convert.ToString(hfIdx);

                // Save Heart Fragments collection state
                PlayerPrefs.SetInt(indexStr + "_heatFragmentState", (int)heartFragmentList[hfIdx].State);

                // Save Heart Fragment location
                PlayerPrefs.SetFloat(indexStr + "_heatFragment_X", heartFragmentList[hfIdx].transform.position.x);
                PlayerPrefs.SetFloat(indexStr + "_heatFragment_Y", heartFragmentList[hfIdx].transform.position.y);
                PlayerPrefs.SetFloat(indexStr + "_heatFragment_Z", heartFragmentList[hfIdx].transform.position.z);
            }

            PlayerPrefs.SetInt("heartFragmentsReturned", gameData.heartFragmentsReturned);
        }

        /// <summary>
        /// Save AudioSettings Data
        /// </summary>
        public void SaveAudioSettings()
        {
            PlayerPrefs.SetFloat("masterVolume", settingsData.masterVolume);
            PlayerPrefs.SetFloat("bgmVolume", settingsData.bgmVolume);
            PlayerPrefs.SetFloat("ambienceVolume", settingsData.ambienceVolume);
            PlayerPrefs.SetFloat("sfxVolume", settingsData.sfxVolume);
            PlayerPrefs.SetFloat("guiSfxVolume", settingsData.guiSfxVolume);
        }

        /// <summary>
        /// Save GraphicsSettings Data
        /// </summary>
        public void SaveGraphicSettings()
        {
            PlayerPrefs.SetString("screenMode", settingsData.screenMode);
            PlayerPrefs.SetInt("screenWidth", settingsData.screenWidth);
            PlayerPrefs.SetInt("screenHeight", settingsData.screenHeight);
            PlayerPrefs.SetFloat("brightness", settingsData.brightness);
            PlayerPrefs.SetFloat("gamma", settingsData.gamma);
            PlayerPrefs.SetInt("vSync", settingsData.vSync ? 1 : 0);
            PlayerPrefs.SetInt("antiAliasing", settingsData.antiAliasing);
            PlayerPrefs.SetInt("graphicsQuality", settingsData.graphicsQuality);
        }

        /// <summary>
        /// Load IsNewGame (default to new game)
        /// </summary>
        public void LoadIsNewGame() => gameData.isNewGame = PlayerPrefs.GetInt("isNewGame", 1) == 1;

        /// <summary>
        /// Load waypoint
        /// </summary>
        public void LoadWaypoint()
        {
            gameData.wayPoint.x = PlayerPrefs.GetFloat("wayPoint_X", defaultWaypoint.x);
            gameData.wayPoint.y = PlayerPrefs.GetFloat("wayPoint_Y", defaultWaypoint.y);
            gameData.wayPoint.z = PlayerPrefs.GetFloat("wayPoint_Z", defaultWaypoint.z);

            gameController.spawnPoint = gameData.wayPoint;
        }

        /// <summary>
        /// Load Heart Fragment state and locations
        /// </summary>
        public void LoadHeartFragments()
        {
            // Initialize
            gameData.heartFragmentsReturned = 0;

            for (int hfIdx = 0; hfIdx < heartFragmentList.Length; hfIdx++)
            {
                indexStr = Convert.ToString(hfIdx);

                // Set Heart Fragment state
                heartFragmentList[hfIdx].UpdateState((HeartFragmentState)PlayerPrefs.GetInt($"{indexStr}_heatFragmentState", 0));

                // If heart fragment has not been returned, then move it to its last location
                if (heartFragmentList[hfIdx].State == Interactable.HeartFragmentState.UNCOLLECTED)
                {
                    // Load Heart Fragment locations
                    heartFragmentList[hfIdx].transform.position = new Vector3(PlayerPrefs.GetFloat(indexStr + "_heatFragment_X", heartFragmentList[hfIdx].transform.position.x),
                                                                              PlayerPrefs.GetFloat(indexStr + "_heatFragment_Y", heartFragmentList[hfIdx].transform.position.y),
                                                                              PlayerPrefs.GetFloat(indexStr + "_heatFragment_Z", heartFragmentList[hfIdx].transform.position.z));

                }
                // If collected, then count it
                else
                {
                    gameData.heartFragmentsReturned++;
                }
            }
        }

        /// <summary>
        /// Load AudioSettings Data
        /// </summary>
        private void LoadAudioSettings()
        {
            SetAudioData();
            SetAudioUI();
            SetAudioMixers();
        }

        /// <summary>
        /// Load GraphicsSettings Data
        /// </summary>
        private void LoadGraphicsSettings()
        {
            SetGraphicsData();
        }

        /// <summary>
        /// Set Waypoint data
        /// </summary>
        /// <param name="transform">Possition of waypoint</param>
        public void SetWaypoint(Transform transform) => gameData.wayPoint = transform.position;

        /// <summary>
        /// Set audio data from saved data
        /// </summary>
        private void SetAudioData()
        {
            settingsData.masterVolume = PlayerPrefs.GetFloat("masterVolume", defaultMasterVolume);
            settingsData.bgmVolume = PlayerPrefs.GetFloat("bgmVolume", defaultBgmVolume);
            settingsData.ambienceVolume = PlayerPrefs.GetFloat("ambienceVolume", defaultAmbienceVolume);
            settingsData.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", defaultSfxVolume);
            settingsData.guiSfxVolume = PlayerPrefs.GetFloat("guiSfxVolume", defaultGuiSfxVolume);

            // Reset values if data loads incorrectly
            if(settingsData.masterVolume < 0.0001)
            {
                settingsData.masterVolume = defaultMasterVolume;
                settingsData.bgmVolume = defaultBgmVolume;
                settingsData.ambienceVolume = defaultAmbienceVolume;
                settingsData.sfxVolume = defaultSfxVolume;
                settingsData.guiSfxVolume = defaultGuiSfxVolume;
            }
        }

        /// <summary>
        /// Set audio ui sliders from saved data (without calling OnValueChanged)
        /// </summary>
        private void SetAudioUI()
        {
            audioManager.masterVolumeSlider.SetValueWithoutNotify(settingsData.masterVolume);
            audioManager.bgmVolumeSlider.SetValueWithoutNotify(settingsData.bgmVolume);
            audioManager.ambienceVolumeSlider.SetValueWithoutNotify(settingsData.ambienceVolume);
            audioManager.sfxVolumeSlider.SetValueWithoutNotify(settingsData.sfxVolume);
            audioManager.guiSfxVolumeSlider.SetValueWithoutNotify(settingsData.guiSfxVolume);
        }

        /// <summary>
        /// Set audio mixer volume from saved data
        /// </summary>
        private void SetAudioMixers()
        {
            audioManager.SetMixerVolume("MasterVolume", settingsData.masterVolume);
            audioManager.SetMixerVolume("BGMVolume", settingsData.bgmVolume);
            audioManager.SetMixerVolume("AmbienceVolume", settingsData.ambienceVolume);
            audioManager.SetMixerVolume("SFXVolume", settingsData.sfxVolume);
            audioManager.SetMixerVolume("GUISFXVolume", settingsData.guiSfxVolume);
        }

        /// <summary>
        /// Set graphics data from saved data
        /// </summary>
        private void SetGraphicsData()
        {
            settingsData.screenMode = PlayerPrefs.GetString("screenMode", defaultScreenMode);
            settingsData.screenWidth = PlayerPrefs.GetInt("screenWidth", defaultScreenWidth);
            settingsData.screenHeight = PlayerPrefs.GetInt("screenHeight", defaultScreenHeight);
            settingsData.brightness = PlayerPrefs.GetFloat("brightness", defaultBrightness);
            settingsData.gamma = PlayerPrefs.GetFloat("gamma", defaultGamma);
            settingsData.vSync = PlayerPrefs.GetInt("vSync", defaultVSync ? 1 : 0) == 1;
            settingsData.antiAliasing = PlayerPrefs.GetInt("antiAliasing", defaultAntiAliasing);
            settingsData.graphicsQuality = PlayerPrefs.GetInt("graphicsQuality", defaultGraphicsQuality);
        }

        /// <summary>
        /// Set new game state
        /// </summary>
        /// <param name="newGameState">State of new game</param>
        public void SetIsNewGame(bool newGameState) => gameData.isNewGame = newGameState;

        /// <summary>
        /// Save input controls
        /// </summary>
        /// <href>
        /// https://gist.github.com/alxcyl/01326c04e938921b35072bb86fc7ff2e
        /// </href>
        public void SaveControlBindings()
        {
            foreach (InputActionMap actionMap in inputActionAsset.actionMaps)
            {
                foreach (InputBinding binding in actionMap.bindings)
                {
                    // Save each binding as a key and value pair
                    PlayerPrefs.SetString(binding.id.ToString(), binding.overridePath);
                }
            }
        }

        /// <summary>
        /// Load input controls
        /// </summary>
        /// <href>
        /// https://gist.github.com/alxcyl/01326c04e938921b35072bb86fc7ff2e
        /// </href>
        public void LoadControlBindings()
        {
            InputBinding binding;
            string bindingValue = null;

            foreach (InputActionMap actionMap in inputActionAsset.actionMaps)
            {
                var bindings = actionMap.bindings;

                for (int bindingIdx = 0; bindingIdx < bindings.Count; bindingIdx++)
                {
                    binding = bindings[bindingIdx];
                    bindingValue = PlayerPrefs.GetString(binding.id.ToString(), null);

                    // If the value is null there is nothing to load
                    if (!string.IsNullOrEmpty(bindingValue)) actionMap.ApplyBindingOverride(bindingIdx, new InputBinding { overridePath = bindingValue });
                }
            }

            ControlGlyphSelector.UpdateGlyphsToMatchKeybinds();
        }

        /// <summary>
        /// Save statistics to file
        /// </summary>
        public void SaveStatistics(StatisticsData data)
        {
            // Full path to save file
            string fullPath = GetStatisticsFullPath();

            try
            {
                // Create save file if it doen't exsist
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Create Json-data from data in read-able format
                string jsonData = JsonUtility.ToJson(data, true);

                // Create stream to write data to file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(jsonData);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("[ERROR] Unable to save to path: " + fullPath + "\n" + e);
            }
        }

        /// <summary>
        /// Load statistics from file
        /// </summary>
        public StatisticsData LoadStatistics()
        {
            // Full path to save file
            string fullPath = GetStatisticsFullPath();
            StatisticsData loadedData = null;

            try
            {
                // Load data from file
                string jsonData = "";

                // Create stream to write data to file
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonData = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<StatisticsData>(jsonData);
            }
            catch (Exception e)
            {
                Debug.Log("[ERROR] Unable to load from path: " + fullPath + "\n" + e);
            }

            return loadedData;
        }

        /// <summary>
        /// Get the full path to the save file
        /// </summary>
        /// <returns></returns>
        private string GetStatisticsFullPath()
        {
            return Path.Combine(dataPath, dataParentFolder, dataFilename);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Editor for GameDataManager
    /// </summary>
    [CustomEditor(typeof(GameDataManager))]
    [CanEditMultipleObjects]
    public class GameDataManagerEditor : Editor
    {
        /// <summary>
        /// This data
        /// </summary>
        private GameDataManager data;

        /// <summary>
        /// This function is called when the object becomes enabled and active
        /// </summary>
        private void OnEnable()
        {
            data = (GameDataManager)target;
        }

        // Override the default inspector GUI
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            SerializedObject so = new SerializedObject(data);

            GUILayout.BeginHorizontal();
            // Save data
            if (GUILayout.Button("Save Settings")) data.SaveSettings();

            // Load data
            if (GUILayout.Button("Load Settings")) data.LoadSettings();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // Save HeartFragmanets
            if (GUILayout.Button("Save HeartFragmanets")) data.SaveHeartFragments();

            // Load HeartFragmanets
            if (GUILayout.Button("Load HeartFragmanets")) data.LoadHeartFragments();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // Save Statistics
            if (GUILayout.Button("Save Statistics")) data.SaveStatistics(data.statisticsData);

            // Load Statistics
            if (GUILayout.Button("Load Statistics")) data.LoadStatistics();
            GUILayout.EndHorizontal();

            // Display data
            DrawDefaultInspector();

            so.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}