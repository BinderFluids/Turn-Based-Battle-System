using Core.Stats;
using UnityEngine;

namespace StatusEffectSystem
{
    public class PoisonStatusEffect : StatusEffect
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
            if (entity.TryGetComponent(out StatBlockComponent statBlockComponent))
                statBlockComponent.AddHealth(-damage);
            
            Debug.Log($"Poisoned {entity.gameObject.name} for {damage} damage");
        }
    }
}