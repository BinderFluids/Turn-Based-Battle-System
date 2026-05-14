using System;
using System.Collections.Generic;
using Battle.Events;
using Battle.Interfaces;
using EventBus;
using UnityEditor;
using UnityUtils;

namespace Battle.Dodging
{
    public class DodgeDirector : Singleton<DodgeDirector>
    {
        private HashSet<DodgeAgentComponent> dodgeAgents = new();
        public IReadOnlyCollection<DodgeAgentComponent> DodgeAgents => dodgeAgents;

        public void AddDodgeAgent(DodgeAgentComponent dodgeAgent) => dodgeAgents.Add(dodgeAgent);
        public void RemoveDodgeAgent(DodgeAgentComponent dodgeAgent) => dodgeAgents.Remove(dodgeAgent);

        private EventBinding<StartDodgePhase> startDodgePhaseBinding;
        private EventBinding<OnActionEnded> actionEndBinding;
        
        public IDodgeFactory ActiveDodgeFactory { get; private set; } 
        
        private void Start()
        {
            startDodgePhaseBinding = new EventBinding<StartDodgePhase>(e => StartDodge(e.DodgeFactory));
            EventBus<StartDodgePhase>.Register(startDodgePhaseBinding);
            
            actionEndBinding = new EventBinding<OnActionEnded>(e => EndDodge());
            EventBus<OnActionEnded>.Register(actionEndBinding);
        }
        
        
        public void StartDodge(IDodgeFactory factory)
        {
            ActiveDodgeFactory = factory;
            dodgeAgents.ForEach(dodgeAgent => dodgeAgent.StartDodge(factory));
        }
        public void EndDodge()
        {
            dodgeAgents.ForEach(dodgeAgent => dodgeAgent.EndDodge());
            
            EventBus<DodgePhaseEnded>.Raise(new DodgePhaseEnded(ActiveDodgeFactory));
            ActiveDodgeFactory = null; 
        }

        private void OnDestroy()
        {
            EventBus<StartDodgePhase>.Deregister(startDodgePhaseBinding);
            EventBus<OnActionEnded>.Deregister(actionEndBinding);
        }
    }
}