using EventBus;

namespace Battle.Events
{
    public struct RequestTurnEntitiesEvent : IEvent { }

    public struct ReturnTurnEntityEvent : IEvent
    {
        public BattleEntity entity;
    }
}