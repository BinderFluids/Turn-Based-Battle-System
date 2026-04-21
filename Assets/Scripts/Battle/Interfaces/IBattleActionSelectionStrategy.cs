using System;
using System.Collections.Generic;

public interface IBattleActionSelectionStrategy
{
    public event Action<IBattleAction> onActionSelected;
    
    void GetAction(IEnumerable<IBattleAction> context);
}