using System;
using System.Collections.Generic;
using System.Linq;
using Registry;
using UnityEngine;

public abstract class ScriptableBattleAction : NestedAssetParent, IBattleAction
{
    [SerializeField] private InterfaceReference<IBattleActionCounterBehaviour> counterBehaviourRef;
    
    public event Action onActionStarted;
    public event Action onActionEnded;
    
    public override Type nestedAssetChildType => typeof(BattleSelectionFilter);

    
    public abstract void StartAction(BattleEntity actor, BattleEntity target);
    protected void EndAction(BattleEntity actor)
    {
        onActionEnded?.Invoke();
    }


    public List<BattleEntity> GetValidTargets(BattleEntity actor)
    {
        List<BattleEntity> output = Registry<BattleEntity>.All.ToList();
        List<BattleSelectionFilter> assetsAsFilters = NestedChildren.Cast<BattleSelectionFilter>().ToList();

        foreach (BattleSelectionFilter filter in assetsAsFilters)
            output = filter.Filter(actor, output);

        return output; 
    }

    public void Counter(BattleEntity defender)
    {
        throw new NotImplementedException();
    }
}