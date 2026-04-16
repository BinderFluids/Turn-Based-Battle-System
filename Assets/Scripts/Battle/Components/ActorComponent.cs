using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ActorComponent : BattleEntityComponent
{
    [SerializeField] private InterfaceReference<IBattleActionSelectionStrategy> selectActionStrategyRef; 
    private IBattleActionSelectionStrategy actionSelectionStrategy => selectActionStrategyRef.Value;
    
    [SerializeField] private InterfaceReference<IBattleEntitySelectionStrategy> targetSelectionStrategyRef; 
    private IBattleEntitySelectionStrategy targetSelectionStrategy => targetSelectionStrategyRef.Value;
    
    [SerializeField] private List<InterfaceReference<IBattleAction>> actionsRef;
    public IReadOnlyList<IBattleAction> Actions => actionsRef.Select(a => a.Value).ToList();
    private IBattleAction chosenAction;


    public event Action onActionStarted = delegate {}; 
    public event Action onActionEnded = delegate { };

    public void SelectAction()
    {
        actionSelectionStrategy.onActionSelected += OnActionSelected;
        actionSelectionStrategy.GetAction(Actions);
    }
    
    void OnActionSelected(IBattleAction action)
    {
        chosenAction = action;
        actionSelectionStrategy.onActionSelected -= OnActionSelected;
        
        targetSelectionStrategy.onEntitySelected += OnTargetSelected; 
        targetSelectionStrategy.GetEntity(Entity, action); 
    }

    
    void OnTargetSelected(BattleEntity target)
    {
        print($"Selected target: {target}");
        
        targetSelectionStrategy.onEntitySelected -= OnTargetSelected; 
        StartAction(chosenAction, target);
    }
    
    public void StartAction(IBattleAction action, BattleEntity target)
    {
        if (action is null)
        {
            Debug.LogError($"Tried to start action on {gameObject.name} with null action");
            return;
        }

        if (target is null)
        {
            Debug.LogError($"Tried to start action on {gameObject.name} with null target");
            return;
        }
        
        chosenAction = action; 
        chosenAction.onActionEnded += OnActionEnded;
        
        chosenAction.StartAction(Entity, target);
    }
    void OnActionEnded()
    {
        chosenAction.onActionEnded -= OnActionEnded;
        onActionEnded?.Invoke();
    }
}