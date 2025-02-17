using DoorGame.Events;
using UnityEngine;
using System.Collections;

namespace DoorGame.Door
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private BoolEvent onDoorOpenedEvent;
        [SerializeField] private Animator doorAnimator;

        private bool _canOpen = true;
        public bool isRoomResetting = false;

        public void OpenDoor()
        {
            if (!_canOpen) return; 
            
            bool badDoor = gameObject.CompareTag("BadDoor");

            switch (tag)
            {
                case "BadDoor":
                    StartCoroutine(BadDoorPicked());
                    break;
                default:
                    StartCoroutine(GoodDoorPicked());
                    break;
            }
            onDoorOpenedEvent.Invoke(badDoor);
        }

        public IEnumerator BadDoorPicked()
        {
            doorAnimator.SetTrigger("BadDoorOpened");
            yield return new WaitForSeconds(1.1f);
        }
        
        public IEnumerator GoodDoorPicked()
        {
            doorAnimator.SetTrigger("GoodDoorOpened");
            yield return new WaitForSeconds(1.1f);
        }

        public IEnumerator ResetToDefault()
        {
            isRoomResetting = true;
            yield return new WaitForSeconds(1.1f);
            isRoomResetting = false;
            doorAnimator.SetTrigger("RoomReset");
        }
        
        public void PreventOpening() => _canOpen = false;
    }
}
