using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DoorGame
{
    public class GlobalVolumeListener : MonoBehaviour
    {
        private Volume _postProcessVolume;
        private ColorAdjustments _colorAdjustments;
        private void Awake()
        {
            _postProcessVolume = GetComponent<Volume>();
            _postProcessVolume.profile.TryGet(out _colorAdjustments);
            
            AdjustBrightness(PlayerPrefs.GetFloat("Brightness"));
            AdjustContrast(PlayerPrefs.GetFloat("Contrast"));
        }
        
        /// <summary>
        /// Adjust brightness of game through global volume.
        /// </summary>
        /// <param name="value"></param>
        public void AdjustBrightness(float value)
        {
            _colorAdjustments.postExposure.value = value;
            PlayerPrefs.SetFloat("Brightness", value);
        }

        /// <summary>
        /// Adjust contrast of game through global volume.
        /// </summary>
        /// <param name="value"></param>
        public void AdjustContrast(float value)
        {
            _colorAdjustments.contrast.value = value;
            PlayerPrefs.SetFloat("Contrast", value);
        }
    }
}
