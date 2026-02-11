using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(menuName = "Battle Action Selection Strategy/Random", fileName = "SelectRandomBattleAction", order = 0)]
public class SelectRandomBattleAction : ScriptableObject, IBattleActionSelectionStrategy
{
    public async UniTask<IBattleAction> GetAction(List<IBattleAction> context)
    {
        return context[Random.Range(0, context.Count)];
    }
}