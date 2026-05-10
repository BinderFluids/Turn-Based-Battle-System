using Battle.Enums;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

namespace Battle
{
    public partial class BattleEntity
    {
        private const float DISTANCE_THRESHOLD = 0.1f; 
    
        public async UniTask MoveTo(Vector3 targetPosition, float movement, EntityMoveType moveType = EntityMoveType.Duration)
        {
            if (moveType == EntityMoveType.Duration)
                await Tween.Position(Transform, targetPosition, movement);

            if (moveType == EntityMoveType.Speed)
                await Tween.PositionAtSpeed(Transform, targetPosition, movement); 
        }
        public async UniTask MoveHome(float speed) 
            => await MoveTo(startPose.position, speed);
    }
}