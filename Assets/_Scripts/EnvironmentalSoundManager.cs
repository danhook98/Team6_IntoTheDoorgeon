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
        
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time < _nextSoundTime) return;
            // pick sound and play it
            // _nextSoundTime = Time.time + (randomOffset).
        }
    }
}
