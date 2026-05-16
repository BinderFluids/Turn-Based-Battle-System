using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EventBus
{
public static class EventBus<T> where T : IEvent {
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
    
    public static void Register(EventBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(T @event) {
        
#if UNITY_EDITOR
        EventBusDebugHooks.NotifyRaised(typeof(T), @event);        
#endif
        
        
        var snapshot = new HashSet<IEventBinding<T>>(bindings);

        eventRaisedThisFrame = @event;
        wasRaisedThisFrame = true; 
        
        foreach (var binding in snapshot) {
            if (bindings.Contains(binding)) {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }
    }

    private static bool wasRaisedThisFrame = false;
    private static T eventRaisedThisFrame; 
    public static async Task<T> AwaitRaise()
    {
        while (!wasRaisedThisFrame)
            await Task.Yield();
        
        wasRaisedThisFrame = false;
        return eventRaisedThisFrame;
    }

    static void Clear() => bindings.Clear();
}
    
}

