using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    private int baseValue;
    private HashSet<IStatModifier> modifiers = new();
    
    public Stat(int value)
        => baseValue = value;
    
    public float GetValue()
    {
        float runningTotal = baseValue;
        
        foreach (IStatModifier modifier in modifiers)
            runningTotal += modifier.Add(runningTotal);
        foreach (IStatModifier modifier in modifiers)
            runningTotal *= modifier.Multiply(runningTotal);
        
        return runningTotal;
    }

    public void AddModifier(IStatModifier modifier) =>
        modifiers.Add(modifier);
    
    public void RemoveModifier(IStatModifier modifier) =>
        modifiers.Remove(modifier);
}