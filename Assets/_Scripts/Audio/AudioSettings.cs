using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace DoorGame.Audio
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider SFXSlider;
        [SerializeField] private AudioMixer audioMixer;

        private void Start()
        {
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
        
        public void SetMusicVolume()
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }
        
        public void SetSFXVolume()
        {
            float volume = SFXSlider.value;
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }
        
        private void LoadVolume()
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", SFXSlider.value);
            
            SetMusicVolume();
            SetSFXVolume();
        }
    }
}
