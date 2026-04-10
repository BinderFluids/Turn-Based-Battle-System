using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BattleFilterModifiers : BattleSelectionFilter
{
    [SerializeField] private List<PhysicalBattleEntityModifier> forbiddenModifiers;
    
    public override List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context)
    {
        return context.Where(e => !forbiddenModifiers.Contains(e.physicalBattleEntityModifier)).ToList();
    }
}