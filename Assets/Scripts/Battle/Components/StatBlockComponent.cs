using System;
using Battle.Enums;
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

        private EventBinding<ChangeEntityHealthEvent> attackEntityBinding;
        
        protected override ComponentType componentType => ComponentType.StatBlock;

        protected override void Awake()
        {
            base.Awake();
        
            if (definition == null)
            {
                Debug.LogError($"{name} is missing a StatBlockDefinition.");
                return;
            }
        
            statBlock = new StatBlock(new StatsMediator(), definition);

            attackEntityBinding = new EventBinding<ChangeEntityHealthEvent>(HandleAttackEntityEvent);
            EventBus<ChangeEntityHealthEvent>.Register(attackEntityBinding); 
            
            RequestHub<RequestableAttackValue>
                .Register(Entity, () => new RequestableAttackValue {AttackValue = statBlock.Attack.Value});
            RequestHub<RequestableDefenseValue>
                .Register(Entity, () => new  RequestableDefenseValue {DefenseValue = statBlock.Defense.Value});
            RequestHub<RequestableSpeedValue>
                .Register(Entity, () => new RequestableSpeedValue {SpeedValue = statBlock.Speed.Value});
        }

        
        void HandleAttackEntityEvent(ChangeEntityHealthEvent e)
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
            EventBus<ChangeEntityHealthEvent>.Deregister(attackEntityBinding);

            RequestHub<RequestableAttackValue>.Deregister(Entity); 
            RequestHub<RequestableDefenseValue>.Deregister(Entity);
            RequestHub<RequestableSpeedValue>.Deregister(Entity);
        }
    }
}