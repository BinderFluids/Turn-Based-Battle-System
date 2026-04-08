using System.Collections.Generic;
using System.Linq;

public class BattleFilterSelf : BattleSelectionFilter
{
    public override string GetNestedAssetName()
    {
        return "Ignore Self";
    }
    
    public override List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context)
    {
        return context.Where(e => e != actor).ToList();
    }
}