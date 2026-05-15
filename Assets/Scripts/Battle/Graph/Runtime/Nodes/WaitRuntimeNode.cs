using System;
using Cysharp.Threading.Tasks;

namespace Battle.Graph.Runtime.Nodes
{
    [Serializable]
    public class WaitRuntimeNode : RuntimeNode
    {
        public float Duration;
        public override async UniTask Execute(BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
        {
            await UniTask.WaitForSeconds(Duration);
        }
    }
}