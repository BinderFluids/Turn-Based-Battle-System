namespace StatusEffectSystem
{
    public abstract class StatusEffect
    {
        public virtual void Apply(BattleEntity entity) { }
        public virtual void Remove(BattleEntity entity) { }
        public virtual void OnTurnStart(BattleEntity entity) { }
    }
    
    
}