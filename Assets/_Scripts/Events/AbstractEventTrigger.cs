using UnityEngine;

namespace DoorGame.Events
{
    /// <summary>
    /// Allows a serialised event to be triggered. Useful when wanting to trigger an event from a component, such as a
    /// button click. 
    /// </summary>
    /// <typeparam name="T">The data type to pass.</typeparam>
    public class AbstractEventTrigger<T> : MonoBehaviour
    {
        [SerializeField] protected AbstractEvent<T> eventToTrigger;
        
        /// <summary>
        /// Invokes the serialised event. 
        /// </summary>
        /// <param name="value">Data to pass.</param>
        public void Trigger(T value) => eventToTrigger.Invoke(value);
    }
}