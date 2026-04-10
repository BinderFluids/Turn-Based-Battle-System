using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class BuildableBattleActionStep
{
    public abstract UniTask Execute(BattleEntity actor, BattleEntity target);
}