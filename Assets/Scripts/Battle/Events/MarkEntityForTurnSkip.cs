using EventBus;

namespace Battle.Events
{
    public struct MarkEntityForTurnSkip : IEvent
    {
        public BattleEntity Entity;
    }
}