using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionDirector : MonoBehaviour
{
    [Header("Graph")] public BattleActionRuntimeGraph RuntimeGraph;

    private Dictionary<Type, object> executors;

    void Awake()
    {
        executors = new Dictionary<Type, object>()
        {
            { typeof(StartRuntimeNode), new StartNodeExecutor() },
            { typeof(MoveToSocketRuntimeNode), new MoveToSocketExecutor() },
            { typeof(WaitRuntimeNode), new WaitNodeExecutor() }
        };
    }
    
    private void Start()
    {
        if (RuntimeGraph == null) return;

        var currentNode = RuntimeGraph.Nodes[0];
        while (currentNode != null)
        {
            if (!executors.TryGetValue(currentNode.GetType(), out var executor))
            {
                Debug.LogError($"No executor found for node type {currentNode.GetType()}");
                break;
            }
        }
    }
}