using EventBus;
using UnityEngine;
using UnityUtils;

namespace StatusEffectSystem
{
    public class StatusEffectHandler 
    {
        EventBinding<TurnStartEvent> turnStartBinding;
        
        public StatusEffectHandler()
        {
            turnStartBinding = new EventBinding<TurnStartEvent>(OnTurnStart);
            EventBus<TurnStartEvent>.Register(turnStartBinding);
        }

        void OnTurnStart(TurnStartEvent turnStartEvent)
        {
            Debug.Log($"Status Effect Handler Received TurnStartEvent. It is {turnStartEvent.entity.gameObject.name}'s turn.");
            
            BattleEntity entity = turnStartEvent.entity;
            for (int i = entity.Statuses.Count - 1; i >= 0; i--)
            {
                entity.Statuses[i].TurnStart(entity);   
            }
        }
    }
}