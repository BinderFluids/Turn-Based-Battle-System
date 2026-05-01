using Battle.Interfaces;
using EventBus;

namespace Battle.Events
{
    /// <summary>
    /// When raised, the ActorComponent with the corresponding Entity should begin their action selection method.
    /// </summary>
    public struct ActorChooseActionEvent : IEvent
    {
        public BattleEntity Entity; 
    }

    /// <summary>
    /// This event is raised when an entity has selected their action
    /// </summary>
    public struct ActionWasSelectedEvent : IEvent
    {
        public BattleEntity Entity;
        public IBattleAction Action;
    }

    /// <summary>
    /// This event is raised when an Actor has started its action
    /// </summary>
    public struct ActionStartedEvent : IEvent
    {
        public BattleEntity Entity;
        public IBattleAction Action;
    }
    
    /// <summary>
    /// This event is raised when an Actor has ended its action
    /// </summary>
    public struct ActionEndedEvent : IEvent
    {
        public BattleEntity Entity;
        public IBattleAction Action;
    }
}