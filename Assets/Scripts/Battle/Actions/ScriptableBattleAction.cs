using System;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using Battle.Events;
using Battle.Interfaces;
using Battle.TargetSelection;
using SerializedInterface;
using UnityEngine;

namespace Battle.Actions
{
    public abstract class ScriptableBattleAction : ScriptableObject, IBattleAction
    {
        [SerializeField] private InterfaceReference<IBattleActionCounterBehaviour> counterBehaviourRef;
    
        public event Action onActionStarted;
        public event Action onActionEnded;

        [SerializeReference, Subclass(IsList = true)] private List<BattleSelectionFilter> filters;

        public virtual IBattleEntitySelectionStrategy ForcedTargetSelectionStrategy => null; 
        
        public abstract void StartAction(BattleEntity actor, BattleEntity target);
        protected void EndAction(BattleEntity actor)
        {
            onActionEnded?.Invoke();
            EventBus<OnActionEnded>.Raise(new OnActionEnded()
            {
                Entity = actor,
                Action = this
            });
            
            onActionEnded = null;
        }
        
        public List<BattleEntity> GetValidTargets(BattleEntity actor, IEnumerable<BattleEntity> ctx)
        {
            if (filters.Count == 0) return ctx.ToList();  
            
            List<BattleEntity> output = new List<BattleEntity>();
            output.AddRange(ctx);

            foreach (BattleSelectionFilter filter in filters)
                output = filter.Filter(actor, output);

            return output; 
        }
    }
}