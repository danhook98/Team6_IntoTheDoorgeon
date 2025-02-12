using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;

    private void DecideBadDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(doors[Random.Range(0, doors.Length)].name);
        }
        
    }
}
