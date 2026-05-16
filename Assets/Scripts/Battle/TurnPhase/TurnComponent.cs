using Core; 
using Battle.Enums;
using Battle.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;
using EventBus;
using SerializedInterface;

namespace Battle.TurnPhase
{
    public class TurnComponent : BattleEntityComponent
    {
        public bool TakeTurn => takeTurn; 
        [SerializeField] private bool takeTurn;
        
        public bool MarkedShouldSkipTurn => markedShouldSkipTurn;
        [SerializeField] private bool markedShouldSkipTurn; 
        
        [SerializeField, Tooltip("Will default if empty")] private InterfaceReference<ITurnHandleStrategy> turnStartHandle; 
        [SerializeField, Tooltip("Will default if empty")] private InterfaceReference<ITurnHandleStrategy> turnEndHandle;
    
        private EventBinding<EntityStartTurnEvent> turnStartBinding;
        private EventBinding<TurnEndEvent> turnEndBinding;
        private EventBinding<MarkEntityForTurnSkip> cancelTurnBinding;
        
        private EventBinding<AddPreTurnTask> addPreTurnTaskBinding;
        private EventBinding<QueuePreTurnCommand> queuePreTurnCommandBinding;
        private EventBinding<AddPostTurnTask> addPostTurnTaskBinding;
        private EventBinding<QueuePostTurnCommand> queuePostTurnCommandBinding;
    
        private ITurnHandleStrategy defaultTurnStartHandle = new ActorChooseActionTurnHandle();
        private ITurnHandleStrategy defaultTurnEndHandle = new EmptyTurnHandle();

        protected override ComponentType componentType => ComponentType.Turn; 
        
        private Transition preTurnTransition = new();
        private Transition postTurnTransition = new();
        
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

            cancelTurnBinding = new EventBinding<MarkEntityForTurnSkip>(() => markedShouldSkipTurn = true); 
            EventBus<MarkEntityForTurnSkip>.Register(cancelTurnBinding);
            
            addPreTurnTaskBinding = new EventBinding<AddPreTurnTask>(e =>
            {
                if (e.Entity == Entity)
                    AddPendingTask(preTurnTransition, e.Task);
            });
            EventBus<AddPreTurnTask>.Register(addPreTurnTaskBinding);

            queuePreTurnCommandBinding = new EventBinding<QueuePreTurnCommand>(e =>
            {
                if (e.Entity == Entity)
                    QueueCommand(preTurnTransition, e.Command);
            });
            EventBus<QueuePreTurnCommand>.Register(queuePreTurnCommandBinding);
            
            addPostTurnTaskBinding = new EventBinding<AddPostTurnTask>(e =>
            {
                if (e.Entity == Entity)
                    AddPendingTask(postTurnTransition, e.Task);
            });
            EventBus<AddPostTurnTask>.Register(addPostTurnTaskBinding);
            
            queuePostTurnCommandBinding = new EventBinding<QueuePostTurnCommand>(e =>
            {
                if (e.Entity == Entity)
                    QueueCommand(postTurnTransition, e.Command);
            });
            EventBus<QueuePostTurnCommand>.Register(queuePostTurnCommandBinding);
        }
        
        public void AddPreTurnPendingTask(UniTask task) => AddPendingTask(preTurnTransition, task);
        public void QueuePreTurnCommand(ICommand command) => QueueCommand(preTurnTransition, command);
        public void AddPostTurnPendingTask(UniTask task) => AddPendingTask(postTurnTransition, task);
        public void QueuePostTurnCommand(ICommand command) => QueueCommand(postTurnTransition, command);
        void AddPendingTask(Transition transition, UniTask task) => transition.AddPendingTask(task);
        void QueueCommand(Transition transition, ICommand command) => transition.QueueCommand(command);

        void DeregisterEventBindings()
        {
            EventBus<EntityStartTurnEvent>.Deregister(turnStartBinding);
            EventBus<TurnEndEvent>.Deregister(turnEndBinding);
            EventBus<MarkEntityForTurnSkip>.Deregister(cancelTurnBinding);
            EventBus<AddPreTurnTask>.Deregister(addPreTurnTaskBinding);
            EventBus<QueuePreTurnCommand>.Deregister(queuePreTurnCommandBinding);
            EventBus<AddPostTurnTask>.Deregister(addPostTurnTaskBinding);
            EventBus<QueuePostTurnCommand>.Deregister(queuePostTurnCommandBinding);
        }
        
        
        void HandleTurnStart(EntityStartTurnEvent e)
        {
            if (!takeTurn) return; 
            if (e.Entity != Entity) return;
            StartTurn().Forget();
        } 
        public async UniTask StartTurn()
        {
            if (preTurnTransition.PendingTasks.Count > 0)
                Debug.Log($"{gameObject.name}: There are pending tasks to be completed before the turn starts, waiting..");
            if (preTurnTransition.QueuedCommands.Count > 0)
                Debug.Log($"{gameObject.name}: There are still commands that need to be executed");
            
            await preTurnTransition.TransitionAsync(); 
            
            if (markedShouldSkipTurn)
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
        
        public async UniTask EndTurn()
        {
            
            {
                await postTurnTransition.TransitionAsync();
            }
            
            markedShouldSkipTurn = false; 
            
            if (turnEndHandle.UnderlyingValue == null)
            {
                Debug.LogWarning($"{gameObject.name}: TurnEndHandle is null, using default: {defaultTurnEndHandle.GetType().Name}");
                defaultTurnEndHandle.Handle(Entity);
            }
            else
                turnEndHandle.Value.Handle(Entity);
        }
        
        void HandleTurnEnd(TurnEndEvent e)
        {
            if (!takeTurn) return; 
            if (e.turnEntity != Entity) return;
            EndTurn().Forget(); 
        }

        private void OnDestroy()
        {
            DeregisterEventBindings();
        }
    }
}