
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Action/Buildable", fileName = "NewBuildableAction", order = 0)]
public class BuildableBattleAction : ScriptableBattleAction
{
    [SerializeReference, Subclass(IsList = true)] private List<BuildableBattleActionStep> steps = new();
    private BattleEntity actor;
    
    public override void StartAction(BattleEntity actor, BattleEntity target)
    {
        this.actor = actor;
        ExecuteSteps(actor, target).Forget();
    }

    private async UniTaskVoid ExecuteSteps(BattleEntity actor, BattleEntity target)
    {
        foreach (BuildableBattleActionStep step in steps)
            await step.Execute(actor, target);
        
        EndAction(actor);
    }
}