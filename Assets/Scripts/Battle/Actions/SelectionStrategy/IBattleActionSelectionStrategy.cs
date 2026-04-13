using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IBattleActionSelectionStrategy
{
    public event Action<IBattleAction> onActionSelected;
    
    void GetAction(IEnumerable<IBattleAction> context);
}