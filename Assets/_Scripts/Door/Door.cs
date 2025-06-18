using UnityEngine;
using System.Collections;
using DoorGame.Audio;
using DoorGame.EventSystem;

namespace DoorGame.Door
{
    public class Door : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private BoolEvent onDoorOpenedEvent;
        [SerializeField] private VoidEvent onMysteriousDoorOpenedEvent;
        [SerializeField] private VoidEvent onMagicalDoorOpenedEvent;
        [SerializeField] private VoidEvent onCursedDoorOpenedEvent;
        
        [Header("Animator")]
        [SerializeField] private Animator doorAnimator;

        [Header("Sounds")] 
        [SerializeField] private AudioClipSOEvent playSfxAudioChannel;
        [SerializeField] private AudioClipSO doorHoverSound;
        [SerializeField] private AudioClipSO doorClickSound;
        [SerializeField] private AudioClipSO coinsDropSound;
        [SerializeField] private AudioClipSO magicalDoorOpenSound;
        [SerializeField] private AudioClipSO cursedDoorOpenSound;
        
        private bool _canOpen = true;
        
        private static readonly int RoomReset = Animator.StringToHash("RoomReset");
        private static readonly int GoodDoorOpened = Animator.StringToHash("GoodDoorOpened");
        private static readonly int BadDoorOpened = Animator.StringToHash("BadDoorOpened");
        private static readonly int CursedDoorOpened = Animator.StringToHash("CursedDoorOpened");

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
                case "MagicalDoor":
                    doorAnimator.SetTrigger(GoodDoorOpened);
                    playSfxAudioChannel.Invoke(magicalDoorOpenSound);
                    playSfxAudioChannel.Invoke(coinsDropSound);
                    StartCoroutine(MysteriousDoorOpened());
                    onMagicalDoorOpenedEvent.Invoke(new Empty());
                    break;
                case "CursedDoor":
                    doorAnimator.SetTrigger(CursedDoorOpened);
                    playSfxAudioChannel.Invoke(cursedDoorOpenSound);
                    StartCoroutine(MysteriousDoorOpened());
                    onCursedDoorOpenedEvent.Invoke(new Empty());
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
