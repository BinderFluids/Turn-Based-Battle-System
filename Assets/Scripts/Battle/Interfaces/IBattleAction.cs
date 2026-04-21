using System;
using System.Collections.Generic;

public interface IBattleAction
{
    public event Action onActionStarted;
    public event Action onActionEnded;
    
    void StartAction(BattleEntity actor, BattleEntity target);
    List<BattleEntity> GetValidTargets(BattleEntity actor, IEnumerable<BattleEntity> ctx); 
}