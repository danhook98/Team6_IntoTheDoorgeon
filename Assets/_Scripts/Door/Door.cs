using System;
using UnityEngine;
using System.Collections;
using DoorGame.Audio;
using DoorGame.EventSystem;
using Random = UnityEngine.Random;

namespace DoorGame.Door
{
    public class Door : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private BoolEvent onDoorOpenedEvent;
        [SerializeField] private VoidEvent onMysteriousDoorOpenedEvent;
        [SerializeField] private VoidEvent onMagicalDoorOpenedEvent;
        [SerializeField] private VoidEvent onCursedDoorOpenedEvent;
        [SerializeField] private FloatEvent onScoreMultiplierChangedEvent;
        
        [Header("Animator")]
        [SerializeField] private Animator doorAnimator;

        [Header("Sounds")] 
        [SerializeField] private AudioClipSOEvent playSfxAudioChannel;
        [SerializeField] private AudioClipSO doorHoverSound;
        [SerializeField] private AudioClipSO[] doorOpenSounds;
        [SerializeField] private AudioClipSO coinsDropSound;
        [SerializeField] private AudioClipSO magicalDoorOpenSound;
        [SerializeField] private AudioClipSO cursedDoorOpenSound;
        
        private ParticleSystem _doorParticles;
        
        private bool _canOpen = true;
        
        private static readonly int RoomReset = Animator.StringToHash("RoomReset");
        private static readonly int GoodDoorOpened = Animator.StringToHash("GoodDoorOpened");
        private static readonly int BadDoorOpened = Animator.StringToHash("BadDoorOpened");
        private static readonly int CursedDoorOpened = Animator.StringToHash("CursedDoorOpened");

        private void Awake()
        {
            _doorParticles = GetComponentInChildren<ParticleSystem>();
        }

        public void OpenDoor()
        {
            if (!_canOpen) return;

            float scoreMultiplier;
            
            // Play the base door open sound.
            PlayClickSound();
            
            bool badDoor = gameObject.CompareTag("BadDoor");

            switch (tag)
            {
                case "BadDoor":
                    //StartCoroutine(BadDoorPicked());
                    doorAnimator.SetTrigger(BadDoorOpened);
                    break;
                case "MagicalDoor":
                    doorAnimator.SetTrigger(GoodDoorOpened);
                    playSfxAudioChannel.Invoke(magicalDoorOpenSound);
                    playSfxAudioChannel.Invoke(coinsDropSound);
                    StartCoroutine(MysteriousDoorOpened());
                    onMagicalDoorOpenedEvent.Invoke(new Empty());
                    scoreMultiplier = 2f;
                    onScoreMultiplierChangedEvent.Invoke(scoreMultiplier);
                    break;
                case "CursedDoor":
                    doorAnimator.SetTrigger(CursedDoorOpened);
                    playSfxAudioChannel.Invoke(cursedDoorOpenSound);
                    StartCoroutine(MysteriousDoorOpened());
                    onCursedDoorOpenedEvent.Invoke(new Empty());
                    scoreMultiplier = 0.5f;
                    onScoreMultiplierChangedEvent.Invoke(scoreMultiplier);
                    break;
                default:
                    //StartCoroutine(GoodDoorPicked());
                    doorAnimator.SetTrigger(GoodDoorOpened);
                    _doorParticles.Stop();
                    _doorParticles.Play();
                    break;
            }
            
            _canOpen = false;

            if (gameObject.CompareTag("MagicalDoor") || gameObject.CompareTag("CursedDoor")) return;
            
                
            onDoorOpenedEvent.Invoke(badDoor);
            
        }

        public void ResetAnimationState()
        {
            doorAnimator.SetTrigger(RoomReset);
            _doorParticles.Stop();
        } 

        public void PreventOpening() => _canOpen = false;
        public void AllowOpening() => _canOpen = true;
        
        // Audio. 
        public void PlayHoverSound() => playSfxAudioChannel.Invoke(doorHoverSound);
        public void PlayClickSound() => playSfxAudioChannel.Invoke(doorOpenSounds[Random.Range(0, doorOpenSounds.Length)]);

        /// <summary>
        /// Wait a few seconds for the animation to play out and trigger mysterious
        /// door opened event.
        /// </summary>
        /// <returns></returns>
        public IEnumerator MysteriousDoorOpened()
        {
            yield return new WaitForSeconds(2f);
            onMysteriousDoorOpenedEvent.Invoke(new Empty());
        }
    }
}
