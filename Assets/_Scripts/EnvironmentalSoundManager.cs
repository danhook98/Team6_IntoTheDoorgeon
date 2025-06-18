using DoorGame.Audio;
using UnityEngine;
using DoorGame.EventSystem;
using System.Collections.Generic;

namespace DoorGame
{
    public class EnvironmentalSoundManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private AudioClipSOEvent onPlayEnvironmentalSfx;
        
        [Header("Environmental Sounds")]
        [SerializeField] private List<AudioClipSO> environmentalSounds;
        
        private float _nextSoundTime = 5f;
        
        void Update()
        {
            if (Time.time < _nextSoundTime) return;
            
            var nextSound = environmentalSounds[UnityEngine.Random.Range(0, environmentalSounds.Count)];
            onPlayEnvironmentalSfx.Invoke(nextSound);
            
            var randomTimeOffset = UnityEngine.Random.Range(10f, 20f);
            _nextSoundTime = Time.time + randomTimeOffset;
        }
    }
}
