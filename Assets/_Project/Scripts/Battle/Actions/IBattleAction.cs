using Cysharp.Threading.Tasks;

public interface IBattleAction
{
    UniTaskVoid Strategy(BattleEntity actor, BattleEntity target);
}