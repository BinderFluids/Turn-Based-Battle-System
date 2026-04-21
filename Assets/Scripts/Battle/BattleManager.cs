using System.Collections.Generic;
using System.Linq;
using EventBus;
using Registry;
using UnityEngine;
using UnityUtils;

public class BattleManager : Singleton<BattleManager>
{
    private int turnNumber = -1;
    [SerializeField] private BattleInputReader inputReader;
    [SerializeField] private List<TurnComponent> turnEntities = new();
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
        turnEntities = FindObjectsByType<TurnComponent>()
            .OrderBy(e => 1f / e.StatBlock.Speed.Value)
            .ToList();
    }
    
    private void NextTurn(TurnEndEvent turnEndEvent)
    {
        turnNumber++;
        int turnIndex = turnNumber % turnEntities.Count;
        
        TurnComponent turnComponent = turnEntities[turnIndex];
        
        EventBus<TurnStartEvent>.Raise(new TurnStartEvent {turnEntity = turnComponent});
    }

    private void OnDestroy()
    {
        Registry<BattleEntity>._onItemAddedNoArgs -= SetSortedTurns;
        EventBus<TurnEndEvent>.Deregister(turnEndBinding);
    }
}