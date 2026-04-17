using Cysharp.Threading.Tasks;

public class WaitNodeExecutor : IStateMachineNodeExecutor
{
    public async UniTask Execute(object node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
    {
        WaitRuntimeNode waitNode = (WaitRuntimeNode)node;
        await UniTask.WaitForSeconds(waitNode.Duration); 
    }
}