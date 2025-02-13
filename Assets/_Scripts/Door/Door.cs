using DoorGame.Events;
using UnityEngine;

namespace DoorGame.Door
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private BoolEvent onDoorOpenedEvent;

        public void OpenDoor()
        {
            bool badDoor = gameObject.CompareTag("BadDoor");
            Debug.Log(badDoor);
            onDoorOpenedEvent.Invoke(badDoor);
        }
    }
}
