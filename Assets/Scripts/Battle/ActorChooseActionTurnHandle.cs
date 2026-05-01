using Battle.Enums;
using Battle.Events;
using Battle.Requests;
using RequestHub; 
using UnityEngine;
using EventBus; 

namespace Battle
{
    public class ActorChooseActionTurnHandle : ITurnHandleStrategy
    {
        private BattleEntity entity;
        private EventBinding<ActionEndedEvent> actionEndHandle; 
    
        public void Handle(BattleEntity entity)
        {
            this.entity = entity;
            
            if (!entity.TryGetComponent(ComponentType.Actor, out _))
            {
                Debug.LogWarning($"{entity.gameObject.name}: No action container component found on turn component");
                TurnEnd();
                return; 
            }

            actionEndHandle ??= new EventBinding<ActionEndedEvent>(ActionEnded);
            EventBus<ActionEndedEvent>.Register(actionEndHandle); 
            
            EventBus<ActorChooseActionEvent>.Raise(new ActorChooseActionEvent {Entity = entity});
        }

        void ActionEnded(ActionEndedEvent e)
        {
            if (e.Entity != entity) return; 
            EventBus<ActionEndedEvent>.Deregister(actionEndHandle);
            
            TurnEnd();
        }

        void TurnEnd()
        {
            TurnEndEvent turnEndEvent = new TurnEndEvent { turnEntity = entity };
            EventBus<TurnEndEvent>.Raise(turnEndEvent);
        }
    }

    public class NextTurnHandle : ITurnHandleStrategy
    {
        public void Handle(BattleEntity entity)
        {
        
        }
    }
}