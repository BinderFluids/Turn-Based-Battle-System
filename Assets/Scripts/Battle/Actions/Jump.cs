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
        [SerializeField] private ActionCommandTierGradient gradient; 
        [SerializeField] private int actionCommandWindow; 
        
        [SerializeField] private float approachDuration;
        [SerializeField] private float jumpDuration;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float miniumAboveEnemy = 1f;

        [SerializeField, Range(0f, 1f), Tooltip("Left side is rise time, right side is fall time.")] 
        private float jumpSegmentDistribution; 
        [SerializeField] private Ease jumpRiseEase;
        [SerializeField] private Ease jumpFallEase;
        [SerializeField] private Ease horizontalMovementEase; 
    
        public override void StartAction(BattleEntity actor, BattleEntity target)
        {
            Transform transform = actor.transform;
            Vector3 startPos = transform.position;
            float targetHeight = Mathf.Max(transform.position.y + jumpHeight, target.transform.position.y + miniumAboveEnemy);

            Vector3 topPosition = target.transform.position;  
            // if (target.TryGetComponent(out FormationSlotComponent formationSlotComponent))
            //     topPosition = formationSlotComponent.topPosition;
            
            Sequence verticalMovement = Sequence.Create()
                .Chain(
                    Tween.PositionY(
                        transform,
                        targetHeight,
                        jumpDuration * jumpSegmentDistribution,
                        jumpRiseEase
                    )
                )
                .Chain(
                    Tween.PositionY(
                        transform,
                        topPosition.y,
                        jumpDuration * (1f - jumpSegmentDistribution),
                        jumpFallEase
                    )
                );
            Tween horizontalMovement = Tween.PositionX(transform, target.transform.position.x, jumpDuration, horizontalMovementEase);

            AwaitTween(actor, transform, startPos, verticalMovement).Forget();
        }

        async UniTask<ActionCommandOutcome> AwaitActionCommand(BattleEntity actor)
        {
            PlayerId playerId = PlayerId.PlayerOne;
            if (RequestHub<RequestPlayerId>.TryRequest(actor, out var request))
                playerId = request.PlayerId; 
        
        
            var window = new ActionCommandWindow(
                id: "Jump",
                duration: actionCommandWindow,
                expectedPlayerInputs: new List<PlayerId> { playerId },
                outcomeStrategy: new DefaultOutcomeStrategy(ActionCommandTier.GOOD)
            );

            ActionCommandOutcome outcome = await BattleWindowService.Instance.RunActionCommandAsync(window);
            Debug.Log($"Success: {outcome.Success} Tier: {outcome.Tier}");
            if (outcome.Tier == ActionCommandTier.GOOD) Debug.Log("Do second jump!");
            
            return outcome; 
        }


        async UniTaskVoid AwaitTween(BattleEntity actor, Transform transform, Vector3 startPos, Sequence sequence)
        {
            await sequence;
        
            ActionCommandOutcome action = await AwaitActionCommand(actor);
        
            Debug.Log(action.Success);
        
            transform.position = startPos;
            EndAction(actor); 
        }
    }
}