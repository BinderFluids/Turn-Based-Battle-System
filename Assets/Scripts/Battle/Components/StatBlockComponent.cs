using System;
using Battle.Requests;
using Battle.Events; 
using Core.Stats;
using UnityEngine;
using EventBus;
using RequestHub; 




namespace Battle.Components
{
    
    public class StatBlockComponent : BattleEntityComponent
    {
        [SerializeField] private StatBlockDefinition definition; 
        [SerializeField] private StatBlock statBlock;
        public StatBlock StatBlock => statBlock;

        private EventBinding<AttackEntityEvent> attackEntityBinding;
        
    
        protected override void Awake()
        {
            base.Awake();
        
            if (definition == null)
            {
                Debug.LogError($"{name} is missing a StatBlockDefinition.");
                return;
            }
        
            statBlock = new StatBlock(new StatsMediator(), definition);

            attackEntityBinding = new EventBinding<AttackEntityEvent>(HandleAttackEntityEvent);
            EventBus<AttackEntityEvent>.Register(attackEntityBinding); 
            
            RequestHub<RequestAttackValue>
                .Register(Entity, () => new RequestAttackValue {AttackValue = statBlock.Attack.Value});
        }

        
        void HandleAttackEntityEvent(AttackEntityEvent e)
        {
            if (e.Target != Entity) return;
            AddHealth(e.Damage); 
        }
    
        public void AddHealth(int amt)
        {
            Debug.LogWarning($"Adding {amt} health to {gameObject.name}");
            StatBlock.Health.Add(amt); 
        }

        private void OnDestroy()
        {
            EventBus<AttackEntityEvent>.Deregister(attackEntityBinding); 
            RequestHub<RequestAttackValue>.Deregister(Entity); 
        }
    }
}