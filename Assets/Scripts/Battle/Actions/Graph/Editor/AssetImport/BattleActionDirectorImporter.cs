using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Api;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, BattleActionDirectorGraph.AssetExtension)]
internal class BattleActionDirectorImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var graph = GraphDatabase.LoadGraphForImporter<BattleActionDirectorGraph>(ctx.assetPath);
        if (graph == null)
        {
            Debug.LogError($"Failed to load graph for {ctx.assetPath}");
            return; 
        }
        
        var startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
        if (startNodeModel == null) return;

        var runtimeAsset = ScriptableObject.CreateInstance<BattleActionRuntimeGraph>();
        var nodeMap = new Dictionary<INode, int>();
        
        //First pass: create nodes
        CreateRuntimeNodes(startNodeModel, runtimeAsset, nodeMap);
        
        //Second pass: setup connections
        SetupConnections(startNodeModel, runtimeAsset, nodeMap);
        
        ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
        ctx.SetMainObject(runtimeAsset); 
    }

    void CreateRuntimeNodes(INode startNode, BattleActionRuntimeGraph runtimeGraph, Dictionary<INode, int> nodeMap)
    {
        var nodesToProcess = new Queue<INode>(); 
        nodesToProcess.Enqueue(startNode);

        while (nodesToProcess.Count > 0)
        {
            var currentNode = nodesToProcess.Dequeue();
            
            if (nodeMap.ContainsKey(currentNode)) continue;
            
            var runtimeNodes = TranslateNodeModelToRuntimeNodes(currentNode);
            foreach (var runtimeNode in runtimeNodes)
            {
                nodeMap[currentNode] = runtimeGraph.Nodes.Count; 
                runtimeGraph.Nodes.Add(runtimeNode);
            }
            
            //Queue up all the connect nodes
            for (int i = 0; i < currentNode.OutputPortCount; i++)
            {
                var port = currentNode.GetOutputPort(i);
                
                if (port.IsConnected)
                    nodesToProcess.Enqueue(port.FirstConnectedPort.GetNode());
            }
        }
    }

    void SetupConnections(INode startNode, BattleActionRuntimeGraph runtimeGraph, Dictionary<INode, int> nodeMap)
    {
        foreach (var kvp in nodeMap)
        {
            var editorNode = kvp.Key;
            var runtimeIndex = kvp.Value;
            var runtimeNode = runtimeGraph.Nodes[runtimeIndex];

            for (int i = 0; i < editorNode.OutputPortCount; i++)
            {
                var port = editorNode.GetOutputPort(i);
                if (port.IsConnected && nodeMap.TryGetValue(port.FirstConnectedPort.GetNode(), out int nextIndex))
                {
                    runtimeNode.NextNodeIndices.Add(nextIndex);
                }
            }
        }
    }

    static List<RuntimeNode> TranslateNodeModelToRuntimeNodes(INode nodeModel)
    {
        List<RuntimeNode> returnedNodes = new List<RuntimeNode>();

        switch (nodeModel)
        {
            case StartNode:
                returnedNodes.Add(new StartRuntimeNode());
                break;
            // case ActionNode:
            //     break;
            default:
                throw new ArgumentException($"Unsupported node type: {nodeModel.GetType()}");
        }

        return returnedNodes; 
    }
    static T GetInputPortValue<T>(IPort port)
    {
        T value = default;

        if (port.IsConnected)
        {
            switch (port.FirstConnectedPort.GetNode())
            {
                case IVariableNode variableNode:
                    variableNode.Variable.TryGetDefaultValue<T>(out value);
                    return value; 
                case IConstantNode constantNode:
                    constantNode.TryGetValue<T>(out value);
                    return value; 
                default:
                    break;
            }
        }
        else
        {
            port.TryGetValue(out value);
        }

        return value; 
    }
}