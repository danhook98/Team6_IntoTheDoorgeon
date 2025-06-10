using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DoorGame
{
    public class Settings : MonoBehaviour
    {
        private Volume _postProcessVolume;
        private ColorAdjustments _colorAdjustments;
        
        private void Awake()
        {
            _postProcessVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
            _postProcessVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);

            AdjustBrightness(PlayerPrefs.GetFloat("Brightness"));
            AdjustContrast(PlayerPrefs.GetFloat("Contrast"));
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
        }
    }
}
