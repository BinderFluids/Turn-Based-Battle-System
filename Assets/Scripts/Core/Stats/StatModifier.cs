using System;
using ImprovedTimers;

namespace Core.Stats
{
    public class BasicStatModifier : StatModifier
    {
        private readonly StatType type;
        private readonly Func<int, int> operation; 
        
        public BasicStatModifier(float duration, StatType type, Func<int, int> operation) : base(duration)
        {
            this.type = type;
            this.operation = operation;
        }

        public override void Handle(object sender, Query query)
        {
            if (query.StatType == type)
                query.Value = operation(query.Value);
        }
    }
    
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval { get; set; }
        public event Action<StatModifier> OnDispose = delegate { };
        public abstract void Handle(object sender, Query query);

        private readonly CountdownTimer timer;

        protected StatModifier(float duration)
        {
            if (duration <= 0) return;
            
            timer = new CountdownTimer(duration);
            timer.OnTimerStop += () => MarkedForRemoval = true; 
            timer.Start();
        }
        
        public void Dispose()
        {
            OnDispose?.Invoke(this);
        }
    }
}