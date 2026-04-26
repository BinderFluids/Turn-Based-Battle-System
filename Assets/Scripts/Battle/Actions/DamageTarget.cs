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

            int actorAttackValue = 1;
            if (RequestHub<RequestAttackValue>.TryRequest(actor, out var request))
                actorAttackValue = request.AttackValue;                 
            
            EventBus<ChangeEntityHealthEvent>.Raise(new ChangeEntityHealthEvent
            {
                Source = actor,
                Target = target,
                Damage = actorAttackValue
            });
            
            EndAction(actor); 
        }
    }
}