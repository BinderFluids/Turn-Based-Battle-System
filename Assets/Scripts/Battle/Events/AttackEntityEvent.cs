using EventBus;

namespace Battle.Events
{
    public struct AttackEntityEvent : IEvent
    {
        public BattleEntity Target;
        public BattleEntity Source;
        public int Damage; 
    }
}