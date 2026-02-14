using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Registry;

public abstract class ScriptableBattleAction : NestedAssetParent, IBattleAction
{
    public event Action onActionStarted;
    public event Action onActionEnded;
    
    public abstract UniTaskVoid Strategy(BattleEntity actor, BattleEntity target);
    public override Type nestedAssetChildType => typeof(BattleSelectionFilter);

    protected void EndAction(BattleEntity actor)
    {
        onActionEnded?.Invoke();
        NextTurnEvent nextTurnEvent = new NextTurnEvent {previousActor = actor};
        EventBus<NextTurnEvent>.Raise(nextTurnEvent);
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