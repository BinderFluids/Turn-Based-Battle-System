using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleActionDirector : MonoBehaviour, IBattleAction
{
    [Header("Graph")] public BattleActionRuntimeGraph RuntimeGraph;

    private Dictionary<Type, IStateMachineNodeExecutor> executors;

    void Awake()
    {
        executors = new Dictionary<Type, IStateMachineNodeExecutor>()
        {
            { typeof(StartRuntimeNode), new StartNodeExecutor() },
            { typeof(MoveToSocketRuntimeNode), new MoveToSocketExecutor() },
            { typeof(WaitRuntimeNode), new WaitNodeExecutor() }
        };
    }
    

    public event Action onActionStarted;
    public event Action onActionEnded;
    public async void StartAction(BattleEntity actor, BattleEntity target)
    {
        onActionStarted?.Invoke();
        
        if (RuntimeGraph == null) return;

        var currentNode = RuntimeGraph.Nodes[0];
        bool bailedOut = false; 
        while (currentNode != null)
        {
            if (!executors.TryGetValue(currentNode.GetType(), out IStateMachineNodeExecutor executor))
            {
                bailedOut = true;
                
                Debug.LogError($"No executor found for node type {currentNode.GetType()}");
                onActionEnded?.Invoke();
                break;
            }

            await executor.Execute(currentNode, this, actor, target);   
            currentNode = currentNode.NextNodeIndices.Count > 0 
                ? RuntimeGraph.Nodes[currentNode.NextNodeIndices[0]] 
                : null;
        }

        await actor.MoveHome(2f); 
        if (!bailedOut) onActionEnded?.Invoke();
    }

    public List<BattleEntity> GetValidTargets(BattleEntity actor, IEnumerable<BattleEntity> ctx)
    {
        return ctx.ToList(); 
    }
}