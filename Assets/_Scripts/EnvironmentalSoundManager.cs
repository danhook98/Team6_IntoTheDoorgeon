using DoorGame.Audio;
using UnityEngine;
using DoorGame.EventSystem;
using System.Collections.Generic;

namespace DoorGame
{
    public class EnvironmentalSoundManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private AudioClipSOEvent onPlayEnvironmentalSound;
        
        [Header("Environmental Sounds")]
        [SerializeField] private List<AudioClipSO> environmentalSounds;
        
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
