using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DoorGame.Events;

namespace DoorGame.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioSource musicAudioSource;
        
        [Header("Sliders and Audio Mixer")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider SFXSlider;
        [SerializeField] private AudioMixer audioMixer;
        
        //[Header("Events")]
        //[SerializeField] private FloatEvent OnMusicChange;
        //[SerializeField] private FloatEvent OnSFXChange;

        private void Awake()
        {
            // Create audio sources if the references are broken or missing. 
            if (!sfxAudioSource)
            {
                Debug.LogWarning("<color=red>AudioManager</color>: SFX AudioSource is missing or the reference is " +
                                 "missing. Creating a new AudioSource.");
                sfxAudioSource = gameObject.AddComponent<AudioSource>();
            }

            if (!musicAudioSource)
            {
                Debug.LogWarning("<color=red>AudioManager</color>: Music AudioSource is missing or the reference is " +
                                 "missing. Creating a new AudioSource.");
                musicAudioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Load last saved player audio settings if they exist
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                LoadVolume();
            }
            else
            {
                SetSFXVolume();
                SetSFXVolume();
            }
        }

        /// <summary>
        /// Plays an audio one shot of the given AudioClipSO's clip.
        /// </summary>
        /// <param name="audioClip">AudioClipSO data file.</param>
        public void PlaySFX(AudioClipSO audioClip)
        {
            sfxAudioSource.PlayOneShot(audioClip.clip);
        }

        /// <summary>
        /// Plays an audio clip of the given AudioClipSO's clip.
        /// </summary>
        /// <param name="audioClip">AudioClipSO data file.</param>
        public void PlayMusic(AudioClipSO audioClip)
        {
            musicAudioSource.clip = audioClip.clip;
            musicAudioSource.Play();
        }
        
        // Audio Settings
        
        public void SetMusicVolume()
        {
            // Set local variable "volume" to relevant audio slider, turn 0.001 - 1 to -80 - 1 because of how AudioMixer works.
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume); // Save changes as PlayerPrefs
        }
        
        public void SetSFXVolume()
        {
            float volume = SFXSlider.value;
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
            
        }
        
        private void LoadVolume()
        {
            // Loads saved audio settings and sets the volume based on them
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", SFXSlider.value);
            
            SetMusicVolume();
            SetSFXVolume();
        }
    }
}