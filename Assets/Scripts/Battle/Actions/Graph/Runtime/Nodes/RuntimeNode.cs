using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Battle.Actions.Graph.Runtime.Nodes
{
    [Serializable]
    public abstract class RuntimeNode
    {
        public List<int> NextNodeIndices = new();
    
        public abstract UniTask Execute(BattleActionDirector ctx, global::BattleEntity actor, global::BattleEntity target); 
    }
}