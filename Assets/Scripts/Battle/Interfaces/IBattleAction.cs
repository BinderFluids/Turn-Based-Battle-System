using System;
using System.Collections.Generic;

namespace Battle.Interfaces
{
    public interface IBattleAction
    {
        public event Action onActionStarted;
        public event Action onActionEnded;
    
        void StartAction(BattleEntity actor, BattleEntity target);
        List<BattleEntity> GetValidTargets(BattleEntity actor, IEnumerable<BattleEntity> ctx); 
    }
}