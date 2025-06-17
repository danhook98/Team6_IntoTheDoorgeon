using UnityEngine;
using Random = UnityEngine.Random;
using DoorGame.EventSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

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
            
            // Generate random threshold for first bad door & increase chances of bad doors spawning.
            var thresholdForBadDoor = Random.Range(1, 101);
            Debug.Log("Threshold number: " + thresholdForBadDoor);
            IncreaseBadDoorChance();
            
            // Check if first door spawns.
            if (thresholdForBadDoor > _oneBadDoorChance) return;
            
            // Copy doors array onto a list of doors, generate random index for bad door.
            List<Door> doorsCopy = doors.ToList();
            int randomIndex = Random.Range(0, doorsCopy.Count);
            Debug.Log("Doorscopy count: " + doorsCopy.Count);
            
            // Set first bad door and remove from list.
            _badDoor = doorsCopy[randomIndex];
            doorsCopy.RemoveAt(randomIndex);
            
            // Set tag and colour of first bad door.
            _badDoor.tag = "BadDoor";
            _badDoor.gameObject.GetComponent<Image>().color = Color.red;
            
            // Calculate threshold for second bad door.
            var thresholdForSecondBadDoor = Random.Range(0, 101);
            
            // Check if second bad door should be spawned.
            if (_guaranteedBadDoors > 0 && thresholdForSecondBadDoor <= _twoBadDoorChance)
            {
                _badDoor = doorsCopy[Random.Range(0, doorsCopy.Count)];
                Debug.Log("Doorscopy count: " + doorsCopy.Count);
                _badDoor.tag = "BadDoor";
            }
            _badDoor.gameObject.GetComponent<Image>().color = Color.red;
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