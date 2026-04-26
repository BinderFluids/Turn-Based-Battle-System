using System;
using Cysharp.Threading.Tasks;

namespace Battle.Graph.Runtime.Nodes
{
    [Serializable]
    public class StartRuntimeNode : RuntimeNode
    {
        public override UniTask Execute(BattleActionDirector ctx, global::Battle.BattleEntity actor, global::Battle.BattleEntity target)
        {
            return UniTask.CompletedTask;
        }
    }
}