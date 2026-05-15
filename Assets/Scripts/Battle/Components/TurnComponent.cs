using Battle.Enums;
using Battle.Events;
using Battle.Requests;
using UnityEngine;
using EventBus;
using SerializedInterface;
using RequestHub; 

namespace Battle.Components
{
    [RequireComponent(typeof(StatBlockComponent))]
    public class TurnComponent : BattleEntityComponent
    {

        [SerializeField] private bool takeTurn;
        private bool shouldSkipTurn; 
        
        public bool TakeTurn => takeTurn; 
        [SerializeField, Tooltip("Will default if empty")] private InterfaceReference<ITurnHandleStrategy> turnStartHandle; 
        [SerializeField, Tooltip("Will default if empty")] private InterfaceReference<ITurnHandleStrategy> turnEndHandle;
    
        private EventBinding<EntityStartTurnEvent> turnStartBinding;
        private EventBinding<TurnEndEvent> turnEndBinding;
        private EventBinding<CancelEntityTurn> cancelTurnBinding;
    
        private ITurnHandleStrategy defaultTurnStartHandle = new ActorChooseActionTurnHandle();
        private ITurnHandleStrategy defaultTurnEndHandle = new EmptyTurnHandle();

        protected override ComponentType componentType => ComponentType.Turn; 
        
        protected override void Awake()
        {
            base.Awake();
            RegisterEventBindings();
        }

        void RegisterEventBindings()
        {
            turnStartBinding = new EventBinding<EntityStartTurnEvent>(HandleTurnStart);
            EventBus<EntityStartTurnEvent>.Register(turnStartBinding);
        
            turnEndBinding = new EventBinding<TurnEndEvent>(HandleTurnEnd);
            EventBus<TurnEndEvent>.Register(turnEndBinding);

            cancelTurnBinding = new EventBinding<CancelEntityTurn>(() => shouldSkipTurn = true); 
            EventBus<CancelEntityTurn>.Register(cancelTurnBinding);
        }

        void DeregisterEventBindings()
        {
            EventBus<EntityStartTurnEvent>.Deregister(turnStartBinding);
            EventBus<TurnEndEvent>.Deregister(turnEndBinding);
            EventBus<CancelEntityTurn>.Deregister(cancelTurnBinding);
        }
        
        public void StartTurn()
        {
            if (shouldSkipTurn)
            {
                HandleTurnEnd(new TurnEndEvent {turnEntity = Entity});
                return;
            }
            
            if (turnStartHandle.UnderlyingValue == null)
            {
                Debug.LogWarning($"{gameObject.name}: TurnStartHandle is null, using default: {defaultTurnStartHandle.GetType().Name}");
                defaultTurnStartHandle.Handle(Entity);
            }
            else
                turnStartHandle.Value.Handle(Entity);
        }
    
        void HandleTurnStart(EntityStartTurnEvent e)
        {
            if (!takeTurn) return; 
            if (e.Entity != Entity) return;
            StartTurn();
        } 
        void HandleTurnEnd(TurnEndEvent e)
        {
            if (!takeTurn) return; 
            if (e.turnEntity != Entity) return;

            shouldSkipTurn = false; 
            
            if (turnEndHandle.UnderlyingValue == null)
            {
                Debug.LogWarning($"{gameObject.name}: TurnEndHandle is null, using default: {defaultTurnEndHandle.GetType().Name}");
                defaultTurnEndHandle.Handle(Entity);
            }
            else
                turnEndHandle.Value.Handle(Entity);
        }

        private void OnDestroy()
        {
            DeregisterEventBindings();
        }
    }
}