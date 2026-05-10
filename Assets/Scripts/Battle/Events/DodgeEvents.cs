using Battle.Interfaces;
using EventBus;

namespace Battle.Events
{
    public struct StartDodge<TDodge> : IEvent where TDodge : IDodgeBehaviour
    {
        public TDodge DodgeBehaviour;
        
        public StartDodge(TDodge dodgeBehaviour)
        {
            DodgeBehaviour = dodgeBehaviour;
        }
    }
}