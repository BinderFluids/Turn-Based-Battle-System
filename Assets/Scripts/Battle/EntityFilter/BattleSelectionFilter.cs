using System.Collections.Generic;

[System.Serializable]
public abstract class BattleSelectionFilter
{
    public abstract List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context);
}