using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;

public enum PhysicalBattleEntityModifier
{
    Flying,
    SpikyTop,
    Underground,
    None
}

public class BattleEntity : MonoBehaviour, ISelectable
{
    public int Strength;
    public int Speed;
    private bool isActive;
    public bool IsActive => isActive;
    public PhysicalBattleEntityModifier physicalBattleEntityModifier;

    [SerializeField] public Vector3 topPosition;
    
    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

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
        Debug.Log($"Starting turn for {gameObject.name}");

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
        chosenAction.onActionEnded += OnActionEnded;
        chosenAction.StartAction(this, target);
    }

    void OnActionEnded()
    {
        chosenAction.onActionEnded -= OnActionEnded;
        isActive = false;
        
        
        NextTurnEvent nextTurnEvent = new NextTurnEvent {previousActor = this};
        EventBus<NextTurnEvent>.Raise(nextTurnEvent);
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