using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BattleSelectionFilter
{
    public abstract List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context);
}