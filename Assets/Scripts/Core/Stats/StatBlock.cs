namespace Core.Stats
{
    
    public class StatBlock
    {
        readonly StatsMediator mediator;
        public StatsMediator Mediator => mediator;

        public Stat Attack;
        public Stat Defense;
        public Resource Health;

        public StatBlock(StatsMediator mediator, StatBlockTemplate template)
        {
            this.mediator = mediator;
            Attack = new Stat(mediator, StatType.Attack, template.attack); 
            Defense = new Stat(mediator, StatType.Defense, template.defense);
        }
    }
}