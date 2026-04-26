using System;
using System.Collections.Generic;
using Battle.Interfaces;
using UnityEngine;

namespace Battle.TargetSelection
{
    public abstract class ScriptableBattleEntitySelectionStrategy : ScriptableObject, IBattleEntitySelectionStrategy
    {
        public abstract event Action<BattleEntity> onEntitySelected;
        public abstract void GetEntity(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx); 
    }
}