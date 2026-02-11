using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;

public class BattleEntity : MonoBehaviour, ISelectable
{
    public int Strength;
    public int Speed;

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
        Debug.Log($"Starting turn for {gameObject.name}");

        IBattleAction action = await AwaitActionSelection();
        AwaitTargetSelection(action).Forget(); 
    }

    async UniTask<IBattleAction> AwaitActionSelection()
    {
        return await actionSelectionStrategyRef.Value
            .GetAction(actions);
    }

    async UniTaskVoid AwaitTargetSelection(IBattleAction action)
    {
        targetSelectionCancellationTokenSource = new CancellationTokenSource();
        try
        {
            BattleEntity target = await targetSelectionStrategyRef.Value
                .GetEntity(this, targetSelectionCancellationTokenSource.Token);
            
            action.Strategy(this, target).Forget();
        }
        catch (OperationCanceledException e)
        {
            Debug.Log($"Target selection for {gameObject.name} was cancelled");
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
    public void OnHover(bool isHovering)
    {
        if (!isHovering) return;
        Debug.Log($"Hovering {gameObject.name}");
    }
}