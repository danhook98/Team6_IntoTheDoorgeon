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
        }
        
        public void AdjustBrightness(float value)
        {
            _colorAdjustments.postExposure.value = value;
            
            if (_colorAdjustments.postExposure.value < -6) _colorAdjustments.postExposure.value = -6;
            if(_colorAdjustments.postExposure.value > 3) _colorAdjustments.postExposure.value = 3;
            
            PlayerPrefs.SetFloat("Brightness", value);
        }
    }
}
