using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject badDoor;

    private bool badDoorDecided = false;

    public void DecideBadDoor()
    {
        badDoorDecided = false;
        
        if (badDoorDecided == false)
        {
            badDoor = doors[Random.Range(0, doors.Length)];
            
            for(var i = 0; i < doors.Length; i++)
                doors[i].tag = "GoodDoor";
            
            badDoor.tag = "BadDoor";
            

            Debug.Log(badDoor);
            badDoorDecided = true;
        } 
    }
}