using System;
using System.Collections.Generic;
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

    public class JumpDodge : IDodgeBehaviour
    {
        private UniTask jumpTask; 
        
        public void UpdateDodge(BattleEntity entity)
        {
            if (!RequestHub<RequestPlayerId>.TryRequest(entity, out var request)) return;

            if (BattleUtils.PlayerInputData.GetInputActionByPlayerID(request.PlayerId).WasPressedThisFrame())
            {
                if (jumpTask.Status.IsCompleted())
                    jumpTask = Jump(entity); 
            }
        }

        async UniTask Jump(BattleEntity entity)
        {
            await entity.Jump(entity.Transform.position, 2f, 1, EntityMotionType.Duration); 
        }
    }
}