
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Action/Buildable", fileName = "NewBuildableAction", order = 0)]
public class BuildableBattleAction : ScriptableBattleAction
{
    [Header("Required Dependencies")]
    [SerializeField] private SocketData socketData;
    public SocketData SocketData => socketData;

    [SerializeField] private AnimatorController animatorController;
    public AnimatorController AnimatorController => animatorController;
    
    [Header("Nested Steps")]
    [SerializeField] private NestedAssetList<BuildableBattleActionStep> steps;
    private BattleEntity actor;

    private void OnValidate()
    {
        foreach (BuildableBattleActionStep step in steps.NestedAssets)
            step.Initialize(this); 
    }

    public override void StartAction(BattleEntity actor, BattleEntity target)
    {
        this.actor = actor;
        ExecuteSteps(actor, target).Forget();
    }

    private async UniTaskVoid ExecuteSteps(BattleEntity actor, BattleEntity target)
    {
        foreach (BuildableBattleActionStep step in steps.NestedAssets)
            await step.Execute(actor, target);
        
        EndAction(actor);
    }
}