
using EventBus;

namespace Battle.Events
{
    public struct TurnEndEvent : IEvent
    {
        public BattleEntity turnEntity; 
    }
}