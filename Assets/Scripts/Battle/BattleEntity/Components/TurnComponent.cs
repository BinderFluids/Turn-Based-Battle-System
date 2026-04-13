using System;
using System.Runtime.CompilerServices;
using Battle.BattleEntity;
using Core.Stats;
using EventBus;
using UnityEngine;

public class TurnComponent : BattleEntityComponent
{
    [SerializeField] private StatBlockComponent statBlockComponent;
    public StatBlock StatBlock => statBlockComponent.StatBlock;
    
    [SerializeField] private InterfaceReference<ITurnHandleStrategy> turnStartHandle; 
    [SerializeField] private InterfaceReference<ITurnHandleStrategy> turnEndHandle;
    
    private EventBinding<TurnStartEvent> turnStartBinding;
    private EventBinding<TurnEndEvent> turnEndBinding;
    
    private ITurnHandleStrategy defaultTurnStartHandle = new SelectActionTurnHandle();
    private ITurnHandleStrategy defaultTurnEndHandle = new EmptyTurnHandle();
    
    private void Awake()
    {
        turnStartBinding = new EventBinding<TurnStartEvent>(HandleTurnStart);
        EventBus<TurnStartEvent>.Register(turnStartBinding);
        
        turnEndBinding = new EventBinding<TurnEndEvent>(HandleTurnEnd);
        EventBus<TurnEndEvent>.Register(turnEndBinding);
    }

    public void StartTurn()
    {
        if (turnStartHandle.UnderlyingValue == null)
        {
            Debug.LogWarning($"{gameObject.name}: TurnStartHandle is null, using default: {defaultTurnStartHandle.GetType().Name}");
            defaultTurnStartHandle.Handle(this);
        }
        else
            turnStartHandle.Value.Handle(this);
    }
    
    void HandleTurnStart(TurnStartEvent e)
    {
        if (e.turnEntity != this) return;
        StartTurn();
    } 
    void HandleTurnEnd(TurnEndEvent e)
    {
        if (e.turnEntity != this) return;
        
        if (turnEndHandle.UnderlyingValue == null)
        {
            Debug.LogWarning($"{gameObject.name}: TurnEndHandle is null, using default: {defaultTurnEndHandle.GetType().Name}");
            defaultTurnEndHandle.Handle(this);
        }
        else
            turnEndHandle.Value.Handle(this);
    }

    private void OnDestroy()
    {
        EventBus<TurnStartEvent>.Deregister(turnStartBinding);
        EventBus<TurnEndEvent>.Deregister(turnEndBinding);
    }
}