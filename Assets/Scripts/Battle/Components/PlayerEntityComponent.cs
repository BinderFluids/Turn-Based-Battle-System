using System;
using Battle.Enums;
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


        protected override ComponentType componentType => ComponentType.Player;
        
        protected override void Start()
        {
            base.Start();
            inputData = playerID switch
            {
                PlayerId.PlayerOne => input.PlayerOne,
                PlayerId.PlayerTwo => input.PlayerTwo,
                _ => throw new NotImplementedException()
            };

            RequestHub<RequestablePlayerId>.Register(Entity, () => new RequestablePlayerId { PlayerId = playerID });
        }

        private void OnDestroy()
        {
            RequestHub<RequestablePlayerId>.Deregister(Entity);
        }
    }
}
