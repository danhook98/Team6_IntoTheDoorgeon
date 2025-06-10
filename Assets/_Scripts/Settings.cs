using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using TMPro;

namespace DoorGame
{
    public class Settings : MonoBehaviour
    {
        // Contrast & Brightness Related.
        private Volume _postProcessVolume;
        private ColorAdjustments _colorAdjustments;
        
        // Resolution Related.
        private Resolution[] _resolutions;
        private TMP_Dropdown _resolutionDropdown;
        
        private void Awake()
        {
            _postProcessVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
            _resolutionDropdown = GameObject.Find("Resolution Dropdown").GetComponent<TMP_Dropdown>();
            _postProcessVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);

            // Load PlayerPrefs
            AdjustBrightness(PlayerPrefs.GetFloat("Brightness"));
            AdjustContrast(PlayerPrefs.GetFloat("Contrast"));
            if(PlayerPrefs.GetInt("Fullscreen") == 1) SetFullscreen(true);
            else SetFullscreen(false);
        }

        private void Start()
        {
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + " x " + _resolutions[i].height;
                options.Add(option);

                if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentResolutionIndex;
            _resolutionDropdown.RefreshShownValue();
        }
        
        public void AdjustBrightness(float value)
        {
            _colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat("Brightness", value);
        }

        public void AdjustContrast(float value)
        {
            _colorAdjustments.contrast.value = value;
            PlayerPrefs.SetFloat("Contrast", value);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        public void SetResolution(int resolutionIndex)
        {
            Screen.SetResolution(_resolutions[resolutionIndex].width, _resolutions[resolutionIndex].height, Screen.fullScreen);
        }
    }
}
