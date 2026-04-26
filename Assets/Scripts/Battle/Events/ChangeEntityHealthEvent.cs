using EventBus;

namespace Battle.Events
{
    public struct ChangeEntityHealthEvent : IEvent
    {
        public IDamageSource Source;
        public BattleEntity Target;
        public int Damage; 
    }
}