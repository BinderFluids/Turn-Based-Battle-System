using PrimeTween;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Action/Player/Jump", fileName = "Jump", order = 0)]
public class Jump : ScriptableBattleAction
{
    public override async UniTaskVoid Strategy(BattleEntity actor, BattleEntity target)
    {
        Transform transform = actor.transform;
        Vector3 startPos = transform.position;
        float jumpHeight = 2f;
        float miniumAboveEnemy = 1f; 
        float targetHeight = Mathf.Max(transform.position.y + jumpHeight, target.topPosition.y + miniumAboveEnemy); 
        
        float duration = 2f;
        Ease jumpEase = Ease.InOutSine;
        Sequence verticalMovement = Sequence.Create()
            .Chain(
                Tween.PositionY(
                    transform,
                    targetHeight,
                    duration / 2f,
                    jumpEase
                )
            )
            .Chain(
                Tween.PositionY(
                    transform,
                    target.topPosition.y,
                    duration / 2f,
                    jumpEase
                )
            );
        Tween horizontalMovement = Tween.PositionX(transform, target.topPosition.x, duration);

        await horizontalMovement;
        
        transform.position = startPos;
        NextTurn(actor); 
    }
}