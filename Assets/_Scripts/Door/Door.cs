using UnityEngine;
using System.Collections;
using DoorGame.Audio;
using DoorGame.EventSystem;

namespace DoorGame.Door
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private BoolEvent onDoorOpenedEvent;
        [SerializeField] private Animator doorAnimator;

        [Header("Sounds")] 
        [SerializeField] private AudioClipSOEvent playSfxAudioChannel;
        [SerializeField] private AudioClipSO doorHoverSound;
        [SerializeField] private AudioClipSO doorClickSound;
        
        private bool _canOpen = true;
        public bool isRoomResetting = false;
        
        private static readonly int RoomReset = Animator.StringToHash("RoomReset");
        private static readonly int GoodDoorOpened = Animator.StringToHash("GoodDoorOpened");
        private static readonly int BadDoorOpened = Animator.StringToHash("BadDoorOpened");

        public void OpenDoor()
        {
            if (!_canOpen) return; 
            
            // Play the base door open sound.
            PlayClickSound();
            
            bool badDoor = gameObject.CompareTag("BadDoor");

            switch (tag)
            {
                case "BadDoor":
                    //StartCoroutine(BadDoorPicked());
                    doorAnimator.SetTrigger(BadDoorOpened);
                    break;
                default:
                    //StartCoroutine(GoodDoorPicked());
                    doorAnimator.SetTrigger(GoodDoorOpened);
                    break;
            }
            
            _canOpen = false;
            
            onDoorOpenedEvent.Invoke(badDoor);
        }

        public void ResetAnimationState() => doorAnimator.SetTrigger(RoomReset);

        public void PreventOpening() => _canOpen = false;
        public void AllowOpening() => _canOpen = true;
        
        // Audio. 
        public void PlayHoverSound() => playSfxAudioChannel.Invoke(doorHoverSound);
        public void PlayClickSound() => playSfxAudioChannel.Invoke(doorClickSound);
    }
}
