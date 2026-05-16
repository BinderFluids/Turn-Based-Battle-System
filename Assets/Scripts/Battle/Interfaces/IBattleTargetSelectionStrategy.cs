using System;
using System.Collections.Generic;
using Battle.Interfaces;

namespace Battle.TargetSelection
{
    public interface IBattleEntitySelectionStrategy
    {
        public event Action<BattleEntity> onEntitySelected;
    
        void BeginTargetSelection(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx); 
    }
}