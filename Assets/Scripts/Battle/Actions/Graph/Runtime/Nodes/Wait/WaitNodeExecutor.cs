using Cysharp.Threading.Tasks;

public class WaitNodeExecutor : IStateMachineNodeExecutor<WaitRuntimeNode>
{
    public async UniTask Execute(WaitRuntimeNode node, BattleActionDirector ctx, BattleEntity actor, BattleEntity target)
    {
        await UniTask.WaitForSeconds(node.Duration);
    }
}