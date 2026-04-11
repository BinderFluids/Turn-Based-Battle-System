namespace Core.Stats
{
    public enum StatType { Attack, Defense }
    
    public class Stats
    {
        private readonly StatBlockTemplate template;
        readonly StatsMediator mediator;
        
        public StatsMediator Mediator => mediator;

        public Stat Attack;
        public Stat Defense;

        public Stats(StatsMediator mediator, StatBlockTemplate template)
        {
            this.mediator = mediator;
            this.template = template;
        }
    }
    
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
}