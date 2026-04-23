using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using UnityEngine;

namespace Battle
{
    [System.Serializable]
    public class BattleFilterModifiers : BattleSelectionFilter
    {
        [SerializeField] private List<PhysicalBattleEntityModifier> forbiddenModifiers;
    
        public override List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context)
        {
            return context.Where(e => !forbiddenModifiers.Contains(e.physicalBattleEntityModifier)).ToList();
        }
    }
}