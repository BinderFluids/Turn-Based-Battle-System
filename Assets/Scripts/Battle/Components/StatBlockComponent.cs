using UnityEngine;
using Core.Stats; 

public class StatBlockComponent : BattleEntityComponent
{
    [SerializeField] private StatBlockTemplate template; 
    [SerializeField] private StatBlock statBlock;
    public StatBlock StatBlock => statBlock;

    protected override void Awake()
    {
        base.Awake();
        
        if (template == null)
        {
            Debug.LogError($"{name} is missing a StatBlockTemplate.");
            return;
        }
        
        statBlock = new StatBlock(new StatsMediator(), template); 
    }
    
    public void AddHealth(int amt)
    {
        Debug.LogWarning($"Adding {amt} health to {gameObject.name}");
        StatBlock.Health.Add(amt); 
    }
}