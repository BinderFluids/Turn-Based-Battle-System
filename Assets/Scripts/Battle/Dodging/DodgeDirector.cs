using System.Collections.Generic;
using Battle.Interfaces;
using UnityUtils;

namespace Battle.Dodging
{
    public class DodgeDirector : Singleton<DodgeDirector>
    {
        private HashSet<DodgeAgentComponent> dodgeAgents;
        public IReadOnlyCollection<DodgeAgentComponent> DodgeAgents => dodgeAgents;

        public IDodgeBehaviour DodgeBehaviour { get; private set; }
        public void AddDodgeAgent(DodgeAgentComponent dodgeAgent) => dodgeAgents.Add(dodgeAgent);
        public void RemoveDodgeAgent(DodgeAgentComponent dodgeAgent) => dodgeAgents.Remove(dodgeAgent);
        
        
        public void StartDodge(IDodgeBehaviour behaviour)
        {
            DodgeBehaviour = behaviour;
            dodgeAgents.ForEach(dodgeAgent => dodgeAgent.StartDodge(DodgeBehaviour));
        }
        public void EndDodge()
        {
            DodgeBehaviour = null; 
            dodgeAgents.ForEach(dodgeAgent => dodgeAgent.EndDodge());
        }
    }
}