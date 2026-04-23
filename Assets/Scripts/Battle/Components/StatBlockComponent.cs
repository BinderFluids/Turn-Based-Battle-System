using Core.Stats;
using UnityEngine;

namespace Battle.Components
{
    public class StatBlockComponent : BattleEntityComponent
    {
        [SerializeField] private StatBlockDefinition definition; 
        [SerializeField] private StatBlock statBlock;
        public StatBlock StatBlock => statBlock;

        protected override void Awake()
        {
            base.Awake();
        
            if (definition == null)
            {
                Debug.LogError($"{name} is missing a StatBlockDefinition.");
                return;
            }
        
            statBlock = new StatBlock(new StatsMediator(), definition); 
        }
    
        public void AddHealth(int amt)
        {
            Debug.LogWarning($"Adding {amt} health to {gameObject.name}");
            StatBlock.Health.Add(amt); 
        }
    }
}