using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class BattleFilterSelf : BattleSelectionFilter
{
    public override List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context)
    {
        return context.Where(e => e != actor).ToList();
    }
}