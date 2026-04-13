using StatusEffectSystem;

public class StatusHandlerComponent : BattleEntityComponent
{
    StatusEffectHandler statusEffectHandler;

    public void AddStatusEffect(StatusEffect statusEffect) => statusEffectHandler.AddStatus(statusEffect); 
    public void RemoveStatusEffect(StatusEffect statusEffect) => statusEffectHandler.RemoveStatus(statusEffect);
}