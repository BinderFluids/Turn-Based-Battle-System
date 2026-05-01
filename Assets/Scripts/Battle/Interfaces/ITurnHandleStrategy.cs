namespace Battle
{
    public interface ITurnHandleStrategy
    {
        void Handle(BattleEntity entity);
    }

    public struct EmptyTurnHandle : ITurnHandleStrategy
    {
        public void Handle(BattleEntity entity)
        {
        
        }
    }
}