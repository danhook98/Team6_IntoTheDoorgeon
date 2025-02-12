using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;

    private void Update()
    {
        DecideBadDoor();
    }
    
    private void DecideBadDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var badDoor = doors[Random.Range(0, doors.Length)];
            Debug.Log(badDoor);
        }
        
    }
}
