using System;
using Cysharp.Threading.Tasks;

namespace Battle.Graph.Runtime.Nodes
{
    [Serializable]
    public class WaitRuntimeNode : RuntimeNode
    {
        public float Duration;
        public override async UniTask Execute(BattleActionDirector ctx, global::Battle.BattleEntity actor, global::Battle.BattleEntity target)
        {
            await UniTask.WaitForSeconds(Duration);
        }
    }
}