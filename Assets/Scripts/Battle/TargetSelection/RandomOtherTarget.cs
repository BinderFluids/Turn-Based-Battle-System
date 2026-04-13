using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Registry;
using UnityEngine;
using UnityUtils;

[CreateAssetMenu(menuName = "Battle Entity Selection Strategy/Random Other", fileName = "RandomOtherEntity", order = 0)]
public class RandomOtherTarget : ScriptableObject, IBattleEntitySelectionStrategy
{
    public event Action<BattleEntity> onEntitySelected;
    public void GetEntity(BattleEntity actor, IBattleAction action)
    {
        var targetList = action.GetValidTargets(actor);
        targetList.Remove(actor); 
        BattleEntity target = targetList.Random();
        
        Debug.Log($"Select Random other target {target}");
        onEntitySelected?.Invoke(target);
    }
}