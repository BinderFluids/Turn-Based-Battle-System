using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;

[CreateAssetMenu(menuName = "Battle Action/Damage Target", fileName = "DamageTarget", order = 0)]
public class DamageTarget : ScriptableBattleAction
{
    private BattleEntity actor; 
    
    public override void StartAction(BattleEntity actor, BattleEntity target)
    {
        Wait(actor, target).Forget();
    }

    async UniTaskVoid Wait(BattleEntity actor, BattleEntity target)
    {
        target.HealthComponent.ChangeHealth(-actor.Strength);
        Debug.Log($"{actor.name} damaged {target.name} for {actor.Strength}");

        await UniTask.WaitForSeconds(.5f);

        EndAction(actor); 
        
    }
}