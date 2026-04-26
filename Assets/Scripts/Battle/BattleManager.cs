using System.Collections.Generic;
using System.Linq;
using RequestHub; 
using Battle.Events;
using Battle.Requests;
using EventBus;
using Registry;
using UnityEngine;
using UnityUtils;

namespace Battle
{
    public class BattleManager : Singleton<BattleManager>
    {
        private int turnNumber = -1;
        [SerializeField] private BattleInputReader inputReader;
        [SerializeField] private List<BattleEntity> turnEntities = new();
        private EventBinding<TurnEndEvent> turnEndBinding;
        private EventBinding<TurnStartEvent> turnStartBinding;
        [SerializeField] private bool manageBattle;
        
        protected override void Awake()
        {
            base.Awake();
            
            inputReader.EnableInput(InputActionType.Player);
        }

        private void Start()
        {
            if (manageBattle)
                StartBattle();
        }

        private void StartBattle()
        {
            Registry<BattleEntity>._onItemAddedNoArgs += SetSortedTurns;
        
            turnEndBinding = new EventBinding<TurnEndEvent>(NextTurn);
            EventBus<TurnEndEvent>.Register(turnEndBinding);
        
            SetSortedTurns();
            NextTurn(default);
        }


        private void SetSortedTurns()
        {
            turnEntities.Clear();
            
            Dictionary<BattleEntity, int> entityBySpeed = new Dictionary<BattleEntity, int>();
            foreach (BattleEntity entity in BattleEntity.AllEntities)
            {
                if (RequestHub<RequestSpeedValue>.TryRequest(entity, out var request))
                {
                    entityBySpeed.Add(entity, request.SpeedValue);
                    turnEntities.Add(entity); 
                } 
            }
            
            turnEntities = turnEntities.OrderByDescending(e => entityBySpeed[e]).ToList();
        }
    
        private void NextTurn(TurnEndEvent turnEndEvent)
        {
            turnNumber++;
            int turnIndex = turnNumber % turnEntities.Count;
        
            BattleEntity turnComponent = turnEntities[turnIndex];
            EventBus<TurnStartEvent>.Raise(new TurnStartEvent {Entity = turnComponent});
        }

        private void OnDestroy()
        {
            Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
            EventBus<TurnEndEvent>.Deregister(turnEndBinding);
        }
    }
}