using System;
using Battle.Components;
using Core.Enums;
using UnityEngine;
using Battle.Requests; 
using RequestHub;

namespace Battle.Components
{
    public class PlayerEntityComponent : BattleEntityComponent
    {
        [SerializeField] private PlayerId playerID;
        public PlayerId PlayerID => playerID;
        [SerializeField] private BattleInputReader input; 
        private BoolInputData inputData;
        public BoolInputData InputData => inputData;

        protected override void Start()
        {
            base.Start();
            inputData = playerID switch
            {
                PlayerId.PlayerOne => input.PlayerOne,
                PlayerId.PlayerTwo => input.PlayerTwo,
                _ => throw new NotImplementedException()
            };

            RequestHub<RequestPlayerId>.Register(Entity, () => new RequestPlayerId { PlayerId = playerID });
        }

        private void OnDestroy()
        {
            RequestHub<RequestPlayerId>.Deregister(Entity);
        }
    }
}
