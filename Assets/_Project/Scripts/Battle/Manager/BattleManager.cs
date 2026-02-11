using System;
using System.Collections.Generic;
using Registry;
using UnityEngine;
using UnityUtils;
using System.Linq;

public class BattleManager : Singleton<BattleManager>
{
    private int turnNumber = -1;
    private List<BattleEntity> turnSortedEntities = new();
    private EventBinding<NextTurnEvent> nextTurnBinding;

    private void Start()
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
            .OrderBy(e => e.Speed)
            .ToList();
    }
    
    private void NextTurn(NextTurnEvent nextTurnEvent)
    {
        turnNumber++;
        int turnIndex = turnNumber % turnSortedEntities.Count;
        
        BattleEntity entity = turnSortedEntities[turnIndex];
        entity.StartTurn();
    }

    private void OnDestroy()
    {
        Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
        EventBus<NextTurnEvent>.Deregister(nextTurnBinding);
    }
}