namespace Battle.Interfaces
{
    public interface IDodgeBehaviour
    {
        void UpdateDodge(BattleEntity entity); 
    }

    public interface IDodgeFactory
    {
        public IDodgeBehaviour GetDodgeBehaviour();
    }
}