using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame.GameplayEvents.MagicalDoorEvent
{
    public class MagicalDoorHandler : MonoBehaviour
    {
        private string[] _tags;

        private void Start()
        {
            _tags = new string[] { "MagicalDoor", "CursedDoor" };
        }

        public void SetTag()
        {
            gameObject.tag = _tags[Random.Range(0, _tags.Length)];
        }
    }
}
