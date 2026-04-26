using Battle.Events;
using Battle.Requests;
using EventBus; 
using UnityEngine;
using RequestHub;

namespace Battle.Actions
{
    [CreateAssetMenu(menuName = "Battle/Action/Damage Target", fileName = "DamageTarget", order = 0)]
    public class DamageTarget : ScriptableBattleAction
    {
        private BattleEntity actor;
        private BattleEntity target;
    
        public override void StartAction(BattleEntity actor, BattleEntity target)
        {
            this.actor = actor;
            this.target = target;

            EventBus<AttackEntityEvent>.Raise(new AttackEntityEvent
            {
                Source = actor,
                Target = target,
                Damage = RequestHub<RequestAttackValue>.Request(actor).AttackValue
            });
            
            EndAction(actor); 
        }
    }
}