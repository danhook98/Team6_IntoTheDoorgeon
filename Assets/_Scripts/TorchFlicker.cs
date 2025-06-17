using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class TorchFlicker : MonoBehaviour
    {
        private Light2D _light;

        private float _nextFlickerTime;

        private float _baseLightRadiusOuter;
        private float _baseLightRadiusInner;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            
            _baseLightRadiusOuter = _light.pointLightOuterRadius;
            _baseLightRadiusInner = _light.pointLightInnerRadius;
            
            _nextFlickerTime = Random.Range(0.5f, 3.5f);
        } 

        private void Update()
        {
            if (Time.time < _nextFlickerTime) return; 
            
            StartCoroutine(Flicker());
            _nextFlickerTime = Time.time + Random.Range(0.15f, 1f);
        }

        private IEnumerator Flicker()
        {
            float offset = (Random.value > 0.5f ? 1 : -1) * Random.Range(0.025f, 0.1f);
                
            _light.pointLightOuterRadius = _baseLightRadiusOuter + offset;
            _light.pointLightInnerRadius = _baseLightRadiusInner + offset;
            
            yield return new WaitForSeconds(0.10f);
            
            _light.pointLightOuterRadius = _baseLightRadiusOuter;
            _light.pointLightInnerRadius = _baseLightRadiusInner;
        }
    }
}
