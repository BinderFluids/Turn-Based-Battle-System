using Battle;
using UnityEngine;

namespace Battle.Actions
{
    [CreateAssetMenu(menuName = "Battle/Action/Inflict Status", fileName = "InflictStatus")]
    public class InflictPoison : ScriptableBattleAction
    {
        [SerializeField] private int damage;
        [SerializeField] private int duration; 
    
        public override void StartAction(BattleEntity actor, BattleEntity target)
        {
            if (target.TryGetComponent(out StatusHandlerComponent statusHandler))
            {
                var statusEffectInstance = new PoisonStatusEffect(damage, duration);
                statusHandler.AddStatusEffect(statusEffectInstance);
            }
        
            EndAction(actor);
        }
    }
}