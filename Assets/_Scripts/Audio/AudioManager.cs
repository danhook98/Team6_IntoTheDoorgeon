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
        
        [Header("Audio Mixer")] 
        [SerializeField] private AudioMixer audioMixer;

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
            LoadVolume();
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
        public void SetMusicVolume(float musicVolume)
        {
            // Ensure the volume value given is valid. 
            if (musicVolume is < 0 or > 1) return; 
            
            // Update the music mixer with the given volume value. A float value between 0 and 1 turns into -80 dB to
            // 0 dB in the audio mixer. The formula below grants a linear change in volume.
            float volume = (musicVolume - 1f) * 80f;
            audioMixer.SetFloat("music", volume);
            PlayerPrefs.SetFloat("musicVolume", volume); // Save changes as PlayerPrefs.
        }
        
        public void SetSFXVolume(float sfxVolume)
        {
            // Ensure the volume value given is valid. 
            if (sfxVolume is < 0 or > 1) return;
            
            // Update the SFX mixer with the given volume value. A float value between 0 and 1 turns into -80 dB to
            // 0 dB in the audio mixer. The formula below grants a linear change in volume.
            float volume = (sfxVolume - 1f) * 80f;
            audioMixer.SetFloat("SFX", volume);
            PlayerPrefs.SetFloat("sfxVolume", volume); // Save changes as PlayerPrefs.
        }
        
        private void LoadVolume()
        {
            // Load the saved volume if it exists, otherwise it defaults to 1.
            float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
            float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
            
            // Set the volume levels of the mixers.
            audioMixer.SetFloat("SFX", sfxVolume);
            audioMixer.SetFloat("music", musicVolume);
        }
    }
}