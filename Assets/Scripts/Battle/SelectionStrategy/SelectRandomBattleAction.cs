using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtils; 


[CreateAssetMenu(menuName = "Battle/Action/Selection Strategy/Random   ", fileName = "SelectRandomBattleAction", order = 0)]
public class SelectRandomBattleAction : ScriptableObject, IBattleActionSelectionStrategy
{
    public event Action<IBattleAction> onActionSelected;

    public void GetAction(IEnumerable<IBattleAction> context)
    {
        Debug.Log($"Selecting random action from {context.Count()} actions");
        onActionSelected?.Invoke(context.Random());
    }
}