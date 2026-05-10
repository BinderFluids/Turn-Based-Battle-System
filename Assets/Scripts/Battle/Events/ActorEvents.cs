using Battle.Interfaces;
using EventBus;

namespace Battle.Events
{
    /// <summary>
    /// When raised, the ActorComponent with the corresponding Entity should begin their action selection method.
    /// </summary>
    public struct ActorChooseAction : IEvent
    {
        public BattleEntity Entity; 
    }

    /// <summary>
    /// This event is raised when an entity has selected their action
    /// </summary>
    public struct OnActionSelected : IEvent
    {
        public BattleEntity Entity;
        public IBattleAction Action;
    }

    /// <summary>
    /// This event is raised when an Actor has started its action
    /// </summary>
    public struct OnActionStarted : IEvent
    {
        public BattleEntity Entity;
        public IBattleAction Action;
    }
    
    /// <summary>
    /// This event is raised when an Actor has ended its action
    /// </summary>
    public struct OnActionEnded : IEvent
    {
        public BattleEntity Entity;
        public IBattleAction Action;
    }
}