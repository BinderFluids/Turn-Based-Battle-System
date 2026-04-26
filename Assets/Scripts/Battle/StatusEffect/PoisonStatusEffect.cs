using Battle;
using UnityEngine;
using Battle.Events; 
using EventBus; 

namespace Battle.StatusEffect
{
    public class PoisonStatusEffect : StatusEffect, IDamageSource
    {
        private int damage;
        
        public PoisonStatusEffect(int damage, int duration) : base(duration)
        {
            this.damage = damage;
        }

        public override void Apply(BattleEntity entity)
        {
            Debug.LogWarning("Applying poison to " + entity.gameObject.name);
        }

        protected override void OnTurnStart(BattleEntity entity)
        {
            EventBus<ChangeEntityHealthEvent>.Raise(new ChangeEntityHealthEvent()
            {
                Source = this,
                Target = entity,
                Damage = damage
            });
            
            Debug.Log($"Poisoned {entity.gameObject.name} for {damage} damage");
        }
    }
}