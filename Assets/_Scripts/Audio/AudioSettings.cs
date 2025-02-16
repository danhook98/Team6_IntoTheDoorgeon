using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using DoorGame.Events;

namespace DoorGame.Audio
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider SFXSlider;
        [SerializeField] private AudioMixer audioMixer;
        
        [Header("Events")]
        [SerializeField] private VoidEvent OnMusicChange;
        [SerializeField] private VoidEvent OnSFXChange;

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
        
        private void SetMusicVolume()
        {
            float volume = musicSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("musicVolume", volume);
        }

        public void AdjustMusicVolume()
        {
            OnMusicChange.Invoke(new Empty());
        }
        
        public void AdjustSFXVolume()
        {
            OnSFXChange.Invoke(new Empty()); 
        }
        
        private void SetSFXVolume()
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
