using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorGame
{
    public class Dice : MonoBehaviour
    {
        public int DiceID { set; get; }
        
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
