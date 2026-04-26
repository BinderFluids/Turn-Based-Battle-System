using System.Collections.Generic;

namespace Battle
{
    [System.Serializable]
    public abstract class BattleSelectionFilter
    {
        public abstract List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context);
    }
}