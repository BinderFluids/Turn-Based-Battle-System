using System.Collections.Generic;
using System.Linq;
using Battle.Events;
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

        private EventBinding<ReturnTurnEntityEvent> returnTurnEntityBinding;
        
        protected override void Awake()
        {
            base.Awake();
            
            inputReader.EnableInput(InputActionType.Player);
            
            returnTurnEntityBinding = new EventBinding<ReturnTurnEntityEvent>(OnReturnTurnEntityEventRaised);
            EventBus<ReturnTurnEntityEvent>.Register(returnTurnEntityBinding);
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

        void OnReturnTurnEntityEventRaised(ReturnTurnEntityEvent e)
        {
            turnEntities.Add(e.entity);
        }

        private void SetSortedTurns()
        {
            
        }
    
        private void NextTurn(TurnEndEvent turnEndEvent)
        {
            turnNumber++;
            int turnIndex = turnNumber % turnEntities.Count;
        
            BattleEntity turnComponent = turnEntities[turnIndex];
            EventBus<TurnStartEvent>.Raise(new TurnStartEvent {turnEntity = turnComponent});
        }

        private void OnDestroy()
        {
            Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
            EventBus<ReturnTurnEntityEvent>.Deregister(returnTurnEntityBinding);
            EventBus<TurnEndEvent>.Deregister(turnEndBinding);
        }
    }
}