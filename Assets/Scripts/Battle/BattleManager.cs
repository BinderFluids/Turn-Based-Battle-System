using System;
using System.Collections.Generic;
using Registry;
using UnityEngine;
using UnityUtils;
using System.Linq;
using EventBus;
using StatusEffectSystem;

public class BattleManager : Singleton<BattleManager>
{
    private int turnNumber = -1;
    [SerializeField] private List<BattleEntity> turnSortedEntities = new();
    private EventBinding<TurnEndEvent> turnEndBinding;
    private EventBinding<TurnStartEvent> turnStartBinding;
    [SerializeField] private bool manageBattle;
    
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
        turnSortedEntities = Registry<BattleEntity>
            .All
            .OrderBy(e => 1f / e.StatBlock.Speed.Value)
            .ToList();
    }
    
    private void NextTurn(TurnEndEvent turnEndEvent)
    {
        turnNumber++;
        int turnIndex = turnNumber % turnSortedEntities.Count;
        
        BattleEntity entity = turnSortedEntities[turnIndex];
        
        EventBus<TurnStartEvent>.Raise(new TurnStartEvent {entity = entity});
        entity.StartTurn().Forget();
    }

    private void OnDestroy()
    {
        Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
        EventBus<TurnEndEvent>.Deregister(turnEndBinding);
    }
}