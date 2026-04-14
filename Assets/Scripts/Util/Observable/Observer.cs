using System;
using UnityEngine;
using UnityEngine.Events;

public class Observer<T>
{
    [SerializeField] private T value;
    [SerializeField] private UnityEvent<T> onValueChangedUnityEvent;
    public event Action<T> onValueChanged = delegate { };

    public T Value
    {
        get => value;
        set
        {
            Set(value);
        }
    }

    public Observer(T value, UnityAction<T> callback = null)
    {
        this.value = value;
        onValueChangedUnityEvent = new UnityEvent<T>();
        if (callback != null) onValueChangedUnityEvent.AddListener(callback);
    }

    public void Set(T value)
    {
        if (Equals(this.value, value)) return;
        this.value = value;
        Invoke(); 
    }

    public void Invoke()
    {
        onValueChangedUnityEvent.Invoke(value);
        onValueChanged?.Invoke(value);
    }

    public void AddListener(UnityAction<T> callback)
    {
        if (callback == null) return;
        if (onValueChangedUnityEvent == null) return;
        
        onValueChangedUnityEvent.AddListener(callback);
    }

    public void RemoveListener(UnityAction<T> callback)
    {
        if (callback == null) return;
        if (onValueChangedUnityEvent == null) return;
        
        onValueChangedUnityEvent.RemoveListener(callback);
    }

    public void RemoveAllListeners()
    {
        if (onValueChangedUnityEvent == null) return; 
        onValueChangedUnityEvent.RemoveAllListeners();
    }

    public void Dispose()
    {
        RemoveAllListeners();
        onValueChangedUnityEvent = null;
        onValueChanged = null;
        value = default;
    }
}