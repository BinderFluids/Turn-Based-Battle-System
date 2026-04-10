using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Wait : BuildableBattleActionStep
{
    [SerializeField] private float duration;
    
    
    
    public override async UniTask Execute(BattleEntity actor, BattleEntity target)
    {
        await UniTask.WaitForSeconds(duration); 
    }
}