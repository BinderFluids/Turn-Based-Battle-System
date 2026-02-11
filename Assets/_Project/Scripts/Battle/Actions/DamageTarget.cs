using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Action/Damage Target", fileName = "DamageTarget", order = 0)]
public class DamageTarget : ScriptableBattleAction
{
    public override async UniTaskVoid Strategy(BattleEntity actor, BattleEntity target)
    {
        target.HealthComponent.ChangeHealth(-actor.Strength);
        Debug.Log($"{actor.name} damaged {target.name} for {actor.Strength}");

        await UniTask.WaitForSeconds(.5f);

        NextTurn(actor); 
    }
}