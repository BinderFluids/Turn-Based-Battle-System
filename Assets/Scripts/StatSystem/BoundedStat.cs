using System;
using UnityEngine;

[Serializable]
public class BoundedStat
{
    public int Value { get; private set;  }
    public Stat MaxValueStat { get; private set; }
    
    public BoundedStat(Stat stat, int value)
    {
        MaxValueStat = stat;
        Value = value;
    }

    public void ChangeValue(int amt) =>
        SetValue(Value + amt);
    public void SetValue(int value) => 
        Value = Mathf.CeilToInt(Mathf.Clamp(value, 0, MaxValueStat.GetValue()));
}