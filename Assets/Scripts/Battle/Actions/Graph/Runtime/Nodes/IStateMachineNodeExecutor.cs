using Cysharp.Threading.Tasks;

public interface IStateMachineNodeExecutor<in TNode> where TNode : RuntimeNode
{
    UniTask Execute(TNode node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target);
}