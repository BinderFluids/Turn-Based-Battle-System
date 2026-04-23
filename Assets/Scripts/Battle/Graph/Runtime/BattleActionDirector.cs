using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Interfaces;
using Battle.Components;
using UnityEngine;

namespace Battle.Graph.Runtime
{
    public class BattleActionDirector : BattleEntityComponent, IBattleAction
    {
        [SerializeField] private BattleEntity entity;
        public BattleEntity Entity => entity;
        
        [Header("Graph")] public BattleActionRuntimeGraph RuntimeGraph;
        
        public event Action onActionStarted;
        public event Action onActionEnded;
        public async void StartAction(global::Battle.BattleEntity actor, global::Battle.BattleEntity target)
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

        public List<global::Battle.BattleEntity> GetValidTargets(global::Battle.BattleEntity actor, IEnumerable<global::Battle.BattleEntity> ctx)
        {
            return ctx.ToList(); 
        }

    }
}