namespace StatusEffectSystem
{
    public abstract class StatusEffect
    {
        private int duration;

        public StatusEffect(int duration)
        {
            this.duration = duration;
        }
        
        public virtual void Apply(BattleEntity entity) { }
        public virtual void Remove(BattleEntity entity) { }

        public void TurnStart(BattleEntity entity)
        {
            OnTurnStart(entity); 
            
            duration--;
            if (duration == 0)
                entity.RemoveStatusEffect(this); 
        }
        protected virtual void OnTurnStart(BattleEntity entity) { }
    }
    
    
}