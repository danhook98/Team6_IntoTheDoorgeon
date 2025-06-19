using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

namespace DoorGame.GameplayEvents.MagicalDoorEvent
{
    public class MagicalDoorHandler : MonoBehaviour
    {
        [SerializeField] private GameObject mysteriousDoor;
        [SerializeField] private TextMeshProUGUI text;
        
        private string[] _tags = { "MagicalDoor", "CursedDoor" };

        private void OnEnable() => SetTag();

        public void SetTag()
        {
            mysteriousDoor.tag = _tags[Random.Range(0, _tags.Length)];
            if (mysteriousDoor.CompareTag("CursedDoor"))
            {
                text.text = "You opened a cursed door!\n" + "You will only gain half the points in the next room!";
            }
            else
            {
                text.text = "You opened a magical door!\n" + "You will gain double the points in the next room!";
            }
        }
    }
}
