using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Registry;

[CreateAssetMenu(menuName = "Battle Action/Damage Target", fileName = "DamageTarget", order = 0)]
public class DamageTarget : ScriptableBattleAction
{
    private BattleEntity actor; 
    
    public override void StartAction(BattleEntity actor, BattleEntity target)
    {
        // Debug.Log($"{actor.name} damaged {target.name} for {actor.statBlock.Attack}");
        // target.statBlock.Health.ChangeValue(-actor.statBlock.Attack.GetValue());
        
        EndAction(actor); 
    }
}