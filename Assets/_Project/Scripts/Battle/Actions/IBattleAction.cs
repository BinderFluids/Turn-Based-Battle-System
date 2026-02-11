using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public interface IBattleAction
{
    UniTaskVoid Strategy(BattleEntity actor, BattleEntity target);
    List<BattleEntity> GetValidTargets(BattleEntity actor);
}