using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Registry;
using UnityEngine;
using UnityUtils;

[CreateAssetMenu(menuName = "Battle Entity Selection Strategy/Random Other", fileName = "RandomOtherEntity", order = 0)]
public class RandomOtherTarget : ScriptableObject, IBattleEntitySelectionStrategy
{
    public async UniTask<BattleEntity> GetEntity(BattleEntity actor, CancellationToken ct)
    {
        return Registry<BattleEntity>
            .All
            .Where(e => e != actor)
            .Random();
    }
}