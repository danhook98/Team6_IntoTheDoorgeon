using UnityEngine;

namespace DoorGame
{
    public class Settings : MonoBehaviour
    {
        private void Awake()
        {
            // Load PlayerPrefs
            if(PlayerPrefs.GetInt("Fullscreen") == 1) SetFullscreen(true);
            else SetFullscreen(false);
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
    }
}
