using UnityEngine;
using Random = UnityEngine.Random;

namespace DoorGame
{
    public class MagicalDoorHandler : MonoBehaviour
    {
        private string[] _tags;
        private GameObject _mysteriousDoor;

        private void Start()
        {
            _mysteriousDoor = this.gameObject;
            _tags = new string[] { "MagicalDoor", "CursedDoor" };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) // TODO: remove after testing.
            {
                SetTag();
            }
        }

        private void SetTag()
        {
            _mysteriousDoor.tag = _tags[Random.Range(0, 2)];
        }
    }
}
