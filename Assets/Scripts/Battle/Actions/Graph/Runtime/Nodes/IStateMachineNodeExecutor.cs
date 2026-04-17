using Cysharp.Threading.Tasks;

public interface IStateMachineNodeExecutor
{
    UniTask Execute(object node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target); 
}