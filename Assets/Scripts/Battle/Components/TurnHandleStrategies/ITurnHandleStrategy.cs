namespace Battle.Components.TurnHandleStrategies
{
    public interface ITurnHandleStrategy
    {
        void Handle(TurnComponent component);
    }

    public struct EmptyTurnHandle : ITurnHandleStrategy
    {
        public void Handle(TurnComponent component)
        {
        
        }
    }
}