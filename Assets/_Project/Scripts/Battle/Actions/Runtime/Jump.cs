using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;

[CreateAssetMenu(menuName = "Battle Action/Player/Jump", fileName = "Jump", order = 0)]
public class Jump : ScriptableBattleAction
{
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
                    target.topPosition.y,
                    jumpDuration * (1f - jumpSegmentDistribution),
                    jumpFallEase
                )
            );
        Tween horizontalMovement = Tween.PositionX(transform, target.transform.position.x, jumpDuration, horizontalMovementEase);

        AwaitTween(actor, transform, startPos, verticalMovement).Forget(); 
    }

    async UniTaskVoid AwaitTween(BattleEntity actor, Transform transform, Vector3 startPos, Sequence sequence)
    {
        await sequence;
        
        transform.position = startPos;
        EndAction(actor); 
    }
}