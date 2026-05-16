using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Interfaces;
using UnityEngine;
using UnityUtils;

namespace Battle.SelectionStrategy
{
    [CreateAssetMenu(menuName = "Battle/Action/Selection Strategy/Random", fileName = "SelectRandomBattleAction", order = 0)]
    public class SelectRandomBattleAction : ScriptableObject, IBattleActionSelectionStrategy
    {
        public event Action<IBattleAction> onActionSelected;

        public void GetAction(IEnumerable<IBattleAction> context)
        {
            onActionSelected?.Invoke(context.Random());
        }
    }
}