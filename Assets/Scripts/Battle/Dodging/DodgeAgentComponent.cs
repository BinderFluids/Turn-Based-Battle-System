using System;
using System.Collections.Generic;
using Battle.Enums;
using Battle.Events;
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

        private Dictionary<Type, IDodgeBehaviour> dodgeBehaviourCache = new(); 


        protected override void Start()
        {
            base.Start();
            DodgeDirector.Instance.AddDodgeAgent(this); 
        }

        private void Update()
        {
            if (!IsDodging) return;
            DodgeBehaviour.UpdateDodge(Entity);
        }

        public void StartDodge(IDodgeFactory factory)
        {
            IsDodging = true;
            
            if (dodgeBehaviourCache.TryGetValue(factory.GetType(), out var dodgeBehaviour))
                DodgeBehaviour = dodgeBehaviour;
            else
            {
                DodgeBehaviour = factory.GetDodgeBehaviour(); 
                dodgeBehaviourCache.Add(factory.GetType(), DodgeBehaviour);
            }
        }
        
        public void EndDodge()
        {
            IsDodging = false;
            DodgeBehaviour = null;
        }

        private void OnDestroy()
        {
            DodgeDirector.Instance.RemoveDodgeAgent(this); 
        }
    }
}