using UnityEngine;

namespace StatusEffectSystem
{
    public class PoisonStatusEffect : StatusEffect
    {
        private int damage;
        private int duration; 
        
        public PoisonStatusEffect(int damage, int duration = 3)
        {
            this.damage = damage;
            this.duration = duration;
        }

        public override void Apply(BattleEntity entity)
        {
            Debug.LogWarning("Applying poison to " + entity.gameObject.name);
        }

        public override void OnTurnStart(BattleEntity entity)
        {
            entity.AddHealth(-damage);
            Debug.Log($"Poisoned {entity.gameObject.name} for {damage} damage");

            duration--;
            if (duration == 0)
                entity.RemoveStatus(this);
        }
    }
}