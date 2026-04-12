namespace Core.Stats
{
    public class Stat
    {
        private int value;

        public int Value
        {
            get
            {
                var q = new Query(StatType.Attack, value);
                mediator.PerformQuery(this, q); 
                return q.Value;
            }
        }

        private StatType type; 
        private StatsMediator mediator;
            
            
        public Stat(StatsMediator mediator, StatType type, int value)
        {
            this.mediator = mediator;
            this.type = type;
            this.value = value;
        }
    }

    public class StatResource : Stat
    {
        private int currentValue; 
        
        
        public StatResource(StatsMediator mediator, StatType type, int value) : base(mediator, type, value)
        {
        }
    }
}