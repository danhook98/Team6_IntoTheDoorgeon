using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private GameObject[] doors;
        [SerializeField] private GameObject badDoor;

        public void GenerateDoors()
        {
            // If bad door has not been selected, pick a door randomly from the array
            // and tag every game object in the array but the bad door as "GoodDoor"
            // Bad door labelled as "BadDoor". 

            ResetDoors();

            badDoor = doors[Random.Range(0, doors.Length)];
            badDoor.tag = "BadDoor";

            // Debug.Log(badDoor);
        }

        private void ResetDoors()
        {
            foreach (var door in doors)
                door.tag = "GoodDoor";
        }
    }
}