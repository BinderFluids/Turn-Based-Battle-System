using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public interface IBattleEntitySelectionStrategy
{
    public event Action<BattleEntity> onEntitySelected;
    
    void GetEntity(BattleEntity actor, IBattleAction action);
}