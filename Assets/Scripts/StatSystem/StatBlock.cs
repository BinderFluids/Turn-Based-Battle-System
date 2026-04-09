using UnityEngine;

[System.Serializable]
public class StatBlock
{
    [field: SerializeField] public Stat FP { get; private set; }
    [field: SerializeField] public Stat HP { get; private set; }
    [field: SerializeField] public Stat BP { get; private set; }
}