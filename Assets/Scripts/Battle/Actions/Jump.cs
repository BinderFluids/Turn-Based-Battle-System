using System.Collections.Generic;
using Battle.Enums;
using Battle.Requests;
using Battle.Window;
using Core.Enums;
using Cysharp.Threading.Tasks;
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
        // [SerializeField] private Ease jumpRiseEase;
        // [SerializeField] private Ease jumpFallEase;
        // [SerializeField] private Ease horizontalMovementEase;

        [Header("Second Jump Settings")] [SerializeField]
        private float secondJumpDuration;
        private float secondJumpHeight; 
    
        public override async void StartAction(BattleEntity actor, BattleEntity target)
        {
            Transform transform = actor.transform;
            Vector3 startPos = transform.position;

            Vector3 topPosition = target.transform.position;  
            // if (target.TryGetComponent(out FormationSlotComponent formationSlotComponent))
            //     topPosition = formationSlotComponent.topPosition;
            
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, Vector3.up);
            moveDirection *= approachEndDistance;
            Vector3 approachPosition = target.transform.position - moveDirection;
            
            await actor.MoveTo(approachPosition, approachDuration, EntityMotionType.Duration);

            await actor.Jump(topPosition, jumpHeight, jumpDuration, EntityMotionType.Duration); 
            ActionCommandOutcome firstJumpOutcome = await AwaitActionCommand(actor, firstJumpGradient);

            //end early if you didn't get a good rating
            if (firstJumpOutcome.Tier != ActionCommandTier.GOOD)
            {
                await actor.Jump(approachPosition, jumpHeight, jumpDuration, EntityMotionType.Duration); 
                transform.position = startPos;
                EndAction(actor); 
                return; 
            }
            
            await actor.Jump(topPosition, jumpHeight, jumpDuration, EntityMotionType.Duration); 
            ActionCommandOutcome secondJumpOutcome = await AwaitActionCommand(actor, secondJumpGradient);
            
            await actor.Jump(approachPosition, jumpHeight, jumpDuration, EntityMotionType.Duration); 
            transform.position = startPos;
            
            EndAction(actor); 
        }

        async UniTask<ActionCommandOutcome> AwaitActionCommand(BattleEntity actor, ActionCommandTierGradient gradient)
        {
            PlayerId playerId = PlayerId.PlayerOne;
            if (RequestHub<RequestablePlayerId>.TryRequest(actor, out var request))
                playerId = request.PlayerId;
            
            var window = BattleWindowService.Instance.ActionCommandWindowBuilder
                .WithPlayerInput(playerId)
                .Build("Jump", gradient);

            ActionCommandOutcome outcome = await BattleWindowService.Instance.RunActionCommandAsync(window);
            Debug.Log($"Outcome: {outcome.Tier}");
            
            return outcome; 
        }
    }
}