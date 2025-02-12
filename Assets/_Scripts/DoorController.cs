using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject badDoor;

    private void Update()
    {
        DecideBadDoor();
        CheckPlayerChoice();
    }
    
    private void DecideBadDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            badDoor = doors[Random.Range(0, doors.Length)];
            badDoor.tag = "BadDoor";
            
            Debug.Log(badDoor);
        }
    }

    private void CheckPlayerChoice()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
            
            switch (hit.collider.gameObject.tag)
            {
                case "BadDoor":
                    Debug.Log("Player has chosen the bad door");
                    break;
                default:
                    Debug.Log(hit.collider.gameObject.name);
                    break;
            }
        }
    }
    
    private void BadDoor()
    {
        // TODO: Reset current score to 0
        // Make death screen visible
        // Play BadDoor SFX and bad door animation
        Debug.Log("Player clicked on bad door!");
    }

    private void GoodDoor()
    {
        // TODO: Add score to the current score
        // Allow the player to make a new choice
        // Play GoodDoor SFX and bad door animation
        Debug.Log("Player clicked on good door!");
    }
}
