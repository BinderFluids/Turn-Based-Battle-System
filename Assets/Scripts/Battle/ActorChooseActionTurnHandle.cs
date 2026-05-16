using Battle.Enums;
using Battle.Events;
using Battle.Requests;
using Battle.Phase;
using Cysharp.Threading.Tasks;
using RequestHub; 
using UnityEngine;
using EventBus; 

namespace Battle
{
    public class ActorChooseActionTurnHandle : ITurnHandleStrategy
    {
        private BattleEntity entity;
        private EventBinding<OnActionEnded> actionEndHandle; 
    
        public void Handle(BattleEntity entity)
        {
            this.entity = entity;
            
            if (!entity.TryGetComponent(ComponentType.Actor, out _))
            {
                Debug.LogWarning($"{entity.gameObject.name}: No action container component found on turn component");
                TurnEnd();
                return; 
            }

            actionEndHandle ??= new EventBinding<OnActionEnded>(ActionEnded);
            EventBus<OnActionEnded>.Register(actionEndHandle);

            BattlePhaseManager.Instance.TransitionToPhase(BattlePhases.SelectingAction); 
        }

        void ActionEnded(OnActionEnded e)
        {
            if (e.Entity != entity) return; 
            EventBus<OnActionEnded>.Deregister(actionEndHandle);
            
            TurnEnd();
        }

        void TurnEnd()
        {
            Debug.Log($"{entity.gameObject.name}: Turn ended");
            
            TurnEndEvent turnEndEvent = new TurnEndEvent { turnEntity = entity };
            EventBus<TurnEndEvent>.Raise(turnEndEvent);
        }
    }
}