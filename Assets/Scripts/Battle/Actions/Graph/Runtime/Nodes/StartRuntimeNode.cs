using System;
using Cysharp.Threading.Tasks;


[Serializable]
public class StartRuntimeNode : RuntimeNode
{
    public override UniTask Execute(BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
    {
        return UniTask.CompletedTask;
    }
}