using System.Collections.Generic;
using System.Linq;

public class BattleEntitySelfFilter : IProcessor<List<BattleEntity>, List<BattleEntity>>
{
    private BattleEntity actor;
    private bool isSelf;

    public BattleEntitySelfFilter(BattleEntity actor, bool isSelf = false)
    {
        this.actor = actor;
        this.isSelf = isSelf;
    } 

    public List<BattleEntity> Process(List<BattleEntity> entities)
    {
        if (isSelf) return new List<BattleEntity> {actor};
        return entities.Where(e => e != actor).ToList();
    }
}