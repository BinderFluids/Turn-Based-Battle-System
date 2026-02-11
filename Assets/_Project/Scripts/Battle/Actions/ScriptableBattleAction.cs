using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class ScriptableBattleAction : ScriptableObject, IBattleAction
{
    public abstract UniTaskVoid Strategy(BattleEntity actor, BattleEntity target);
    public abstract List<BattleEntity> GetValidTargets(BattleEntity actor); 

    protected void NextTurn(BattleEntity actor)
    {
        NextTurnEvent nextTurnEvent = new NextTurnEvent {previousActor = actor};
        EventBus<NextTurnEvent>.Raise(nextTurnEvent);
    }
}