using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Actions.Graph.Runtime.Nodes
{
    [Serializable]
    public class MoveToSocketRuntimeNode : RuntimeNode
    {
        public Vector3 SocketPosition;
        public float Speed;

        public override async UniTask Execute(BattleActionDirector ctx, global::BattleEntity actor, global::BattleEntity target)
        {
            await actor.MoveTo(SocketPosition, Speed); 
        }
    }
}