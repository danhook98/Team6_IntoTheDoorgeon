using UnityEngine;
using UnityEngine.Events;

namespace DoorGame.Events
{
    /// <summary>
    /// An abstract event listener class, must be inherited from with a valid data type passed. The inherited classes
    /// can then be used on game objects as components, and will listen for the serialised event to be invoked. 
    /// </summary>
    /// <typeparam name="T">Data type to receive from an event.</typeparam>
    public class AbstractEventListener<T> : MonoBehaviour
    {
        [SerializeField] private AbstractEvent<T> eventToListen;
        [SerializeField] private UnityEvent<T> onEvent;

        /// <summary>
        /// Registers with the serialised event when the attached game object is enabled.
        /// </summary>
        protected void OnEnable() => eventToListen.Register(this);
        
        /// <summary>
        /// Unregisters from the serialised event when the attached game object is disabled.
        /// </summary>
        protected void OnDisable() => eventToListen.Unregister(this);

        /// <summary>
        /// Invokes the serialised UnityEvent when called.
        /// </summary>
        /// <param name="value">Data to pass to the UnityEvent.</param>
        public void Listen(T value) => onEvent?.Invoke(value);
    }
}