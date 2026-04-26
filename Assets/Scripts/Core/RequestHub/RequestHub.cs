using System;
using System.Collections.Generic;
using UnityEngine;

namespace RequestHub
{
    public interface IRequest  { }

    public interface IRequestProvider { }

    public static class RequestHub<T> where T : IRequest
    {
        public delegate T ProvideRequestDelegate();

        private static Dictionary<IRequestProvider, ProvideRequestDelegate> providerDelegateDictionary = new();

        public static void Register(IRequestProvider provider, ProvideRequestDelegate provideRequestDelegate)
        {
            if (!providerDelegateDictionary.TryAdd(provider, provideRequestDelegate))
                Debug.LogWarning("Provider already exists for provider " + provider.GetType().Name);
        }
        public static void Deregister(IRequestProvider provider) => providerDelegateDictionary.Remove(provider);

        public static T Request(IRequestProvider provider)
        {
            if (!providerDelegateDictionary.TryGetValue(provider, out ProvideRequestDelegate provideRequestDelegate))
                throw new ArgumentException($"Provider '{provider}' is not registered in RequestHub"); 
            
            T value =  provideRequestDelegate();
            
            return value; 
        }
    }
}