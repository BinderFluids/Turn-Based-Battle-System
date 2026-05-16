using System;
using System.Collections.Generic;
using UnityEngine;

namespace RequestHub
{
    public interface IRequestable  { }

    public interface IRequestableProvider { }

    public static class RequestHub<T> where T : IRequestable
    {
        public delegate T ProvideRequestDelegate();
        private static Dictionary<IRequestableProvider, ProvideRequestDelegate> providerDelegateDictionary = new();

        public static void Register(IRequestableProvider provider, ProvideRequestDelegate provideRequestDelegate)
        {
            if (!providerDelegateDictionary.TryAdd(provider, provideRequestDelegate))
                Debug.LogWarning("Key already exists for provider " + provider.GetType().Name);
        }

        public static void Register(IRequestableProvider provider, T request)
        {
            if (!providerDelegateDictionary.TryAdd(provider, () => request))
                Debug.LogWarning("Key already exists for provider " + provider.GetType().Name);
        }
        
        public static void Deregister(IRequestableProvider provider) => providerDelegateDictionary.Remove(provider);
        public static void Clear() => providerDelegateDictionary.Clear();

        public static bool TryRequest(IRequestableProvider provider, out T request)
        {
            request = default; 
            
            if (!providerDelegateDictionary.TryGetValue(provider, out ProvideRequestDelegate provideRequestDelegate))
                return false; 
            
            request =  provideRequestDelegate();
            return true; 
        }
    }
}