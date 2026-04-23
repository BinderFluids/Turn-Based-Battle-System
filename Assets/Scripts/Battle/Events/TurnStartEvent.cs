using EventBus;

namespace Battle.Events
{
    public struct TurnStartEvent : IEvent
    {
        public BattleEntity turnEntity; 
    }
}