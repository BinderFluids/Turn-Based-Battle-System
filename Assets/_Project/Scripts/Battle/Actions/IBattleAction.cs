using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IBattleAction
{
    public event Action onActionStarted;
    public event Action onActionEnded;
    
    void StartAction(BattleEntity actor, BattleEntity target);
    List<BattleEntity> GetValidTargets(BattleEntity actor);
}