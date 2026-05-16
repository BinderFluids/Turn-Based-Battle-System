namespace Battle
{
    public interface ITurnHandleStrategy
    {
        void Handle(BattleEntity entity);
    }

    public struct NextTurnHandle : ITurnHandleStrategy
    {
        public void Handle(BattleEntity entity) => BattleManager.Instance.EndTurn();
    }
}