using EventBus;

namespace Battle.Events
{
    public struct AttackEntityEvent : IEvent
    {
        public IDamageSource Source;
        public BattleEntity Target;
        public int Damage; 
    }
}