namespace Core.Stats
{
    public enum StatType { Attack, Defense }
    
    public class Stats
    {
        private readonly StatBlockTemplate template;
        readonly StatsMediator mediator;
        
        public StatsMediator Mediator => mediator;

        public int Attack
        {
            get
            {
                var q = new Query(StatType.Attack, template.attack);
                mediator.PerformQuery(this, q); 
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(StatType.Defense, template.defense);
                mediator.PerformQuery(this, q); 
                return q.Value;
            }
        }

        public Stats(StatsMediator mediator, StatBlockTemplate template)
        {
            this.mediator = mediator;
            this.template = template;
        }
    }
}