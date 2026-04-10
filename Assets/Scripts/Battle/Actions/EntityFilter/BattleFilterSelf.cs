using System.Collections.Generic;
using System.Linq;

public class BattleFilterSelf : BattleSelectionFilter, INestableAsset
{
    public override string GetListDisplayName() => "Ignore Self";
    
    public override List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context)
    {
        return context.Where(e => e != actor).ToList();
    }
}