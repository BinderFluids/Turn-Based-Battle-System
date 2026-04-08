using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Registry;

public abstract class ScriptableBattleAction : NestedAssetParent, IBattleAction
{
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
}