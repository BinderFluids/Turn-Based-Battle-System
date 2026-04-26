using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public partial class BattleEntity
    {
        private const float DISTANCE_THRESHOLD = 0.1f; 
    
        public async UniTask MoveTo(Vector3 targetPosition, float speed)
        {
            while (Vector3.Distance(Transform.position, targetPosition) >= DISTANCE_THRESHOLD)
            {
                Transform.position = Vector3.MoveTowards(Transform.position, targetPosition, speed * Time.deltaTime);
                await UniTask.Yield();
            }
        }

        public async UniTask MoveHome(float speed)
        {
            await MoveTo(startPose.position, speed); 
        }
    }
}