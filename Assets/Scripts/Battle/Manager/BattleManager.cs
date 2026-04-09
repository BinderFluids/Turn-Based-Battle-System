using System;
using System.Collections.Generic;
using Registry;
using UnityEngine;
using UnityUtils;
using System.Linq;
using EventBus; 

public class BattleManager : Singleton<BattleManager>
{
    private int turnNumber = -1;
    [SerializeField] private List<BattleEntity> turnSortedEntities = new();
    private EventBinding<NextTurnEvent> nextTurnBinding;
    [SerializeField] private bool manageBattle;

    private void Start()
    {
        if (manageBattle)
            StartBattle();
    }

    private void StartBattle()
    {
        Registry<BattleEntity>._onItemAddedNoArgs += SetSortedTurns;

        nextTurnBinding = new EventBinding<NextTurnEvent>(NextTurn);
        EventBus<NextTurnEvent>.Register(nextTurnBinding);
        
        SetSortedTurns();
        NextTurn(default);
    }

    private void SetSortedTurns()
    {
        turnSortedEntities = Registry<BattleEntity>
            .All
            .OrderBy(e => 1f / e.statBlock.Speed.GetValue())
            .ToList();
    }
    
    private void NextTurn(NextTurnEvent nextTurnEvent)
    {
        turnNumber++;
        int turnIndex = turnNumber % turnSortedEntities.Count;
        
        BattleEntity entity = turnSortedEntities[turnIndex];
        entity.StartTurn().Forget();
    }

    private void OnDestroy()
    {
        Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
        EventBus<NextTurnEvent>.Deregister(nextTurnBinding);
    }
}