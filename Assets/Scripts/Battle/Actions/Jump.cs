using System.Collections.Generic;
using Battle.Enums;
using Battle.Requests;
using Battle.Window;
using Core.Enums;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using RequestHub;

namespace Battle.Actions
{
    [CreateAssetMenu(menuName = "Battle/Action/Player/Jump", fileName = "Jump", order = 0)]
    public class Jump : ScriptableBattleAction
    {
        [SerializeField] private ActionCommandTierGradient firstJumpGradient;
        [SerializeField] private ActionCommandTierGradient secondJumpGradient; 
 
        [SerializeField] private float approachDuration;
        [SerializeField] private float approachEndDistance;
        [SerializeField] private float jumpDuration;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float miniumAboveEnemy = 1f;

        [Header("First Jump Settings"), SerializeField, Range(0f, 1f), Tooltip("Left side is rise time, right side is fall time.")] 
        private float jumpSegmentDistribution; 
        [SerializeField] private Ease jumpRiseEase;
        [SerializeField] private Ease jumpFallEase;
        [SerializeField] private Ease horizontalMovementEase;

        [Header("Second Jump Settings")] [SerializeField]
        private float secondJumpDuration;
        private float secondJumpHeight; 
    
        public override async void StartAction(BattleEntity actor, BattleEntity target)
        {
            Transform transform = actor.transform;
            Vector3 startPos = transform.position;
            float targetHeight = Mathf.Max(transform.position.y + jumpHeight, target.transform.position.y + miniumAboveEnemy);

            Vector3 topPosition = target.transform.position;  
            // if (target.TryGetComponent(out FormationSlotComponent formationSlotComponent))
            //     topPosition = formationSlotComponent.topPosition;

            await AwaitApproach(transform, topPosition);
            
            await AwaitJump(transform, target.transform);
            ActionCommandOutcome firstJumpOutcome = await AwaitActionCommand(actor, firstJumpGradient);

            //end early if you didn't get a good rating
            if (firstJumpOutcome.Tier != ActionCommandTier.GOOD)
            {
                await AwaitLand(transform, target.transform);
                transform.position = startPos;
                EndAction(actor); 
                return; 
            }
            
            await AwaitJump(transform, target.transform, false);
            ActionCommandOutcome secondJumpOutcome = await AwaitActionCommand(actor, secondJumpGradient);
            
            await AwaitLand(transform, target.transform);
            transform.position = startPos;
            
            EndAction(actor); 
        }

        async UniTask<ActionCommandOutcome> AwaitActionCommand(BattleEntity actor, ActionCommandTierGradient gradient)
        {
            PlayerId playerId = PlayerId.PlayerOne;
            if (RequestHub<RequestPlayerId>.TryRequest(actor, out var request))
                playerId = request.PlayerId; 
            
            var window = new ActionCommandWindow(
                id: "Jump",
                expectedPlayerInputs: new List<PlayerId> { playerId },
                outcomeStrategy: new ActionCommandOutcomeStrategy(),
                gradient: gradient
            );

            ActionCommandOutcome outcome = await BattleWindowService.Instance.RunActionCommandAsync(window);
            Debug.Log($"Outcome: {outcome.Tier}");
            
            return outcome; 
        }

        async UniTask AwaitApproach(Transform actorTransform, Vector3 targetPosition)
        {
            Vector3 directionToTarget = targetPosition - actorTransform.position;
            directionToTarget = Vector3.ProjectOnPlane(directionToTarget, Vector3.up); 
            directionToTarget.Normalize();
            directionToTarget *= approachEndDistance;
            
            Tween approachTween = 
                Tween.Position(
                    actorTransform, 
                    targetPosition - directionToTarget, 
                    approachDuration, 
                    horizontalMovementEase
                    );

            await approachTween; 
        }
        async UniTask AwaitJump(Transform actorTransform, Transform targetTransform, bool moveHorizontally = true)
        {
            
            Vector3 topPosition = targetTransform.transform.position; 
            float targetHeight = Mathf.Max(actorTransform.position.y + jumpHeight, targetTransform.transform.position.y + miniumAboveEnemy);
            
            Sequence verticalMovement = Sequence.Create()
                //Rise
                .Chain(
                    Tween.PositionY(
                        actorTransform,
                        targetHeight,
                        jumpDuration * jumpSegmentDistribution,
                        jumpRiseEase
                    )
                )
                //Fall
                .Chain(
                    Tween.PositionY(
                        actorTransform,
                        topPosition.y,
                        jumpDuration * (1f - jumpSegmentDistribution),
                        jumpFallEase
                    )
                );
            //Horizontal movement
            
            if (moveHorizontally)
            {
                Tween.PositionX(
                    actorTransform,
                    targetTransform.position.x,
                    jumpDuration,
                    horizontalMovementEase
                );
            }

            await verticalMovement; 
        }
        
        async UniTask AwaitLand(Transform actorTransform, Transform targetTransform)
        {
            
        }
    }
}