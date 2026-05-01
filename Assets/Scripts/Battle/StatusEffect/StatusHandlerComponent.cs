using Battle.Enums;

namespace Battle.StatusEffect
{
    public class StatusHandlerComponent : BattleEntityComponent
    {
        StatusEffectHandler statusEffectHandler;
        protected override ComponentType componentType => ComponentType.StatusHandler; 

        public void AddStatusEffect(StatusEffect statusEffect) => statusEffectHandler.AddStatus(statusEffect); 
        public void RemoveStatusEffect(StatusEffect statusEffect) => statusEffectHandler.RemoveStatus(statusEffect);
    }
}