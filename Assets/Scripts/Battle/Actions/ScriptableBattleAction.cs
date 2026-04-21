using System;
using System.Collections.Generic;
using SerializedInterface;
using UnityEngine;

public abstract class ScriptableBattleAction : ScriptableObject, IBattleAction
{
    [SerializeField] private InterfaceReference<IBattleActionCounterBehaviour> counterBehaviourRef;
    
    public event Action onActionStarted;
    public event Action onActionEnded;
    
    [SerializeReference, Subclass(IsList = true)] private List<BattleSelectionFilter> filters; 
    
    public abstract void StartAction(BattleEntity actor, BattleEntity target);
    protected void EndAction(BattleEntity actor)
    {
        onActionEnded?.Invoke();
    }


    public List<BattleEntity> GetValidTargets(BattleEntity actor, IEnumerable<BattleEntity> ctx)
    {
        List<BattleEntity> output = new List<BattleEntity>();
        output.AddRange(ctx);

        foreach (BattleSelectionFilter filter in filters)
            output = filter.Filter(actor, output);

        return output; 
    }
}