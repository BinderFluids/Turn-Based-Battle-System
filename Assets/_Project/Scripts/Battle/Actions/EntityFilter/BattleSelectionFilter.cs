using System.Collections.Generic;

public abstract class BattleSelectionFilter : NestedAsset
{
    public abstract List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context);
}