using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    private int baseValue;
    private HashSet<IStatModifier> modifiers = new();
    
    public Stat(int value)
        => baseValue = value;
    
    public void ChangeBaseValue(int amt) => baseValue += amt;
    public void SetBaseValue(int value) => baseValue = value;
    public int GetBaseValue() => baseValue;
    
    public int GetValue()
    {
        float runningTotal = baseValue;
        
        foreach (IStatModifier modifier in modifiers)
            runningTotal += modifier.Add(runningTotal);
        foreach (IStatModifier modifier in modifiers)
            runningTotal *= modifier.Multiply(runningTotal);
        
        return Mathf.CeilToInt(runningTotal);
    }

    public void AddModifier(IStatModifier modifier) =>
        modifiers.Add(modifier);
    
    public void RemoveModifier(IStatModifier modifier) =>
        modifiers.Remove(modifier);
}