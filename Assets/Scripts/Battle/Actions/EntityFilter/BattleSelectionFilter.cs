using System.Collections.Generic;
using UnityEngine;

public abstract class BattleSelectionFilter : NestedAsset
{
    public abstract List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context);
}