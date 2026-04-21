using System;
using Cysharp.Threading.Tasks;

namespace Battle.Actions.Graph.Runtime.Nodes
{
    [Serializable]
    public class StartRuntimeNode : RuntimeNode
    {
        public override UniTask Execute(BattleActionDirector ctx, global::BattleEntity actor, global::BattleEntity target)
        {
            return UniTask.CompletedTask;
        }
    }
}