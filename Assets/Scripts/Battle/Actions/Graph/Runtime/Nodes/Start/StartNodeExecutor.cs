using Cysharp.Threading.Tasks;

public class StartNodeExecutor : IStateMachineNodeExecutor
{
    public UniTask Execute(object node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target) 
        => UniTask.CompletedTask;
}


