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
    
    [SerializeField] private InterfaceReference<IBattleActionSelectionStrategy> 
        actionSelectionStrategyRef;
    [SerializeField] private InterfaceReference<IBattleEntitySelectionStrategy> 
        targetSelectionStrategyRef;
    
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

        IBattleAction action = await AwaitActionSelection();
        await AwaitTargetSelection(action);
        
        isActive = false;
    }

    async UniTask<IBattleAction> AwaitActionSelection()
    {
        return await actionSelectionStrategyRef.Value
            .GetAction(actions);
    }

    async UniTask AwaitTargetSelection(IBattleAction action)
    {
        targetSelectionCancellationTokenSource = new CancellationTokenSource();
        try
        {
            BattleEntity target = await targetSelectionStrategyRef.Value
                .GetEntity(this, action, targetSelectionCancellationTokenSource.Token);
            
            action.Strategy(this, target).Forget();
        }
        catch (OperationCanceledException e)
        {
            Debug.Log($"Target selection for {gameObject.name} was cancelled");
            isActive = false;
            StartTurn();
        }
    }
    
    CancellationTokenSource targetSelectionCancellationTokenSource;
    public void CancelTargetSelection()
    {
        Debug.Log($"Cancelling target selection for {gameObject.name}");
        targetSelectionCancellationTokenSource?.Cancel();
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