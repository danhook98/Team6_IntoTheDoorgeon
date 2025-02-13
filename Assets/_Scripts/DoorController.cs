using UnityEngine;

namespace DoorGame
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private GameObject[] doors;
        [SerializeField] private GameObject badDoor;

        private bool badDoorDecided = false;

        public void DecideBadDoor()
        {
            // Resets bad door selected to false so this can be called/used again.
            badDoorDecided = false;
        
            if (badDoorDecided == false)
            {
                // If bad door has not been selected, pick a door randomly from the array
                // and tag every game object in the array but the bad door as "GoodDoor"
                // Bad door labelled as "BadDoor". 
            
                badDoor = doors[Random.Range(0, doors.Length)];
            
                for(var i = 0; i < doors.Length; i++)
                    doors[i].tag = "GoodDoor";
            
                badDoor.tag = "BadDoor";
            

                Debug.Log(badDoor);
                badDoorDecided = true;
            } 
        }
    }
}