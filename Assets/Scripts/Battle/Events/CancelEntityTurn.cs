using EventBus;

namespace Battle.Events
{
    public struct CancelEntityTurn : IEvent
    {
        public BattleEntity Entity;
    }
}