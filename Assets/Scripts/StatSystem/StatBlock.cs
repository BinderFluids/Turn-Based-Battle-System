using System;
using UnityEngine;

[Serializable]
public class StatBlock
{
    public int BadgePoints { get; private set; }
    
    [field: SerializeField] public BoundedStat Health { get; private set; }
    [field: SerializeField] public BoundedStat TeamPoints { get; private set; }
    
    [field: SerializeField] public Stat Speed { get; private set; }
    [field: SerializeField] public Stat Attack { get; private set; }
}