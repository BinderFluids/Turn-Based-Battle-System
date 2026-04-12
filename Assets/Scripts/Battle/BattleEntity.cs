using System;
using System.Collections.Generic;
using System.Linq;
using Core.Stats;
using EventBus; 
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;
using StatusEffectSystem;

public class BattleEntity : MonoBehaviour, ISelectable
{
    private bool isActive;
    public bool IsActive => isActive;
    public PhysicalBattleEntityModifier physicalBattleEntityModifier;

    [SerializeField] public Vector3 topPosition;
    
    [SerializeField] private StatBlockComponent statBlockComponent;
    public StatBlock StatBlock => statBlockComponent.StatBlock;

    private List<StatusEffect> statuses = new();
    public IReadOnlyList<StatusEffect> Statuses => statuses;
    public void AddStatus(StatusEffect status)
    {
        if (statuses.Any(s => s.GetType() == status.GetType())) return; 
        
        statuses.Add(status);
        status.Apply(this); 
    }
    public void RemoveStatus(StatusEffect status)
    {
        if (!statuses.Contains(status)) return;
        if (status is null) return;
        
        statuses.Remove(status);
        status.Remove(this);
    }

    [SerializeField] private List<InterfaceReference<IBattleAction>> actionsRef;
    private List<IBattleAction> actions => actionsRef.Select(a => a.Value).ToList();
    private IBattleAction chosenAction;
    
    [SerializeField] private InterfaceReference<IBattleActionSelectionStrategy> 
        actionSelectionStrategyRef;
    [SerializeField] private InterfaceReference<IBattleEntitySelectionStrategy> 
        targetSelectionStrategyRef;
    IBattleActionSelectionStrategy actionSelectionStrategy => actionSelectionStrategyRef.Value;
    IBattleEntitySelectionStrategy targetSelectionStrategy => targetSelectionStrategyRef.Value;
    
    void Awake()
    {
        Registry<BattleEntity>.TryAdd(this); 
    }

    public async UniTaskVoid StartTurn()
    {
        if (Registry<BattleEntity>.All.Count(e => e.IsActive) > 0)
        {
            Debug.LogWarning($"{gameObject.name} tried to start a turn while another is active");
            return; 
        }

        isActive = true;
        Debug.Log($"{gameObject.name}: My turn just started!");

        if (actions.Count == 0 && gameObject.name != "Player")
        {
            NextTurn();
            return; 
        }
        
        actionSelectionStrategy.onActionSelected += OnActionSelected;
        actionSelectionStrategy.GetAction(actions);
    }
    void OnActionSelected(IBattleAction action)
    {
        print($"Selected action: {action}");
        
        chosenAction = action;
        actionSelectionStrategy.onActionSelected -= OnActionSelected;

        targetSelectionStrategy.onEntitySelected += OnTargetSelected; 
        targetSelectionStrategy.GetEntity(this, action); 
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
        action.onActionEnded += OnActionEnded;
        action.StartAction(this, target);
    }
    void OnActionEnded()
    {
        chosenAction.onActionEnded -= OnActionEnded;
        NextTurn();
    }

    void NextTurn()
    {
        isActive = false;
        TurnEndEvent turnEndEvent = new TurnEndEvent {entity = this};
        EventBus<TurnEndEvent>.Raise(turnEndEvent);
    }

    public void AddHealth(int amt)
    {
        Debug.LogWarning($"Adding {amt} health to {gameObject.name}");
        StatBlock.Health.Add(amt); 
    }
    
    public void CancelTargetSelection()
    {
        Debug.LogWarning("Cancel target selection currently does nothing!");
        //Debug.Log($"Cancelling target selection for {gameObject.name}");
    }
    private void OnDestroy()
    {
        Registry<BattleEntity>.Remove(this); 
    }

    //ISelection Implementation
    public event Action OnSelected;
    public void Select()
    {
        //throw new NotImplementedException();
    }
}