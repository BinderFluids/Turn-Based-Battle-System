using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableBattleEntitySelectionStrategy : ScriptableObject, IBattleEntitySelectionStrategy
{
    public abstract event Action<BattleEntity> onEntitySelected;
    public abstract void GetEntity(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx); 
}