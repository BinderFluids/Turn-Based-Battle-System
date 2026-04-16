using Cysharp.Threading.Tasks;

public class StartNodeExecutor : IStateMachineNodeExecutor<StartRuntimeNode>
{
    public UniTask Execute(StartRuntimeNode node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target) 
        => UniTask.CompletedTask;
}


