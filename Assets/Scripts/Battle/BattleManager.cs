using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using RequestHub; 
using Battle.Events;
using Battle.Requests;
using Battle.TurnPhase;
using Cysharp.Threading.Tasks;
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
        private EventBinding<EntityStartTurnEvent> turnStartBinding;
        [SerializeField] private bool manageBattle;
        public BattleEntity ActiveEntity { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;
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
        
            turnEndBinding = new EventBinding<TurnEndEvent>(HandleTurnEnd);
            EventBus<TurnEndEvent>.Register(turnEndBinding);
        
            SetSortedTurns();
            NextTurn();
        }


        private void SetSortedTurns()
        {
            turnEntities.Clear();
            
            Dictionary<BattleEntity, int> entityBySpeed = new Dictionary<BattleEntity, int>();
            foreach (BattleEntity entity in BattleEntity.AllEntities)
            {
                if (RequestHub<RequestableSpeedValue>.TryRequest(entity, out var request))
                {
                    entityBySpeed.Add(entity, request.SpeedValue);
                    turnEntities.Add(entity); 
                } 
            }
            
            turnEntities = turnEntities.OrderByDescending(e => entityBySpeed[e]).ToList();
        }

        private void HandleTurnEnd(TurnEndEvent turnEndEvent) => EndTurn();

        public void EndTurn() => EndTurnAsync().Forget();
        private async UniTask EndTurnAsync()
        {
            await BattlePhaseManager.Instance.TransitionToPhaseAsync(BattlePhases.EndTurn); 
            NextTurn();
        }
        
        public void NextTurn() => NextTurnAsync().Forget();
        private async UniTask NextTurnAsync()
        {
            turnNumber++;
            int turnIndex = turnNumber % turnEntities.Count;

            await BattlePhaseManager.Instance.TransitionToPhaseAsync(BattlePhases.StartTurn); 
        
            ActiveEntity = turnEntities[turnIndex];
            EventBus<EntityStartTurnEvent>.Raise(new EntityStartTurnEvent {Entity = ActiveEntity});
        }

        private void OnDestroy()
        {
            Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
            EventBus<TurnEndEvent>.Deregister(turnEndBinding);
        }
    }
}