using Battle.Enums;
using EventBus;

namespace Battle.Events
{
    public struct TransitionToPhase : IEvent
    {
        public BattlePhases Phase; 
        
        public TransitionToPhase(BattlePhases phase)
        {
            Phase = phase;
        }
    }
    
    /// <summary>
    /// Raised after a phase has been transitioned to
    /// </summary>
    public struct PhaseStarted : IEvent
    {
        public BattlePhases Phase; 
        
        public PhaseStarted(BattlePhases phase)
        {
            Phase = phase;
        }
    }
    
    /// <summary>
    /// Raised as soon as a new phase has been called to transition
    /// </summary>
    public struct PhaseEnded : IEvent
    {
        public BattlePhases Phase;
        
        public PhaseEnded(BattlePhases phase)
        {
            Phase = phase;
        }
    }
}