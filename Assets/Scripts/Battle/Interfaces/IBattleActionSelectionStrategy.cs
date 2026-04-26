using System;
using System.Collections.Generic;

namespace Battle.Interfaces
{
    public interface IBattleActionSelectionStrategy
    {
        public event Action<IBattleAction> onActionSelected;
    
        void GetAction(IEnumerable<IBattleAction> context);
    }
}