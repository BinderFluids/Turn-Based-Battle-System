using System;
using Battle.Enums;
using Battle.Events;
using Battle.Interfaces;
using Battle.Requests;
using Cysharp.Threading.Tasks;
using SerializedInterface;
using UnityEngine;
using RequestHub; 

namespace Battle.Dodging
{
    public class DodgeAgentComponent : BattleEntityComponent
    {
        protected override ComponentType componentType => ComponentType.DodgeAgent;

        public bool IsDodging { get; private set; }
        public IDodgeBehaviour DodgeBehaviour { get; private set; }


        protected override void Start()
        {
            base.Start();
            DodgeDirector.Instance.AddDodgeAgent(this); 
        }

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

        private void OnDestroy()
        {
            DodgeDirector.Instance.RemoveDodgeAgent(this); 
        }
    }

    public class JumpDodge : IDodgeBehaviour
    {
        private DodgeAgentComponent agent;
        
        public void Update()
        {
            if (!RequestHub<RequestPlayerId>.TryRequest(agent.Entity, out var request)) return;

            if (BattleUtils.PlayerInputData.GetInputActionByPlayerID(request.PlayerId).WasPressedThisFrame())
                Jump(agent.Entity);
        }

        async UniTaskVoid Jump(BattleEntity entity)
        {
            Vector3 startPos = entity.Transform.position;
            
            //await entity.MoveTo()
        }
    }
}