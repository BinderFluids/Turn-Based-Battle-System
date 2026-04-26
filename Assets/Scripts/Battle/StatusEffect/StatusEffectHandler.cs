using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Events;
using EventBus;

namespace Battle.StatusEffect
{
    public class StatusEffectHandler
    {
        private BattleEntity target;
        private List<StatusEffect> statusEffects = new(); 
        public IReadOnlyList<StatusEffect> StatusEffects => statusEffects;

        EventBinding<TurnStartEvent> turnStartBinding;
        
        public StatusEffectHandler(BattleEntity target)
        {
            this.target = target; 
            
            turnStartBinding = new EventBinding<TurnStartEvent>(OnTurnStart);
            EventBus<TurnStartEvent>.Register(turnStartBinding);
        }
        
        public void AddStatus(StatusEffect status)
        {
            if (statusEffects.Any(s => s.GetType() == status.GetType())) return; 
    
            statusEffects.Add(status);
            status.Apply(target); 
        }
        public void RemoveStatus(StatusEffect status)
        {
            if (!statusEffects.Contains(status)) return;
            if (status is null) return;
    
            statusEffects.Remove(status);
            status.Remove(target);
        }

        void OnTurnStart(TurnStartEvent e)
        {
            if (e.Entity != target) return;
            for (int i = statusEffects.Count - 1; i >= 0; i--)
                statusEffects[i].TurnStart(target); 
        }
    }
}