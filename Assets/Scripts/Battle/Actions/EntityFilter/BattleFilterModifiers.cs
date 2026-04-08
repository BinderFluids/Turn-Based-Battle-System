using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleFilterModifiers : BattleSelectionFilter
{
    [SerializeField] private List<PhysicalBattleEntityModifier> forbiddenModifiers;

    public override string GetNestedAssetName()
    {
        return "Forbidden Modifiers";
    }

    public override List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context)
    {
        return context.Where(e => !forbiddenModifiers.Contains(e.physicalBattleEntityModifier)).ToList();
    }
}