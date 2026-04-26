using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Graph.Runtime.Nodes
{
    [Serializable]
    public class MoveToSocketRuntimeNode : RuntimeNode
    {
        public Vector3 SocketPosition;
        public float Speed;

        public override async UniTask Execute(BattleActionDirector ctx, BattleEntity actor, global::Battle.BattleEntity target)
        {
            await actor.MoveTo(SocketPosition, Speed); 
        }
    }
}