
using Core.Stats;
using UnityEngine;

public partial class BattleEntity
{
    [SerializeField] private StatBlockComponent statBlockComponent;
    public StatBlock StatBlock => statBlockComponent.StatBlock;public void AddHealth(int amt)
    {
        Debug.LogWarning($"Adding {amt} health to {gameObject.name}");
        StatBlock.Health.Add(amt); 
    }
}