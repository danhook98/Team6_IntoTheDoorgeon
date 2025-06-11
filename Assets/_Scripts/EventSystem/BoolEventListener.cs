using UnityEngine;

namespace DoorGame.EventSystem
{
    public class BoolEventListener : AbstractEventListener<bool>
    {
        [SerializeField] private bool invertEventState = false;
        
        public override void Listen(bool value) => onEvent?.Invoke(invertEventState ? !value : value);
    }
}
