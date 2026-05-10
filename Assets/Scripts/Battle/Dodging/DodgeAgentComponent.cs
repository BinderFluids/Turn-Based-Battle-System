using System;
using Battle.Enums;
using Battle.Interfaces;
using SerializedInterface;
using UnityEngine;

namespace Battle.Dodging
{
    public class DodgeAgentComponent : BattleEntityComponent
    {
        protected override ComponentType componentType => ComponentType.DodgeAgent;

        public bool IsDodging { get; private set; }
        public IDodgeBehaviour DodgeBehaviour { get; private set; }

        private void Update()
        {
            if (!IsDodging) return;
            DodgeBehaviour.Update();
        }

        public void StartDodge(IDodgeBehaviour dodgeBehaviour)
        {
            IsDodging = true;
            DodgeBehaviour = dodgeBehaviour;
        }
        
        public void EndDodge()
        {
            IsDodging = false;
            DodgeBehaviour = null;
        }
    }
}