using UnityEngine;

namespace DoorGame.Events
{
    public class VoidEventTrigger : MonoBehaviour
    {
        [SerializeField] private VoidEvent eventToTrigger;

        public void TriggerEvent() => eventToTrigger.Invoke(new Empty());
    }
}
