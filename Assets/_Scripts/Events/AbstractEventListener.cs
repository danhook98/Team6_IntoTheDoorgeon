using UnityEngine;
using UnityEngine.Events;

namespace DoorGame.Events
{
    public abstract class AbstractEventListener<T> : MonoBehaviour
    {
        [SerializeField] private AbstractEvent<T> eventToListen;
        [SerializeField] private UnityEvent<T> onEvent;

        private void OnEnable()
        {
            eventToListen.Register(this);
        }

        private void OnDisable()
        {
            eventToListen.Unregister(this);
        }

        public void Listen(T value)
        {
            onEvent?.Invoke(value);
        }
    }
}