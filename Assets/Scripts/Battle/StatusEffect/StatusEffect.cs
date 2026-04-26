using Battle;

namespace Battle.StatusEffect
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
                if (entity.TryGetComponent(out StatusHandlerComponent statusHandler))
                    statusHandler.RemoveStatusEffect(this); 
        }
        protected virtual void OnTurnStart(BattleEntity entity) { }
    }
    
    
}