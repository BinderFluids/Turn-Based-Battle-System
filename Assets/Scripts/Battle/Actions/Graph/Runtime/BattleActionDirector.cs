using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.Actions.Graph.Runtime
{
    public class BattleActionDirector : BattleEntityComponent, IBattleAction
    {
        [Header("Graph")] public BattleActionRuntimeGraph RuntimeGraph;
        
        public event Action onActionStarted;
        public event Action onActionEnded;
        public async void StartAction(global::BattleEntity actor, global::BattleEntity target)
        {
            onActionStarted?.Invoke();
        
            if (RuntimeGraph == null) return;

            var currentNode = RuntimeGraph.Nodes[0];
            while (currentNode != null)
            {
                await currentNode.Execute(this, actor, target);   
                currentNode = currentNode.NextNodeIndices.Count > 0 
                    ? RuntimeGraph.Nodes[currentNode.NextNodeIndices[0]] 
                    : null;
            }

            await actor.MoveHome(2f); 
            onActionEnded?.Invoke();
        }

        public List<global::BattleEntity> GetValidTargets(global::BattleEntity actor, IEnumerable<global::BattleEntity> ctx)
        {
            return ctx.ToList(); 
        }
    }
}