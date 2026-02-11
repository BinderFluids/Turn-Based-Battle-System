using System.Collections.Generic;
using System.Linq;

public class BattleEntityModifierFilter : IProcessor<List<BattleEntity>, List<BattleEntity>>
{
    private List<PhysicalBattleEntityModifier> forbiddenModifiers; 
    
    public BattleEntityModifierFilter(List<PhysicalBattleEntityModifier> forbiddenModifiers) => this.forbiddenModifiers = forbiddenModifiers;

    public List<BattleEntity> Process(List<BattleEntity> entities)
    {
        return entities.Where(e => !forbiddenModifiers.Contains(e.physicalBattleEntityModifier)).ToList();
    }
}