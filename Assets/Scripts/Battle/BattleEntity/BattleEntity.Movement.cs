using System;
using Battle.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using PlasticPipe.PlasticProtocol.Messages;

namespace Battle
{
    public partial class BattleEntity
    {
        private const float DISTANCE_THRESHOLD = 0.1f; 
    
        public async UniTask MoveTo(Vector3 targetPosition, float movement, EntityMotionType motionType = EntityMotionType.Duration)
        {
            Vector3 startPosition = Transform.position;
            float duration; 
            
            if (motionType == EntityMotionType.Duration)
                await LMotion.Create(startPosition, targetPosition, movement)
                    .BindToPosition(Transform)
                    .AddTo(Transform)
                    .ToUniTask(); 
            else if (motionType == EntityMotionType.Speed)
            {
                float distance = Mathf.Abs((startPosition - targetPosition).magnitude);
                float time = distance / movement;
                
                await LMotion.Create(startPosition, targetPosition, time)
                    .BindToPosition(Transform)
                    .AddTo(Transform)
                    .ToUniTask(); 
                
                Transform.position = targetPosition;
            }
        }

        public async UniTask Jump(Vector3 targetPosition, float height, float movement,
            EntityMotionType motionType = EntityMotionType.Duration)
        {
            Vector3 startPosition = Transform.position;

            float duration = motionType switch
            {
                EntityMotionType.Duration => movement,
                EntityMotionType.Speed => Vector3.Distance(startPosition, targetPosition) / movement,
                _ => throw new System.NotImplementedException()
            };

            float peakY = Mathf.Max(startPosition.y, targetPosition.y) + height;

            await LMotion.Create(0f, 1f, duration)
                .Bind(t =>
                {
                    float x = Mathf.Lerp(startPosition.x, targetPosition.x, t);

                    float y = t < 0.5f
                        ? Mathf.Lerp(startPosition.y, peakY, t * 2f)
                        : Mathf.Lerp(peakY, targetPosition.y, (t - 0.5f) * 2f);

                    Transform.position = new Vector3(x, y, startPosition.z);
                })
                .ToUniTask();
        }
        
        public async UniTask MoveHome(float speed) 
            => await MoveTo(startPose.position, speed);
    }
}