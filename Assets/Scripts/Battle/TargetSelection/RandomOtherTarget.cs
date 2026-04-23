using System;
using System.Collections.Generic;
using Battle.Interfaces;
using UnityEngine;

namespace Battle.TargetSelection
{
    [CreateAssetMenu(menuName = "Battle/Entity Selection/Random Other", fileName = "RandomOtherEntity", order = 0)]
    public class RandomOtherTarget : ScriptableObject, IBattleEntitySelectionStrategy
    {
        public event Action<BattleEntity> onEntitySelected;
        public void GetEntity(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx)
        {
            var targetList = action.GetValidTargets(actor, ctx); 
            targetList.Remove(actor); 
            BattleEntity target = targetList.Random();
        
            Debug.Log($"Select Random other target {target}");
            onEntitySelected?.Invoke(target);
        }
    }
}