using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class ScriptableBattleAction : ScriptableObject, IBattleAction
{
    public abstract UniTaskVoid Strategy(BattleEntity actor, BattleEntity target);

    protected void NextTurn(BattleEntity actor)
    {
        NextTurnEvent nextTurnEvent = new NextTurnEvent {previousActor = actor};
        EventBus<NextTurnEvent>.Raise(nextTurnEvent);
    }
}