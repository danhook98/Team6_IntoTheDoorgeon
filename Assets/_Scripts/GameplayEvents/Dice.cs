using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorGame
{
    public class Dice : MonoBehaviour
    {
        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
