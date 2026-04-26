namespace Core.Stats
{
    public class StatBlock
    {
        readonly StatsMediator mediator;
        public StatsMediator Mediator => mediator;

        public Stat Attack;
        public Stat Defense;
        public Stat Speed; 
        public Resource Health;

        public StatBlock(StatsMediator mediator, StatBlockDefinition definition)
        {
            this.mediator = mediator;
            Attack = new Stat(mediator, StatType.Attack, definition.attack); 
            Defense = new Stat(mediator, StatType.Defense, definition.defense);
            Speed = new Stat(mediator, StatType.Speed, definition.speed);
            Health = new Resource(new Stat(mediator, StatType.Health, definition.health)); 
        }
    }
}