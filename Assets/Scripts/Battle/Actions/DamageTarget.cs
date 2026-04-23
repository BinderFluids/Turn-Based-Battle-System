using Core.Stats;
using UnityEngine;
using Battle.Components;

namespace Battle.Actions
{
    [CreateAssetMenu(menuName = "Battle/Action/Damage Target", fileName = "DamageTarget", order = 0)]
    public class DamageTarget : ScriptableBattleAction
    {
        private BattleEntity actor;
    
        public override void StartAction(BattleEntity actor, BattleEntity target)
        {
        
            if (!actor.TryGetComponent<StatBlockComponent>(out var actorStatBlockComponent))
            {
                Debug.LogError($"{actor.name} does not have a stat block component");
                return;
            }
            if (!target.TryGetComponent<StatBlockComponent>(out var targetStatBlockComponent))
            {
                Debug.LogError($"{target.name} does not have a stat block component");
                return;
            }
        
            Debug.Log($"{actor.name} damaged {target.name} for {actorStatBlockComponent.StatBlock.Attack.Value}");
            targetStatBlockComponent.StatBlock.Health.Add(-actorStatBlockComponent.StatBlock.Attack.Value);
        
            EndAction(actor); 
        }
    }
}