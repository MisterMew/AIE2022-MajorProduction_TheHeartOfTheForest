/*
 * Date Created: 22.08.2022
 * Author: Nghia
 * Contributors: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace HotF
{
    /// <summary>
    /// Main Audio manager
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Tooltip("Master Mixer")]
        [SerializeField] private AudioMixer masterMixer;
        [Tooltip("Settings Data")]
        [SerializeField] private SettingsData settingsData;

        [Header("Main AudioSources")]
        [Tooltip("BGM AudioSource")]
        [SerializeField] public AudioSource bgm;
        [Tooltip("Ambience AudioSource")]
        [SerializeField] public AudioSource ambience;
        [Tooltip("GUI SFX Ambience")]
        [SerializeField] public AudioSource guiSfx;
        [Tooltip("HeartFragment Collect SFX")]
        [SerializeField] public AudioSource hfcSfx;

        [Header("Audio UI Sliders")]
        [Tooltip("masterVolume slider")]
        [SerializeField] public Slider masterVolumeSlider;
        [Tooltip("bgmVolume slider")]
        [SerializeField] public Slider bgmVolumeSlider;
        [Tooltip("ambienceVolume slider")]
        [SerializeField] public Slider ambienceVolumeSlider;
        [Tooltip("sfxVolume slider")]
        [SerializeField] public Slider sfxVolumeSlider;
        [Tooltip("guiSfxVolume slider")]
        [SerializeField] public Slider guiSfxVolumeSlider;

        /// <summary>
        /// Setup AudioManager
        /// </summary>
        public void Setup()
        {
            bgm.loop = true;
            ambience.loop = true;

            // Make sliders change mixer volume
            masterVolumeSlider.onValueChanged.AddListener(delegate { SetMasterVolume(masterVolumeSlider.value); });
            bgmVolumeSlider.onValueChanged.AddListener(delegate { SetBgmVolume(bgmVolumeSlider.value); });
            ambienceVolumeSlider.onValueChanged.AddListener(delegate { SetAmbienceVolume(ambienceVolumeSlider.value); });
            sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSfxVolume(sfxVolumeSlider.value); });
            guiSfxVolumeSlider.onValueChanged.AddListener(delegate { SetGuiSfxVolume(guiSfxVolumeSlider.value); });
        }

        // Start is called before the first frame update
        void Start()
        {
            //Setup();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Play the current bgm
        /// </summary>
        public void PlayBgm() => bgm.Play();

        /// <summary>
        /// Stop the current bgm
        /// </summary>
        public void StopBgm() => bgm.Stop();

        /// <summary>
        /// Play the current Ambience
        /// </summary>
        public void PlayAmbience() => ambience.Play();

        /// <summary>
        /// Stop the current Ambience
        /// </summary>
        public void StopAmbience() => ambience.Stop();

        /// <summary>
        /// Play the current gui sfx
        /// </summary>
        /// <param name="clip">AudioClip to play</param>
        public void PlayGuiSfx(AudioClip clip) => guiSfx.PlayOneShot(clip);

        /// <summary>
        /// Stop the current gui sfx
        /// </summary>
        public void StopGuiSfx(AudioClip clip) => guiSfx.Stop();


        /// <summary>
        /// Set volume of mixer
        /// </summary>
        /// <param name="name">Name of mixer group</param>
        /// <param name="value">Value to set volume in db</param>
        public void SetMixerVolume(string name, float value) => masterMixer.SetFloat(name, LinearToLogorithmic(value));

        /// <summary>
        /// Set masterVolume with slider
        /// </summary>
        /// <param name="value">Volume between [0,1]</param>
        public void SetMasterVolume(float value)
        {
            settingsData.masterVolume = value;
            SetMixerVolume("MasterVolume", value);
        }

        /// <summary>
        /// Set BgmVolume with slider
        /// </summary>
        /// <param name="value">Volume between [0,1]</param>
        public void SetBgmVolume(float value)
        {
            settingsData.bgmVolume = value;
            SetMixerVolume("BGMVolume", value);
        }

        /// <summary>
        /// Set AmbienceVolume with slider
        /// </summary>
        /// <param name="value">Volume between [0,1]</param>
        public void SetAmbienceVolume(float value)
        {
            settingsData.ambienceVolume = value;
            SetMixerVolume("AmbienceVolume", value);
        }

        /// <summary>
        /// Set SfxVolume with slider
        /// </summary>
        /// <param name="value">Volume between [0,1]</param>
        public void SetSfxVolume(float value)
        {
            settingsData.sfxVolume = value;
            SetMixerVolume("SFXVolume", value);
        }

        /// <summary>
        /// Set GuiSfxVolume with slider
        /// </summary>
        /// <param name="value">Volume between [0,1]</param>
        public void SetGuiSfxVolume(float value)
        {
            settingsData.guiSfxVolume = value;
            SetMixerVolume("GUISFXVolume", value);
        }

        /// <summary>
        /// Converts a linear value from 0 - 1 into logorithmic value
        /// </summary>
        /// <param name="value">Volume between [0,1]</param>
        /// <returns></returns>
        public float LinearToLogorithmic(float value) => Mathf.Log10(value) * 20;
    }

}