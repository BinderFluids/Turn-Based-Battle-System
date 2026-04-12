
using EventBus;

public struct TurnEndEvent : IEvent
{
    public BattleEntity entity; 
}