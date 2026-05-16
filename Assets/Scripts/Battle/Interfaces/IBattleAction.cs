using System;
using System.Collections.Generic;
using Battle.TargetSelection;

namespace Battle.Interfaces
{
    public interface IBattleAction
    {
        public event Action onActionStarted;
        public event Action onActionEnded;

        public IBattleEntitySelectionStrategy ForcedTargetSelectionStrategy { get; }

        void StartAction(BattleEntity actor, BattleEntity target);
        List<BattleEntity> GetValidTargets(BattleEntity actor, IEnumerable<BattleEntity> ctx); 
    }
}