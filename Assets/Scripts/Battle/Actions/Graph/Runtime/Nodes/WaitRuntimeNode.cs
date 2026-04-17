using System;
using Cysharp.Threading.Tasks;

namespace Battle.Actions.Graph.Runtime.Nodes
{
    [Serializable]
    public class WaitRuntimeNode : RuntimeNode
    {
        public float Duration;
        public override async UniTask Execute(BattleActionDirector ctx, global::BattleEntity actor, global::BattleEntity target)
        {
            await UniTask.WaitForSeconds(Duration);
        }
    }
}