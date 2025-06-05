using System.Collections.Generic;
using UnityEngine;

// Used this tutorial for a different way of doing events: https://www.youtube.com/watch?v=nJkYCU8Qe8o

namespace DoorGame.EventSystem
{
    /// <summary>
    /// An abstract event class, must be inherited from with a valid data type passed. Allows event listeners to be
    /// notified when the event is invoked. Listeners can register and unregister from listening to the event.
    /// </summary>
    /// <typeparam name="T">The data type passed to listeners when the event is invoked.</typeparam>
    public class AbstractEvent<T> : ScriptableObject
    {
        private List<AbstractEventListener<T>> _listeners = new();

        /// <summary>
        /// Registers an abstract event listener to the event. 
        /// </summary>
        /// <param name="listener">The listener to register.</param>
        public void Register(AbstractEventListener<T> listener)
        {
            if (!_listeners.Contains(listener)) { _listeners.Add(listener); }
        }

        /// <summary>
        /// Removes an abstract event listener from the listeners list. 
        /// </summary>
        /// <param name="listener">The listener to remove.</param>
        public void Unregister(AbstractEventListener<T> listener)
        {
            if (_listeners.Contains(listener)) { _listeners.Remove(listener); }
        }

        /// <summary>
        /// Invokes the event, calling the 'Listen' method on all registered listeners.
        /// </summary>
        /// <param name="value">The value to pass to the listeners.</param>
        public void Invoke(T value)
        {
            foreach (var listener in _listeners) { listener.Listen(value); }
        }
    }
    
    // As the abstract event class requires a data type, an 'empty' struct is used as a substitute for an empty event. 
    public readonly struct Empty {}
}