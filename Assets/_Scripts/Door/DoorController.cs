using UnityEngine;
using UnityEngine.UI;
using Colour = UnityEngine.Color;
using Random = UnityEngine.Random;
using DoorGame.EventSystem;

namespace DoorGame.Door
{
    public class DoorController : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private VoidEvent onGoodDoorOpenedEvent; 
        [SerializeField] private VoidEvent onBadDoorOpenedEvent;
        
        [SerializeField] private Door[] doors;
        
        private Door _badDoor;

        public void GenerateDoors()
        {
            // If bad door has not been selected, pick a door randomly from the array and tag every game object in the
            // array but the bad door as "GoodDoor", bad door labelled as "BadDoor". 
            ResetDoors();
            
            _badDoor = doors[Random.Range(0, doors.Length)];
            _badDoor.tag = "BadDoor";
        }

        private void ResetDoors()
        {
            foreach (var door in doors)
            {
                door.tag = "GoodDoor";
                
                door.AllowOpening();
                door.ResetAnimationState();
            }
        }
        
        public void DoorOpened(bool isBadDoor)
        {
            if (isBadDoor)
            {
                onBadDoorOpenedEvent.Invoke(new Empty());
                Debug.Log("Bad door opened", this);
            }
            else
            {
                onGoodDoorOpenedEvent.Invoke(new Empty());
                Debug.Log("Good door opened", this);
            }
        }
    }
}