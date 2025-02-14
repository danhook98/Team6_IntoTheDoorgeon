using DoorGame.Events;
using UnityEngine;

namespace DoorGame.Door
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private BoolEvent onDoorOpenedEvent;

        private bool _canOpen = true;

        public void OpenDoor()
        {
            if (!_canOpen) return; 
            
            bool badDoor = gameObject.CompareTag("BadDoor");
            
            // Play a good or bad animation. 
            
            onDoorOpenedEvent.Invoke(badDoor);
        }
        
        public void PreventOpening() => _canOpen = false;
    }
}
