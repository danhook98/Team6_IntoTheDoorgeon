using UnityEngine;
using DoorGame.Events;
using Random = UnityEngine.Random;

namespace DoorGame.Door
{
    public class DoorController : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private VoidEvent onGoodDoorOpenedEvent; 
        [SerializeField] private VoidEvent onBadDoorOpenedEvent; 
        
        [SerializeField] private GameObject[] doors;
        
        private GameObject _badDoor;

        public void GenerateDoors()
        {
            // If bad door has not been selected, pick a door randomly from the array
            // and tag every game object in the array but the bad door as "GoodDoor"
            // Bad door labelled as "BadDoor". 

            ResetDoors();
            
            doors[Random.Range(0, doors.Length)].tag = "BadDoor";
        }

        private void ResetDoors()
        {
            foreach (var door in doors)
                door.tag = "GoodDoor";
        }

        public void DoorOpened(bool isBadDoor)
        {
            if (isBadDoor)
            {
                onBadDoorOpenedEvent.Invoke(new Empty());
                Debug.Log("Bad door opened");
            }
            else
            {
                onGoodDoorOpenedEvent.Invoke(new Empty());
                Debug.Log("Good door opened");
            }
        }
    }
}