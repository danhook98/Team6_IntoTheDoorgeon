using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace DoorGame
{
    public class Settings : MonoBehaviour
    {
        // Resolution Related.
        private Resolution[] _resolutions;
        private TMP_Dropdown _resolutionDropdown;
        
        private void Awake()
        {
            _resolutionDropdown = GameObject.Find("Resolution Dropdown").GetComponent<TMP_Dropdown>();

            // Load PlayerPrefs
            if(PlayerPrefs.GetInt("Fullscreen") == 1) SetFullscreen(true);
            else SetFullscreen(false);
        }

        private void Start()
        {
            // Get available resolutions from the player's monitor,
            // Add them to a list of strings
            // Send the list to the resolution dropdown so it can display it.
            _resolutions = Screen.resolutions;
            _resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].ToString();
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

        /// <summary>
        /// Set game to fullscreen or windowed mode.
        /// </summary>
        /// <param name="isFullscreen"></param>
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        /// <summary>
        /// Set resolution by using an index.
        /// </summary>
        /// <param name="resolutionIndex"></param>
        public void SetResolution(int resolutionIndex)
        {
            Screen.SetResolution(_resolutions[resolutionIndex].width, _resolutions[resolutionIndex].height, Screen.fullScreen);
        }
    }
}
