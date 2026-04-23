using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Battle.Graph.Runtime.Nodes
{
    [Serializable]
    public abstract class RuntimeNode
    {
        public List<int> NextNodeIndices = new();
    
        public abstract UniTask Execute(BattleActionDirector ctx, global::Battle.BattleEntity actor, global::Battle.BattleEntity target); 
    }
}