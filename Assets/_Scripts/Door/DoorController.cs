using UnityEngine;
using Random = UnityEngine.Random;
using DoorGame.EventSystem;
using UnityEngine.UI;

namespace DoorGame.Door
{
    public class DoorController : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private VoidEvent onGoodDoorOpenedEvent; 
        [SerializeField] private VoidEvent onBadDoorOpenedEvent;
        
        [Header("Doors")]
        [SerializeField] private Door[] doors;
        
        private Door _badDoor;
        
        // Bad door chances.
        private int _oneBadDoorChance;
        private int _twoBadDoorChance;

        private int _guaranteedBadDoors;

        private void Awake()
        {
            _oneBadDoorChance = -5;
            _guaranteedBadDoors = 0;
        }
        
        public void GenerateDoors()
        {
            // If bad door has not been selected, pick a door randomly from the array and tag every game object in the
            // array but the bad door as "GoodDoor", bad door labelled as "BadDoor". 
            ResetDoors();
            
            // Check if bad door can be spawned.
            var thresholdForBadDoor = Random.Range(0, 101);
            Debug.Log("Threshold number: " + thresholdForBadDoor);
            IncreaseBadDoorChance();
            
            do
            {
                if (thresholdForBadDoor > _oneBadDoorChance && _guaranteedBadDoors == 0) return;
                _badDoor = doors[Random.Range(0, doors.Length)];

                if (_badDoor.CompareTag("BadDoor")) continue;
                _badDoor.tag = "BadDoor";
                _guaranteedBadDoors--;
                if (_guaranteedBadDoors < 0) _guaranteedBadDoors = 0;

                // TODO: Comment out after testing.
                _badDoor.gameObject.GetComponent<Image>().color = Color.red;
            } while (_guaranteedBadDoors >= 1);
            
            var thresholdForSecondBadDoor = Random.Range(0, 101);
        }

        private void ResetDoors()
        {
            foreach (var door in doors)
            {
                door.tag = "GoodDoor";
                door.gameObject.GetComponent<Image>().color = Color.white;
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

        public void IncreaseBadDoorChance()
        {
            _oneBadDoorChance += 5;

            if (_oneBadDoorChance >= 100)
            {
                _guaranteedBadDoors = 1;
                _oneBadDoorChance = 100;
                _twoBadDoorChance += 5;
            }

            if (_twoBadDoorChance >= 50)
            {
                _twoBadDoorChance = 50;
                _guaranteedBadDoors = 2;
            }
            
            Debug.Log("one bad door chance: " + _oneBadDoorChance);
            Debug.Log("Two bad door chance: " + _twoBadDoorChance);
        }
    }
}