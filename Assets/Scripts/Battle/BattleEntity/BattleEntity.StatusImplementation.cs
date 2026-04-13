using System.Collections.Generic;
using System.Linq;
using StatusEffectSystem;

public partial class BattleEntity
{
    StatusEffectHandler statusEffectHandler;

    public void AddStatusEffect(StatusEffect statusEffect) => statusEffectHandler.AddStatus(statusEffect); 
    public void RemoveStatusEffect(StatusEffect statusEffect) => statusEffectHandler.RemoveStatus(statusEffect);
}