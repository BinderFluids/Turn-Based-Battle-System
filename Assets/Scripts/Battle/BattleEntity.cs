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
    public new bool TryGetComponent<T>(out T component)
    {
        //Attempt to find BattleComponent from dictionary
        if (typeof(T).IsAssignableFrom(typeof(IBattleEntityComponent)) &&
            components.TryGetValue(typeof(T), out var foundBattleComponent) && 
            foundBattleComponent is T typed)
        {
            component = typed;
            return true;
        }
        
        //Pass through to base TryGetComponent method
        if (gameObject.TryGetComponent(out T foundComponent))
        {
            component = foundComponent;
            return true; 
        }

        //Return false
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