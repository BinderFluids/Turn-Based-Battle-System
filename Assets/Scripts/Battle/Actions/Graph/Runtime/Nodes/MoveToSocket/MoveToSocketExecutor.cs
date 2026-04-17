
using Cysharp.Threading.Tasks;

public class MoveToSocketExecutor : IStateMachineNodeExecutor
{
    public async UniTask Execute(object node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
    {
        MoveToSocketRuntimeNode moveToSocketNode = (MoveToSocketRuntimeNode)node;
        
        await actor.MoveTo(moveToSocketNode.SocketPosition, moveToSocketNode.Speed); 
    }
}