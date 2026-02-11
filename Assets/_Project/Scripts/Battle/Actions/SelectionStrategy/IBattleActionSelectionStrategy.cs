using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IBattleActionSelectionStrategy
{
    UniTask<IBattleAction> GetAction(List<IBattleAction> context);
}