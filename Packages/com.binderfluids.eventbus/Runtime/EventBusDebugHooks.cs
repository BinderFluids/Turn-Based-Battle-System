using System;

namespace EventBus
{
    public static class EventBusDebugHooks
    {
        public static event Action<Type, object> EventRaised;

        public static void NotifyRaised(Type eventType, object eventData)
        {
            var handler = EventRaised;
            handler?.Invoke(eventType, eventData);
        }
    }
}