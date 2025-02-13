using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    [SerializeField] private GameObject badDoor;

    private bool badDoorDecided = false;

    private void Update()
    {
        DecideBadDoor();
        //CheckPlayerChoice();
    }

    public void DecideBadDoor()
    {
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

/* private void CheckPlayerChoice()
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
} */
