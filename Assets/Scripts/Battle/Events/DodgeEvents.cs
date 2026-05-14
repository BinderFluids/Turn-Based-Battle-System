using Battle.Interfaces;
using EventBus;

namespace Battle.Events
{
    public struct StartDodgePhase : IEvent  
    {
        public IDodgeFactory DodgeFactory;
        
        public StartDodgePhase(IDodgeFactory dodgeFactory)
        {
            DodgeFactory = dodgeFactory;
        }
    }
    
    public struct DodgePhaseEnded : IEvent
    {
        public IDodgeFactory DodgeFactory;
        
        public DodgePhaseEnded(IDodgeFactory dodgeFactory)
        {
            DodgeFactory = dodgeFactory;
        }

    }
}