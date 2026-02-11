using System.Threading;
using Cysharp.Threading.Tasks;

public interface IBattleEntitySelectionStrategy
{
    UniTask<BattleEntity> GetEntity(BattleEntity actor, IBattleAction action, CancellationToken ct);
}