using EventBus;

public struct TurnStartEvent : IEvent
{
    public BattleEntity entity; 
}