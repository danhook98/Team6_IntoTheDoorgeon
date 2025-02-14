using System.Collections.Generic;
using UnityEngine;

// Used this tutorial for a different way of doing events: https://www.youtube.com/watch?v=nJkYCU8Qe8o

namespace DoorGame.Events
{
    /// <summary>
    /// An abstract generic event channel scriptable object. Listeners can register and unregister. The event can be
    /// triggered by calling the Invoke method. 
    /// </summary>
    /// <typeparam name="T">The data type used for the inheriting class.</typeparam>
    public abstract class AbstractEvent<T> : ScriptableObject
    {
        private List<AbstractEventListener<T>> _listeners;

        /// <summary>
        /// Registers the given listener to this event. 
        /// </summary>
        /// <param name="listener">Listener to register.</param>
        public void Register(AbstractEventListener<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }
        }

        /// <summary>
        /// Unregisters the given listener to this event.
        /// </summary>
        /// <param name="listener">The listener to unregister.</param>
        public void Unregister(AbstractEventListener<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        /// <summary>
        /// Invokes the event of the scriptable object and passes the set type value.
        /// </summary>
        /// <param name="value">The value to pass.</param>
        public void Invoke(T value)
        {
            foreach (AbstractEventListener<T> listener in _listeners)
            {
                listener.Listen(value);
            }
        }
    }

    public readonly struct Empty {}
}