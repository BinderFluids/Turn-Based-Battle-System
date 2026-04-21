using System;
using System.Collections.Generic;

public interface IBattleEntitySelectionStrategy
{
    public event Action<BattleEntity> onEntitySelected;
    
    void GetEntity(BattleEntity actor, IBattleAction action, IEnumerable<BattleEntity> ctx); 
}