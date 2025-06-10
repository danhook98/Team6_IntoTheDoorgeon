using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DoorGame
{
    public class Settings : MonoBehaviour
    {
        private Volume _postProcessVolume;
        
        private void Awake()
        {
            _postProcessVolume = GameObject.Find("PostProcessVolume").GetComponent<Volume>();
            var brightness = _postProcessVolume.GetComponent<ColorAdjustments>();
        }
        
        public void AdjustBrightness(float value)
        {
            var brightness = _postProcessVolume.GetComponent<ColorAdjustments>();
            brightness.postExposure.value += value;
        }
    }
}
