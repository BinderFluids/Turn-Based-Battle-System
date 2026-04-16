
using Cysharp.Threading.Tasks;

public class MoveToSocketExecutor : IStateMachineNodeExecutor<MoveToSocketRuntimeNode>
{
    public async UniTask Execute(MoveToSocketRuntimeNode node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
    {
        await actor.MoveTo(node.SocketPosition, node.Speed); 
    }
}