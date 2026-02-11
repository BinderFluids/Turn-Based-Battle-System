using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class ScriptableBattleEntitySelectionStrategy : ScriptableObject, IBattleEntitySelectionStrategy
{
    public abstract UniTask<BattleEntity> GetEntity(BattleEntity actor, CancellationToken ct); 
}