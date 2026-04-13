using System;
using System.Collections.Generic;
using System.Linq;
using EventBus; 
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;
using StatusEffectSystem;
using UnityEngine.UI;

public partial class BattleEntity : MonoBehaviour
{
    public PhysicalBattleEntityModifier physicalBattleEntityModifier;
    
    private Dictionary<Type, IBattleEntityComponent> components;
    public new bool TryGetComponent<T>(out T component) where T : IBattleEntityComponent
    {
        if (components.TryGetValue(typeof(T), out var value) && value is T typed)
        {
            component = typed;
            return true;
        }

        component = default;
        return false;
    }
        
    void Awake()
    {
        Registry<BattleEntity>.TryAdd(this); 
        components = GetComponents<IBattleEntityComponent>().ToDictionary(c => c.GetType());
    }

    
    private void OnDestroy()
    {
        Registry<BattleEntity>.Remove(this); 
    }
}