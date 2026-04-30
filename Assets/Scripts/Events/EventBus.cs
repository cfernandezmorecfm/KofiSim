using System; // Para el tipo Delegate y Type
using System.Collections.Generic; // Para el tipo Dictionary

public static class EventBus
{
    private static Dictionary<Type, Delegate> handlers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        Type eventType = typeof(T);

        if (handlers.TryGetValue(eventType, out Delegate existing))
        {
            handlers[eventType] = Delegate.Combine(existing, handler);
        }
        else
        {
            handlers[eventType] = handler;
        }
    }

    public static void Publish<T>(T evt)
    {
        Type eventType = typeof(T);
        
        if (handlers.TryGetValue(eventType, out Delegate existing))
        {
            (existing as Action<T>)?.Invoke(evt); // Llama a los handlers registrados para este tipo de evento
        }
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        Type eventType = typeof(T);

        if (handlers.TryGetValue(eventType, out Delegate existing))
        {
            Delegate remaining = Delegate.Remove(existing, handler);

            if (remaining == null) // Si no quedan handlers para este tipo de evento, lo eliminamos del diccionario
            {
                handlers.Remove(eventType);
            }
            else
            {
                handlers[eventType] = remaining;
            }
        }
    }

    public static void Clear()
    {
        handlers.Clear();
    }
}
