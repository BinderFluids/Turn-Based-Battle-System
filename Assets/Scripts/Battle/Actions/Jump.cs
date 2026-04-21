using System.Collections.Generic;
using Battle.BattleEntity;
using Battle.BattleWindow;
using Battle.BattleWindow.OutcomeStrategy;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/Action/Player/Jump", fileName = "Jump", order = 0)]
public class Jump : ScriptableBattleAction
{
    private BattleEntity actor;
    
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
        this.actor = actor; 
        
        Transform transform = actor.transform;
        Vector3 startPos = transform.position;
        float targetHeight = Mathf.Max(transform.position.y + jumpHeight, target.transform.position.y + miniumAboveEnemy);

        Vector3 topPosition = target.transform.position;  
        if (target.TryGetComponent(out FormationSlotComponent formationSlotComponent))
            topPosition = formationSlotComponent.topPosition;
        
        
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

    async UniTask<ActionCommandOutcome> AwaitActionCommand()
    {
        PlayerId playerId = PlayerId.PlayerOne;
        if (!actor.TryGetComponent(out PlayerEntityComponent playerEntityComponent))
            Debug.LogWarning("No player entity component found on actor, using PlayerOne's input");
        else
            playerId = playerEntityComponent.PlayerID; 
        
        
        var window = new ActionCommandWindow(
            id: "Jump",
            duration: 10,
            expectedPlayerInputs: new List<PlayerId> { playerId },
            outcomeStrategy: new DefaultOutcomeStrategy()
        );

        return await BattleWindowService.Instance.RunActionCommandAsync(window);
    }


    async UniTaskVoid AwaitTween(BattleEntity actor, Transform transform, Vector3 startPos, Sequence sequence)
    {
        await sequence;
        
        ActionCommandOutcome action = await AwaitActionCommand();
        
        Debug.Log(action.Success);
        
        transform.position = startPos;
        EndAction(actor); 
    }
}