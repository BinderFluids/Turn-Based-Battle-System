using System;
using System.Collections.Generic;
using System.Linq;
using Registry;
using UnityEngine;

public abstract class ScriptableBattleAction : ScriptableObject, IBattleAction
{
    [SerializeField] private InterfaceReference<IBattleActionCounterBehaviour> counterBehaviourRef;
    
    public event Action onActionStarted;
    public event Action onActionEnded;
    
    [SerializeField] private NestedAssetList<BattleSelectionFilter> filters;
    
    public abstract void StartAction(BattleEntity actor, BattleEntity target);
    protected void EndAction(BattleEntity actor)
    {
        onActionEnded?.Invoke();
    }


    public List<BattleEntity> GetValidTargets(BattleEntity actor)
    {
        List<BattleEntity> output = Registry<BattleEntity>.All.ToList();
        IReadOnlyList<BattleSelectionFilter> filters = this.filters.NestedAssets; 

        foreach (BattleSelectionFilter filter in filters)
            output = filter.Filter(actor, output);

        return output; 
    }

    public void Counter(BattleEntity defender)
    {
        throw new NotImplementedException();
    }
}