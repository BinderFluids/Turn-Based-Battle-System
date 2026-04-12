using System;
using UnityEngine;

namespace Core.Stats
{
    public class Resource
    {
        private int value;
        private Stat maxValue;
        
        public int Value => value;
        public Stat MaxValue => maxValue;
        
        public Resource(Stat maxValue)
        {
            this.maxValue = maxValue;
            value = maxValue.Value;
        }

        public Resource(int value, Stat maxValue)
        {
            this.maxValue = maxValue;
            this.value = Mathf.Clamp(value, 0, maxValue.Value);
        }
        
        public void Add(int amount) => value = Mathf.Clamp(value + amount, 0, maxValue.Value);
    }
}