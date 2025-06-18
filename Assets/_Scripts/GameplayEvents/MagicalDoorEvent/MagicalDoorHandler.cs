using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameplayEvents.MagicalDoorEvent
{
    public class MagicalDoorHandler : MonoBehaviour
    {
        private string[] _tags = { "MagicalDoor", "CursedDoor" };

        private void OnEnable() => SetTag();

        public void SetTag()
        {
            gameObject.tag = _tags[Random.Range(0, _tags.Length)];
        }
    }
}
