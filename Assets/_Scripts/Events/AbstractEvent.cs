using System.Collections.Generic;
using UnityEngine;

// Used this tutorial for a different way of doing events: https://www.youtube.com/watch?v=nJkYCU8Qe8o

public abstract class AbstractEvent<T> : ScriptableObject
{
    private List<AbstractEventListener<T>> _listeners;

    public void Register(AbstractEventListener<T> listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void Unregister(AbstractEventListener<T> listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }

    public void Invoke(T value)
    {
        foreach (AbstractEventListener<T> listener in _listeners)
        {
            listener.Listen(value);
        }
    }
}