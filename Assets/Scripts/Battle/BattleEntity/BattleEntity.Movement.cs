using Battle.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public partial class BattleEntity
    {
        private const float DISTANCE_THRESHOLD = 0.1f; 
    
        public async UniTask MoveTo(Vector3 targetPosition, float movement, EntityMoveType moveType = EntityMoveType.Speed)
        {
            if (moveType == EntityMoveType.Speed)
            {
                while (Vector3.Distance(Transform.position, targetPosition) >= DISTANCE_THRESHOLD)
                {
                    Transform.position =
                        Vector3.MoveTowards(Transform.position, targetPosition, movement * Time.deltaTime);
                    await UniTask.Yield();
                }
                Transform.position = targetPosition;
            }

            //TODO: import prime tween
            // if (moveType == EntityMoveType.Duration)
            // {
            //     Tween
            // }
        }

        public async UniTask MoveHome(float speed)
        {
            await MoveTo(startPose.position, speed); 
        }
    }
}